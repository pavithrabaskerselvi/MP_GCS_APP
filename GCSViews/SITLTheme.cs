using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    /// <summary>Chennai Drone Academy palette used by the SITL page.</summary>
    internal static class CdaTheme
    {
        public static readonly Color Page = Color.FromArgb(240, 237, 252);
        public static readonly Color Card = Color.FromArgb(248, 246, 254);
        public static readonly Color CardBorder = Color.FromArgb(214, 206, 248);
        public static readonly Color Primary = Color.FromArgb(108, 77, 246);
        public static readonly Color PrimaryDark = Color.FromArgb(79, 55, 214);
        public static readonly Color PrimaryLight = Color.FromArgb(150, 124, 250);
        public static readonly Color Text = Color.FromArgb(58, 40, 160);
        public static readonly Color TextMuted = Color.FromArgb(120, 110, 170);
        public static readonly Color Field = Color.FromArgb(246, 244, 254);

        public static Font Ui(float size, FontStyle style = FontStyle.Regular)
        {
            return new Font("Segoe UI", size, style, GraphicsUnit.Point);
        }

        public static GraphicsPath Round(Rectangle r, int radius)
        {
            int d = Math.Max(2, radius * 2);
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        /// <summary>Keeps a standard control on our colours even if ThemeManager recolours it.</summary>
        public static void Guard(Control c, Color back, Color fore)
        {
            c.BackColor = back;
            c.ForeColor = fore;
            c.BackColorChanged += (s, e) => { if (c.BackColor != back) c.BackColor = back; };
            c.ForeColorChanged += (s, e) => { if (c.ForeColor != fore) c.ForeColor = fore; };
        }

        /// <summary>Guard a control and all its internal children (NumericUpDown owns a TextBox).</summary>
        public static void GuardDeep(Control c, Color back, Color fore)
        {
            Guard(c, back, fore);
            foreach (Control child in c.Controls)
                GuardDeep(child, back, fore);
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        public static void SetCue(TextBox t, string cue)
        {
            EventHandler apply = (s, e) => SendMessage(t.Handle, 0x1501, (IntPtr)1, cue);
            if (t.IsHandleCreated) apply(null, null);
            t.HandleCreated += apply;
        }
    }

    /// <summary>Icons drawn in code (purple line-art style), so no resource names are needed.</summary>
    internal static class CdaIcons
    {
        public enum Kind { Plane, Car, Quad, Heli }

        public static Bitmap Make(Kind kind, int size, Color color)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.ScaleTransform(size / 100f, size / 100f);
                using (var fill = new SolidBrush(color))
                using (var pen = new Pen(color, 5f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                using (var light = new SolidBrush(Color.FromArgb(120, color)))
                {
                    switch (kind)
                    {
                        case Kind.Plane:
                            g.FillPolygon(fill, new[]
                            {
                                new PointF(50, 4), new PointF(57, 30), new PointF(96, 58), new PointF(96, 68),
                                new PointF(57, 58), new PointF(55, 82), new PointF(72, 92), new PointF(72, 98),
                                new PointF(50, 92), new PointF(28, 98), new PointF(28, 92), new PointF(45, 82),
                                new PointF(43, 58), new PointF(4, 68), new PointF(4, 58), new PointF(43, 30)
                            });
                            break;
                        case Kind.Quad:
                            g.DrawLine(pen, 22, 22, 78, 78);
                            g.DrawLine(pen, 78, 22, 22, 78);
                            foreach (var c in new[] { new PointF(20, 20), new PointF(80, 20), new PointF(20, 80), new PointF(80, 80) })
                            {
                                g.FillEllipse(light, c.X - 15, c.Y - 15, 30, 30);
                                g.DrawEllipse(pen, c.X - 15, c.Y - 15, 30, 30);
                            }
                            using (var p = CdaTheme.Round(new Rectangle(38, 38, 24, 24), 7))
                                g.FillPath(fill, p);
                            break;
                        case Kind.Heli:
                            g.DrawLine(pen, 8, 20, 92, 20);
                            g.DrawLine(pen, 50, 20, 50, 32);
                            g.FillEllipse(fill, 24, 32, 44, 28);
                            g.FillPolygon(fill, new[] { new PointF(64, 40), new PointF(94, 36), new PointF(94, 44), new PointF(64, 52) });
                            g.DrawEllipse(pen, 84, 28, 14, 14);
                            g.DrawLine(pen, 28, 70, 66, 70);
                            g.DrawLine(pen, 38, 60, 36, 70);
                            g.DrawLine(pen, 56, 60, 58, 70);
                            break;
                        case Kind.Car:
                            using (var p = CdaTheme.Round(new Rectangle(6, 46, 88, 24), 8))
                                g.FillPath(fill, p);
                            g.FillPolygon(fill, new[] { new PointF(26, 48), new PointF(38, 28), new PointF(66, 28), new PointF(78, 48) });
                            g.FillEllipse(Brushes.White, 24, 60, 22, 22);
                            g.FillEllipse(Brushes.White, 56, 60, 22, 22);
                            g.DrawEllipse(pen, 24, 60, 22, 22);
                            g.DrawEllipse(pen, 56, 60, 22, 22);
                            break;
                    }
                }
            }
            return bmp;
        }
    }

    /// <summary>Panel that ignores ThemeManager: always transparent.</summary>
    internal class CdaSurface : Panel
    {
        public CdaSurface()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }
    }

    /// <summary>TableLayoutPanel that ignores ThemeManager. Opaque = lavender page colour, else transparent.</summary>
    internal class CdaTable : TableLayoutPanel
    {
        public bool Opaque { get; set; }
        public CdaTable()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
        }
        public override Color BackColor
        {
            get { return Opaque ? CdaTheme.Page : Color.Transparent; }
            set { }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Opaque) e.Graphics.Clear(CdaTheme.Page);
            else base.OnPaintBackground(e);
        }
    }

    /// <summary>Label with fixed colours (ThemeManager cannot recolour it).</summary>
    internal class CdaLabel : Label
    {
        public Color TextColor { get; set; } = CdaTheme.Text;
        public CdaLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }
        public override Color ForeColor { get { return TextColor; } set { } }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis;
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, TextColor, flags);
        }
    }

    /// <summary>Rounded input holder: wraps a TextBox / ComboBox / NumericUpDown.</summary>
    internal class CdaField : Panel
    {
        public CdaField(Control inner)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Padding = new Padding(9, 5, 5, 0);
            inner.Dock = DockStyle.Top;
            Controls.Add(inner);
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = CdaTheme.Round(rect, 8))
            {
                using (var br = new SolidBrush(CdaTheme.Field)) g.FillPath(br, path);
                using (var pen = new Pen(CdaTheme.CardBorder)) g.DrawPath(pen, path);
            }
        }
    }

    /// <summary>Rounded lavender card with a soft border and optional vertical gradient.</summary>
    internal class CdaCard : Panel
    {
        public int Radius { get; set; } = 14;
        public bool Gradient { get; set; } = false;
        public Color Top { get; set; } = Color.FromArgb(250, 248, 255);
        public Color Bottom { get; set; } = Color.FromArgb(236, 232, 252);

        public CdaCard()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = CdaTheme.Round(rect, Radius))
            {
                if (Gradient)
                {
                    using (var br = new LinearGradientBrush(rect, Top, Bottom, 90f))
                        g.FillPath(br, path);
                    DrawScenery(g, rect);
                }
                else
                {
                    using (var br = new SolidBrush(CdaTheme.Card))
                        g.FillPath(br, path);
                }
                using (var pen = new Pen(CdaTheme.CardBorder, 1.2f))
                    g.DrawPath(pen, path);
            }
            base.OnPaint(e);
        }

        // soft hills + pines along the bottom, like the banner in the design
        private void DrawScenery(Graphics g, Rectangle r)
        {
            var state = g.Save();
            using (var clip = CdaTheme.Round(r, Radius))
                g.SetClip(clip, CombineMode.Intersect);
            int h = r.Height;
            using (var br = new SolidBrush(Color.FromArgb(70, 150, 124, 250)))
            {
                var pts = new[]
                {
                    new Point(0, r.Bottom), new Point(0, r.Bottom - h / 3), new Point(r.Width / 8, r.Bottom - h / 2),
                    new Point(r.Width / 4, r.Bottom - h / 4), new Point(r.Width * 3 / 4, r.Bottom - h / 4),
                    new Point(r.Width * 7 / 8, r.Bottom - h / 2), new Point(r.Width, r.Bottom - h / 3), new Point(r.Width, r.Bottom)
                };
                g.FillPolygon(br, pts);
            }
            using (var br = new SolidBrush(Color.FromArgb(90, 108, 77, 246)))
            {
                foreach (var x in new[] { 18, 34, 50, r.Width - 60, r.Width - 44, r.Width - 28 })
                {
                    int baseY = r.Bottom - 6;
                    g.FillPolygon(br, new[] { new Point(x, baseY), new Point(x + 8, baseY - 26), new Point(x + 16, baseY) });
                    g.FillPolygon(br, new[] { new Point(x + 2, baseY - 12), new Point(x + 8, baseY - 36), new Point(x + 14, baseY - 12) });
                }
            }
            g.Restore(state);
        }
    }

    /// <summary>Two-line pill button ("Copter Swarm" / "Single link"). Selected = purple gradient.</summary>
    internal class CdaPillButton : Control
    {
        private bool _hover;
        public bool Selected { get; set; }
        public string Line1 { get; set; } = "";
        public string Line2 { get; set; } = "";
        public Image Glyph { get; set; }

        public CdaPillButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Cursor = Cursors.Hand;
            TabStop = false;
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(1, 1, Width - 3, Height - 3);
            using (var path = CdaTheme.Round(rect, Math.Max(6, Height / 3)))
            {
                if (Selected)
                {
                    using (var br = new LinearGradientBrush(rect, CdaTheme.PrimaryLight, CdaTheme.Primary, 0f))
                        g.FillPath(br, path);
                }
                else
                {
                    using (var br = new SolidBrush(_hover ? Color.White : Color.FromArgb(252, 251, 255)))
                        g.FillPath(br, path);
                    using (var pen = new Pen(CdaTheme.CardBorder))
                        g.DrawPath(pen, path);
                }
            }

            int textX = 12;
            if (Glyph != null)
            {
                int s = Math.Min(26, Height - 12);
                g.DrawImage(Glyph, new Rectangle(10, (Height - s) / 2, s, s));
                textX = 10 + s + 8;
            }

            var fg = Selected ? Color.White : CdaTheme.Text;
            using (var b1 = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            using (var b2 = new Font("Segoe UI", 7.5f))
            using (var br = new SolidBrush(fg))
            {
                var h1 = g.MeasureString(Line1, b1).Height;
                var h2 = g.MeasureString(Line2, b2).Height;
                float y = (Height - (h1 + h2 - 2)) / 2f;
                g.DrawString(Line1, b1, br, textX, y);
                g.DrawString(Line2, b2, br, textX, y + h1 - 2);
            }
        }
    }

    /// <summary>Firmware tile (Plane / Rover / Multirotor / Helicopter) with hover + selected state.</summary>
    internal class CdaTile : Control
    {
        private bool _hover;
        public bool Selected { get; set; }
        public Image Picture { get; set; }
        public string Caption { get; set; } = "";

        public CdaTile()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            Cursor = Cursors.Hand;
            TabStop = false;
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(1, 1, Width - 3, Height - 3);
            using (var path = CdaTheme.Round(rect, 10))
            {
                var hot = Selected || _hover;
                var top = hot ? Color.FromArgb(246, 243, 255) : Color.White;
                var bot = hot ? Color.FromArgb(226, 220, 250) : Color.FromArgb(246, 244, 253);
                using (var br = new LinearGradientBrush(rect, top, bot, 90f))
                    g.FillPath(br, path);
                using (var pen = new Pen(Selected ? CdaTheme.PrimaryLight : CdaTheme.CardBorder, Selected ? 1.6f : 1f))
                    g.DrawPath(pen, path);
            }

            int captionH = 22;
            if (Picture != null)
            {
                int area = Height - captionH - 14;
                float scale = Math.Min((float)(Width - 24) / Picture.Width, (float)area / Picture.Height);
                int w = (int)(Picture.Width * scale), h = (int)(Picture.Height * scale);
                g.DrawImage(Picture, (Width - w) / 2, 8 + (area - h) / 2, w, h);
            }
            using (var f = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (var br = new SolidBrush(CdaTheme.Text))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(Caption, f, br, new RectangleF(0, Height - captionH - 4, Width, captionH), sf);
            }
        }
    }

    /// <summary>Small round icon button (zoom +/-, recenter) that floats over the map.</summary>
    internal class CdaRoundButton : Control
    {
        private bool _hover;
        public string Glyph { get; set; } = "+";

        public CdaRoundButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            Cursor = Cursors.Hand;
            Size = new Size(32, 32);
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var br = new SolidBrush(_hover ? Color.FromArgb(240, 236, 255) : Color.White))
                g.FillEllipse(br, rect);
            using (var pen = new Pen(CdaTheme.CardBorder))
                g.DrawEllipse(pen, rect);
            using (var f = new Font("Segoe UI", 12f, FontStyle.Bold))
            using (var br = new SolidBrush(CdaTheme.Primary))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(Glyph, f, br, new RectangleF(0, -1, Width, Height), sf);
            }
        }
    }

    /// <summary>Map / Satellite segmented toggle shown on the top-right of the map.</summary>
    internal class CdaSegmented : Control
    {
        public string Left { get; set; } = "Map";
        public string Right { get; set; } = "Satellite";
        public bool RightSelected { get; set; } = true;
        public event EventHandler SelectionChanged;

        public CdaSegmented()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            Cursor = Cursors.Hand;
            Size = new Size(120, 28);
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            bool right = e.X > Width / 2;
            if (right != RightSelected)
            {
                RightSelected = right;
                Invalidate();
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
            base.OnMouseClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var full = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = CdaTheme.Round(full, Height / 2))
            {
                using (var br = new SolidBrush(Color.White)) g.FillPath(br, path);
                using (var pen = new Pen(CdaTheme.CardBorder)) g.DrawPath(pen, path);
            }
            var half = new Rectangle(RightSelected ? Width / 2 : 2, 2, Width / 2 - 3, Height - 5);
            using (var path = CdaTheme.Round(half, half.Height / 2))
            using (var br = new LinearGradientBrush(half, CdaTheme.PrimaryLight, CdaTheme.Primary, 0f))
                g.FillPath(br, path);

            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            using (var f = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            {
                using (var br = new SolidBrush(RightSelected ? CdaTheme.Text : Color.White))
                    g.DrawString(Left, f, br, new RectangleF(0, 0, Width / 2, Height), sf);
                using (var br = new SolidBrush(RightSelected ? Color.White : CdaTheme.Text))
                    g.DrawString(Right, f, br, new RectangleF(Width / 2, 0, Width / 2, Height), sf);
            }
        }
    }

    /// <summary>White "Location 12.9344° N, 80.2432° E" chip shown on the top-left of the map.</summary>
    internal class CdaLocationChip : Control
    {
        private string _lat = "--", _lng = "--";

        public CdaLocationChip()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            Size = new Size(160, 42);
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }

        public void SetLocation(double lat, double lng)
        {
            _lat = Math.Abs(lat).ToString("0.0000") + "\u00B0 " + (lat >= 0 ? "N" : "S");
            _lng = Math.Abs(lng).ToString("0.0000") + "\u00B0 " + (lng >= 0 ? "E" : "W");
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = CdaTheme.Round(rect, 10))
            {
                using (var br = new SolidBrush(Color.FromArgb(245, 255, 255, 255))) g.FillPath(br, path);
                using (var pen = new Pen(CdaTheme.CardBorder)) g.DrawPath(pen, path);
            }
            using (var br = new SolidBrush(CdaTheme.Primary))
            {
                g.FillEllipse(br, 12, 9, 12, 12);
                g.FillPolygon(br, new[] { new Point(13, 17), new Point(23, 17), new Point(18, 27) });
            }
            using (var br = new SolidBrush(Color.White)) g.FillEllipse(br, 16, 13, 4, 4);

            using (var f1 = new Font("Segoe UI", 7f, FontStyle.Bold))
            using (var f2 = new Font("Segoe UI", 7f))
            using (var br = new SolidBrush(CdaTheme.Text))
            {
                g.DrawString("Location", f1, br, 34, 6);
                g.DrawString(_lat + ", " + _lng, f2, br, 34, 21);
            }
        }
    }

    /// <summary>Tab-like "Home Location - Drag Me" breadcrumb at the top of the page.</summary>
    internal class CdaBreadcrumb : Control
    {
        public CdaBreadcrumb()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        }
        public override Color BackColor { get { return Color.Transparent; } set { } }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using (var pen = new Pen(CdaTheme.CardBorder))
                g.DrawLine(pen, 0, Height - 1, Width, Height - 1);

            var tab = new Rectangle(0, 2, 215, Height - 2);
            using (var path = new GraphicsPath())
            {
                path.AddArc(tab.X, tab.Y, 14, 14, 180, 90);
                path.AddArc(tab.Right - 14, tab.Y, 14, 14, 270, 90);
                path.AddLine(tab.Right, tab.Bottom, tab.X, tab.Bottom);
                path.CloseFigure();
                using (var br = new LinearGradientBrush(tab, CdaTheme.PrimaryLight, CdaTheme.PrimaryDark, 0f))
                    g.FillPath(br, path);
            }
            using (var br = new SolidBrush(Color.White))
                g.FillPolygon(br, new[] { new Point(18, 13), new Point(26, 6), new Point(34, 13), new Point(32, 13), new Point(32, 20), new Point(20, 20), new Point(20, 13) });
            using (var f = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (var br = new SolidBrush(Color.White))
                g.DrawString("Home Location - Drag Me", f, br, 42, 7);
        }
    }
}