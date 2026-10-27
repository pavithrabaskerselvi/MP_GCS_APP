```csharp
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MissionPlanner
{
    public partial class Splash : Form
    {
        private Image _bg;
        private Bitmap _cache;
        private float _scale, _ox, _oy;
        private Rectangle _barDirty;
        private Size _cacheSize = Size.Empty;

        public PictureBox pictureBox1 = new PictureBox();

        private readonly Timer _timer = new Timer();
        private readonly Stopwatch _clock = new Stopwatch();

        public int DurationMs = 3000;
        private float _progress = 0f;

        private static readonly RectangleF BarInImage =
            new RectangleF(678, 438, 306, 18);

        public Splash()
        {
            InitializeComponent();

            TXT_version.Visible = false;
            label1.Visible = false;
            pictureBox1.Visible = false;

            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer, true);

            FormBorderStyle = FormBorderStyle.None;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;

            BackColor = Color.FromArgb(40, 12, 90);

#if NETSTANDARD2_0
            MinimumSize = Size.Empty;
            MaximumSize = Size.Empty;
#endif

            Bounds = Screen.PrimaryScreen.Bounds;

            LoadBackground();

            _timer.Interval = 33;
            _timer.Tick += (s, e) =>
            {
                float t = DurationMs <= 0 ? 1f :
                    Math.Min(1f,
                        (float)_clock.ElapsedMilliseconds / DurationMs);

                _progress = t * t * (3f - 2f * t);

                if (!_barDirty.IsEmpty)
                    Invalidate(_barDirty);
            };
        }

        private void LoadBackground()
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();

                foreach (var name in asm.GetManifestResourceNames())
                {
                    if (name.EndsWith(
                        "splashbg.png",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        using (var stream =
                            asm.GetManifestResourceStream(name))
                        using (var image = Image.FromStream(stream))
                            _bg = new Bitmap(image);

                        return;
                    }
                }
            }
            catch
            {
                _bg = null;
            }

            string[] paths =
            {
                Path.Combine(
                    Application.StartupPath ?? "", "splashbg.png"),
                Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory ?? "",
                    "splashbg.png"),
                @"D:\MP_GCS\MP_GCS_APP\splashbg.png"
            };

            foreach (var path in paths)
            {
                try
                {
                    if (File.Exists(path))
                    {
                        using (var image = Image.FromFile(path))
                            _bg = new Bitmap(image);

                        return;
                    }
                }
                catch
                {
                    // Try the next location.
                }
            }

            try
            {
                _bg = SplashBgData.Load();
            }
            catch
            {
                _bg = null;
            }
        }

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

        private void EnsureCache()
        {
            if (_bg == null ||
                ClientSize.Width <= 0 ||
                ClientSize.Height <= 0)
                return;

            if (_cache != null && _cacheSize == ClientSize)
                return;

            if (_cache != null)
                _cache.Dispose();

            _cacheSize = ClientSize;

            _scale = Math.Max(
                (float)ClientSize.Width / _bg.Width,
                (float)ClientSize.Height / _bg.Height);

            float w = _bg.Width * _scale;
            float h = _bg.Height * _scale;

            _ox = (ClientSize.Width - w) / 2f;
            _oy = (ClientSize.Height - h) / 2f;

            _cache = new Bitmap(
                ClientSize.Width,
                ClientSize.Height,
                PixelFormat.Format32bppPArgb);

            using (var graphics = Graphics.FromImage(_cache))
            {
                graphics.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

                graphics.DrawImage(_bg, _ox, _oy, w, h);
            }

            var bar = GetBarRect();

            _barDirty = Rectangle.Ceiling(new RectangleF(
                bar.X - 24f * _scale,
                bar.Y - 14f * _scale,
                bar.Width + 140f * _scale,
                bar.Height + 28f * _scale));
        }

        private RectangleF GetBarRect()
        {
            float pad = 3f * _scale;

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

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Painting is handled in OnPaint.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;

            if (_bg == null)
            {
                using (var brush = new LinearGradientBrush(
                    ClientRectangle,
                    Color.FromArgb(60, 20, 140),
                    Color.FromArgb(150, 80, 220),
                    90f))
                {
                    graphics.FillRectangle(brush, ClientRectangle);
                }

                return;
            }

            EnsureCache();

            if (_cache == null)
                return;

            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.DrawImage(
                _cache,
                e.ClipRectangle,
                e.ClipRectangle,
                GraphicsUnit.Pixel);
            graphics.CompositingMode = CompositingMode.SourceOver;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            DrawLoadingBar(graphics);
        }

        private void DrawLoadingBar(Graphics graphics)
        {
            var bar = GetBarRect();
            float radius = bar.Height / 2f;

            using (var path = RoundedRect(bar, radius))
            using (var brush = new SolidBrush(
                Color.FromArgb(245, 232, 222, 250)))
            using (var pen = new Pen(
                Color.FromArgb(120, 124, 58, 237), 1.2f))
            {
                graphics.FillPath(brush, path);
                graphics.DrawPath(pen, path);
            }

            float fillWidth = bar.Width * _progress;

            if (fillWidth <= 0)
                return;

            var fill = new RectangleF(
                bar.X, bar.Y, fillWidth, bar.Height);

            for (int i = 3; i >= 1; i--)
            {
                var glow = RectangleF.Inflate(
                    fill, i * 3f * _scale, i * 2f * _scale);

                using (var path = RoundedRect(
                    glow, glow.Height / 2f))
                using (var brush = new SolidBrush(
                    Color.FromArgb(22, 168, 85, 247)))
                {
                    graphics.FillPath(brush, path);
                }
            }

            using (var path = RoundedRect(fill, radius))
            using (var brush = new LinearGradientBrush(
                new RectangleF(
                    fill.X, fill.Y,
                    Math.Max(1f, fill.Width), fill.Height),
                Color.FromArgb(91, 33, 182),
                Color.FromArgb(217, 70, 239),
                0f))
            {
                graphics.FillPath(brush, path);
            }

            int percentage = (int)Math.Round(_progress * 100f);

            using (var font = new Font(
                "Segoe UI",
                Math.Max(10f, 15f * _scale),
                FontStyle.Bold,
                GraphicsUnit.Pixel))
            using (var brush = new SolidBrush(
                Color.FromArgb(76, 29, 149)))
            {
                string text = percentage + "%";

                graphics.DrawString(
                    text,
                    font,
                    brush,
                    bar.Right + 12f * _scale,
                    bar.Y);
            }
        }

        private static GraphicsPath RoundedRect(
            RectangleF rectangle, float radius)
        {
            float diameter = Math.Min(
                radius * 2f,
                Math.Min(rectangle.Width, rectangle.Height));

            var path = new GraphicsPath();

            path.AddArc(
                rectangle.X, rectangle.Y,
                diameter, diameter, 180, 90);

            path.AddArc(
                rectangle.Right - diameter,
                rectangle.Y,
                diameter, diameter, 270, 90);

            path.AddArc(
                rectangle.Right - diameter,
                rectangle.Bottom - diameter,
                diameter, diameter, 0, 90);

            path.AddArc(
                rectangle.X,
                rectangle.Bottom - diameter,
                diameter, diameter, 90, 90);

            path.CloseFigure();

            return path;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();

            if (_cache != null)
                _cache.Dispose();

            if (_bg != null)
                _bg.Dispose();

            base.OnFormClosed(e);
        }
    }
}
```