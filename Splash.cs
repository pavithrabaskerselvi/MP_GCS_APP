using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace MissionPlanner
{
    public partial class Splash : Form
    {
        private Image _bg;
        private Bitmap _cache;          // background pre-scaled to screen size (drawn once)
        private float _scale, _ox, _oy;
        private Rectangle _barDirty;    // only this area is repainted by the timer
        private Size _cacheSize = Size.Empty;

        // Compatibility: Program.cs sets Splash.pictureBox1 (not shown on screen)
        public PictureBox pictureBox1 = new PictureBox();

        private readonly Timer _timer = new Timer();
        private readonly Stopwatch _clock = new Stopwatch();

        // Time taken for the bar to go 0 -> 100
        public int DurationMs = 3000;

        private float _progress = 0f;   // 0..1

        // Baked-in loading bar position in the original image (1672 x 941)
        private static readonly RectangleF BarInImage = new RectangleF(678, 438, 306, 18);

        public Splash()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.FromArgb(40, 12, 90);
            Bounds = Screen.PrimaryScreen.Bounds;

            LoadBackground();

            _timer.Interval = 33; // ~30 fps is plenty for a bar
            _timer.Tick += (s, e) =>
            {
                float t = Math.Min(1f, (float)_clock.ElapsedMilliseconds / DurationMs);
                _progress = t * t * (3f - 2f * t);   // smooth ease-in-out
                if (!_barDirty.IsEmpty) Invalidate(_barDirty);
            };
        }

        // Start/stop animation with visibility (restarts from 0% each time it is shown)
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
            {
                _progress = 0f;
                _clock.Restart();
                _timer.Start();
            }
            else
            {
                _timer.Stop();
                _clock.Stop();
            }
        }

        private void LoadBackground()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "splashbg.png");
                if (File.Exists(path))
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var tmp = Image.FromStream(fs))
                        _bg = new Bitmap(tmp);
                }
            }
            catch { _bg = null; }
        }

        // Scale the background ONCE to the current screen size ("cover", no stretching)
        private void EnsureCache()
        {
            if (_bg == null || ClientSize.Width <= 0 || ClientSize.Height <= 0) return;
            if (_cache != null && _cacheSize == ClientSize) return;

            if (_cache != null) _cache.Dispose();
            _cacheSize = ClientSize;

            _scale = Math.Max((float)ClientSize.Width / _bg.Width,
                              (float)ClientSize.Height / _bg.Height);
            float w = _bg.Width * _scale;
            float h = _bg.Height * _scale;
            _ox = (ClientSize.Width - w) / 2f;
            _oy = (ClientSize.Height - h) / 2f;

            _cache = new Bitmap(ClientSize.Width, ClientSize.Height, PixelFormat.Format32bppPArgb);
            using (var cg = Graphics.FromImage(_cache))
            {
                cg.InterpolationMode = InterpolationMode.HighQualityBicubic;
                cg.DrawImage(_bg, _ox, _oy, w, h);
            }

            // Area the bar + glow + percentage text can touch
            var bar = GetBarRect();
            _barDirty = Rectangle.Ceiling(new RectangleF(
                bar.X - 24f * _scale,
                bar.Y - 14f * _scale,
                bar.Width + 140f * _scale,
                bar.Height + 28f * _scale));
        }

        private RectangleF GetBarRect()
        {
            float pad = 3f * _scale;   // slightly bigger than baked bar so it hides it
            return new RectangleF(
                _ox + BarInImage.X * _scale - pad,
                _oy + BarInImage.Y * _scale - pad,
                BarInImage.Width * _scale + pad * 2,
                BarInImage.Height * _scale + pad * 2);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            if (_bg == null)
            {
                using (var br = new LinearGradientBrush(ClientRectangle,
                    Color.FromArgb(60, 20, 140), Color.FromArgb(150, 80, 220), 90f))
                    g.FillRectangle(br, ClientRectangle);
                return;
            }

            EnsureCache();
            if (_cache == null) return;

            // Fast: only copies the invalidated region
            g.CompositingMode = CompositingMode.SourceCopy;
            g.DrawImage(_cache, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);
            g.CompositingMode = CompositingMode.SourceOver;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            DrawLoadingBar(g);
        }

        private void DrawLoadingBar(Graphics g)
        {
            var bar = GetBarRect();
            float r = bar.Height / 2f;

            // Track
            using (var path = RoundedRect(bar, r))
            using (var br = new SolidBrush(Color.FromArgb(245, 232, 222, 250)))
            using (var pen = new Pen(Color.FromArgb(120, 124, 58, 237), 1.2f))
            {
                g.FillPath(br, path);
                g.DrawPath(pen, path);
            }

            // Fill
            float fillW = Math.Max(bar.Height, bar.Width * _progress);
            var fill = new RectangleF(bar.X, bar.Y, fillW, bar.Height);

            // Glow behind the fill
            for (int i = 3; i >= 1; i--)
            {
                var glow = RectangleF.Inflate(fill, i * 3f * _scale, i * 2f * _scale);
                using (var gp = RoundedRect(glow, glow.Height / 2f))
                using (var gb = new SolidBrush(Color.FromArgb(22, 168, 85, 247)))
                    g.FillPath(gb, gp);
            }

            using (var fp = RoundedRect(fill, r))
            {
                using (var fb = new LinearGradientBrush(
                    new RectangleF(fill.X - 1, fill.Y, fill.Width + 2, fill.Height),
                    Color.FromArgb(91, 33, 182), Color.FromArgb(217, 70, 239), 0f))
                    g.FillPath(fb, fp);

                var state = g.Save();
                g.SetClip(fp, CombineMode.Intersect);

                // Moving shine
                float shineW = bar.Height * 3f;
                float shineX = fill.X + ((_clock.ElapsedMilliseconds % 1100) / 1100f) * (fill.Width + shineW) - shineW;
                var shine = new RectangleF(shineX, fill.Y, shineW, fill.Height);
                using (var sb = new LinearGradientBrush(shine, Color.White, Color.White, 0f))
                {
                    var blend = new ColorBlend(3);
                    blend.Positions = new[] { 0f, 0.5f, 1f };
                    blend.Colors = new[] {
                        Color.FromArgb(0, 255, 255, 255),
                        Color.FromArgb(160, 255, 255, 255),
                        Color.FromArgb(0, 255, 255, 255) };
                    sb.InterpolationColors = blend;
                    g.FillRectangle(sb, shine);
                }

                // Top gloss
                using (var glb = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
                    g.FillRectangle(glb, fill.X, fill.Y, fill.Width, fill.Height / 2f);

                g.Restore(state);
            }

            // Percentage text
            int pct = (int)Math.Round(_progress * 100f);
            float fontSize = Math.Max(10f, 15f * _scale);
            using (var f = new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var tb = new SolidBrush(Color.FromArgb(76, 29, 149)))
            {
                string txt = pct + "%";
                SizeF sz = g.MeasureString(txt, f);
                g.DrawString(txt, f, tb, bar.Right + 12f * _scale, bar.Y + (bar.Height - sz.Height) / 2f);
            }
        }

        private static GraphicsPath RoundedRect(RectangleF rc, float radius)
        {
            float d = Math.Min(radius * 2f, Math.Min(rc.Width, rc.Height));
            var p = new GraphicsPath();
            p.AddArc(rc.X, rc.Y, d, d, 180, 90);
            p.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
            p.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
            p.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            if (_cache != null) _cache.Dispose();
            if (_bg != null) _bg.Dispose();
            base.OnFormClosed(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e) { }
    }
}