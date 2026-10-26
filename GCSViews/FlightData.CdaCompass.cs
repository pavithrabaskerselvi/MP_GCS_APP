using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MissionPlanner.Utilities;

namespace MissionPlanner.GCSViews
{
    // ------------------------------------------------------------------
    //  CDA compass for the Flight Data map (new purple image version).
    //   * round compass picture, rotates with aircraft yaw
    //   * DOUBLE CLICK  -> toggles  PINNED (stays in place)  <->  FREE (drag anywhere)
    //   * position + pinned state are remembered in Settings
    // ------------------------------------------------------------------
    public class CdaCompass : Control
    {
        static readonly Color Purple = Color.FromArgb(107, 78, 255);

        static Bitmap _img;                 // loaded once from CdaCompassImage.cs

        double _heading;
        bool _pinned;
        bool _dragging;
        Point _dragStartMouse;     // screen coords
        Point _dragStartLoc;       // control location at drag start

        [Browsable(false)]
        public double Heading
        {
            get { return _heading; }
            set
            {
                double v = double.IsNaN(value) ? 0 : ((value % 360) + 360) % 360;
                if (Math.Abs(v - _heading) < 0.2) return;   // ignore tiny jitter, saves repaints
                _heading = v;
                Invalidate();
            }
        }

        [Browsable(false)]
        public bool Pinned
        {
            get { return _pinned; }
            set
            {
                _pinned = value;
                Cursor = _pinned ? Cursors.Default : Cursors.SizeAll;
                Invalidate();
            }
        }

        public CdaCompass()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Size = new Size(96, 96);
            Pinned = false;
            TabStop = false;

            try { if (_img == null) _img = CdaCompassImage.Load(); } catch { }
        }

        // ---------- round shape ----------
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var gp = new GraphicsPath();
            gp.AddEllipse(0, 0, Width, Height);
            var old = Region;
            Region = new Region(gp);
            if (old != null) old.Dispose();
            gp.Dispose();
        }

        // ---------- drawing ----------
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float s = Math.Min(Width, Height);
            float c = s / 2f;

            // ---- rotating compass picture ----
            if (_img != null)
            {
                var st = g.Save();
                g.TranslateTransform(c, c);
                g.RotateTransform((float)-_heading);
                g.DrawImage(_img, -c, -c, s, s);
                g.Restore(st);
            }
            else
            {
                using (var p = new Pen(Purple, 3f))
                    g.DrawEllipse(p, 1.5f, 1.5f, s - 3f, s - 3f);
            }

            // ---- fixed heading pointer at the top (does not rotate) ----
            var tri = new[] { new PointF(c - 5, 0), new PointF(c + 5, 0), new PointF(c, 9) };
            using (var b = new SolidBrush(Color.FromArgb(60, 40, 150)))
                g.FillPolygon(b, tri);
            using (var p = new Pen(Color.White, 1f))
                g.DrawPolygon(p, tri);

            // ---- pin indicator: filled dot = pinned, hollow = free to move ----
            float d = 6f;
            float py = s - d - 6f;
            using (var p = new Pen(Color.White, 1.5f))
            {
                if (_pinned)
                {
                    using (var b = new SolidBrush(Purple))
                        g.FillEllipse(b, c - d / 2, py, d, d);
                    g.DrawEllipse(p, c - d / 2, py, d, d);
                }
                else
                    g.DrawEllipse(p, c - d / 2, py, d, d);
            }
        }

        // ---------- mouse ----------
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && !_pinned)
            {
                _dragging = true;
                _dragStartMouse = Cursor.Position;
                _dragStartLoc = Location;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!_dragging || Parent == null) return;

            int nx = _dragStartLoc.X + (Cursor.Position.X - _dragStartMouse.X);
            int ny = _dragStartLoc.Y + (Cursor.Position.Y - _dragStartMouse.Y);
            Location = Clamp(new Point(nx, ny));
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (_dragging)
            {
                _dragging = false;
                SaveState();
            }
        }

        // double click = pin / un-pin
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            _dragging = false;
            Pinned = !Pinned;
            SaveState();
        }

        // ---------- helpers ----------
        Point Clamp(Point p)
        {
            if (Parent == null) return p;
            int maxX = Math.Max(0, Parent.ClientSize.Width - Width);
            int maxY = Math.Max(0, Parent.ClientSize.Height - Height);
            return new Point(Math.Min(Math.Max(0, p.X), maxX), Math.Min(Math.Max(0, p.Y), maxY));
        }

        public void RestoreState(Point defaultLocation)
        {
            int x, y;
            if (!int.TryParse(Settings.Instance["CdaCompassX"], out x)) x = defaultLocation.X;
            if (!int.TryParse(Settings.Instance["CdaCompassY"], out y)) y = defaultLocation.Y;
            Location = Clamp(new Point(x, y));

            bool pinned;
            if (!bool.TryParse(Settings.Instance["CdaCompassPinned"], out pinned)) pinned = false;
            Pinned = pinned;

            // keep it inside the map area when the window is resized / panels hidden
            if (Parent != null)
                Parent.Resize += (s, e) => Location = Clamp(Location);
        }

        void SaveState()
        {
            try
            {
                Settings.Instance["CdaCompassX"] = Left.ToString();
                Settings.Instance["CdaCompassY"] = Top.ToString();
                Settings.Instance["CdaCompassPinned"] = _pinned.ToString();
            }
            catch { }
        }
    }

    // Hook part of FlightData
    public partial class FlightData
    {
        private CdaCompass _cdaCompass;

        // Call ONE time in the FlightData constructor, right after InitCdaLayout();
        private void InitCdaCompass()
        {
            // old white wind-direction circle is replaced (kept alive, only hidden, so nothing else breaks)
            windDir1.Visible = false;

            _cdaCompass = new CdaCompass { Name = "cdaCompass" };
            // heading comes straight from the aircraft yaw
            _cdaCompass.DataBindings.Add(new Binding("Heading", bindingSource1, "yaw", true,
                DataSourceUpdateMode.Never));

            splitContainer1.Panel2.Controls.Add(_cdaCompass);
            _cdaCompass.BringToFront();
            _cdaCompass.RestoreState(new Point(4, 4));
        }
    }
}