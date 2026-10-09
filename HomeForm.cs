using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MissionPlanner
{
    // ---------------------------------------------------------------
    //  Icons drawn in code (no image files needed). Origin = center.
    // ---------------------------------------------------------------
    public static class CdaIcons
    {
        public static void Draw(Graphics g, string kind)
        {
            using (var pen = new Pen(Color.White, 2.6f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;

                switch (kind)
                {
                    case "data": // document
                        using (var p = CdaTheme.Round(new Rectangle(-14, -18, 28, 36), 4))
                            g.DrawPath(pen, p);
                        g.DrawLine(pen, -7, -8, 7, -8);
                        g.DrawLine(pen, -7, 0, 7, 0);
                        g.DrawLine(pen, -7, 8, 2, 8);
                        break;

                    case "plan": // map pin
                        using (var p = new GraphicsPath())
                        {
                            p.AddArc(-12f, -19f, 24f, 24f, 150f, 240f);
                            p.AddLine(p.GetLastPoint(), new PointF(0f, 19f));
                            p.CloseFigure();
                            g.DrawPath(pen, p);
                        }
                        g.DrawEllipse(pen, -4.5f, -11.5f, 9f, 9f);
                        break;

                    case "setup": // gear
                        using (var tp = new Pen(Color.White, 5f))
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                double a = i * Math.PI / 4.0;
                                g.DrawLine(tp,
                                    (float)(Math.Cos(a) * 12), (float)(Math.Sin(a) * 12),
                                    (float)(Math.Cos(a) * 18), (float)(Math.Sin(a) * 18));
                            }
                        }
                        g.DrawEllipse(pen, -12f, -12f, 24f, 24f);
                        g.DrawEllipse(pen, -5f, -5f, 10f, 10f);
                        break;

                    case "config": // sliders
                        g.DrawLine(pen, -17, -12, 17, -12);
                        g.DrawLine(pen, -17, 0, 17, 0);
                        g.DrawLine(pen, -17, 12, 17, 12);
                        g.FillEllipse(Brushes.White, -10.5f, -16.5f, 9f, 9f);
                        g.FillEllipse(Brushes.White, 2.5f, -4.5f, 9f, 9f);
                        g.FillEllipse(Brushes.White, -6.5f, 7.5f, 9f, 9f);
                        break;

                    case "simulation": // monitor with play button
                        using (var p = CdaTheme.Round(new Rectangle(-18, -16, 36, 26), 4))
                            g.DrawPath(pen, p);
                        g.DrawLine(pen, 0, 10, 0, 16);
                        g.DrawLine(pen, -9, 16, 9, 16);
                        g.FillPolygon(Brushes.White, new[]
                        {
                            new PointF(-4, -9), new PointF(-4, 3), new PointF(6, -3)
                        });
                        break;

                    case "help": // question mark in circle
                        g.DrawEllipse(pen, -17f, -17f, 34f, 34f);
                        using (var f = new Font("Segoe UI", 17f, FontStyle.Bold))
                        using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                            g.DrawString("?", f, Brushes.White, new RectangleF(-17, -17, 34, 36), sf);
                        break;
                }
            }
        }
    }

    // ---------------------------------------------------------------
    //  Purple square tile (icon + label)
    // ---------------------------------------------------------------
    public class CdaTile : Control
    {
        public string Kind = "data";
        bool hover;

        public CdaTile()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            float k = Width / 112f;
            var r = new Rectangle(2, 2, Width - 5, Height - 5);
            using (var path = CdaTheme.Round(r, (int)(18 * k)))
            {
                Color c1 = hover ? Color.FromArgb(176, 136, 255) : Color.FromArgb(150, 108, 245);
                Color c2 = hover ? Color.FromArgb(120, 78, 228) : Color.FromArgb(96, 58, 206);
                using (var br = new LinearGradientBrush(r, c1, c2, 90f))
                    g.FillPath(br, path);
                using (var pen = new Pen(Color.FromArgb(hover ? 210 : 90, 255, 255, 255), 1.5f))
                    g.DrawPath(pen, path);
            }

            g.TranslateTransform(Width / 2f, Height * 0.38f);
            g.ScaleTransform(k * 0.95f, k * 0.95f);
            CdaIcons.Draw(g, Kind);
            g.ResetTransform();

            using (var f = new Font("Segoe UI", 8.5f * k, FontStyle.Bold))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                g.DrawString(Text, f, Brushes.White, new RectangleF(0, Height * 0.68f, Width, Height * 0.26f), sf);
        }
    }

    // ---------------------------------------------------------------
    //  Pill button (filled gradient, or outline)
    // ---------------------------------------------------------------
    public class CdaPillButton : Control
    {
        public bool Outline;
        bool hover;

        public CdaPillButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
        }

        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var r = new Rectangle(1, 1, Width - 3, Height - 3);
            using (var path = CdaTheme.Round(r, r.Height / 2))
            {
                if (!Outline)
                {
                    Color c1 = hover ? Color.FromArgb(210, 130, 255) : Color.FromArgb(190, 110, 245);
                    Color c2 = hover ? Color.FromArgb(140, 90, 240) : Color.FromArgb(120, 72, 226);
                    using (var br = new LinearGradientBrush(r, c1, c2, 0f))
                        g.FillPath(br, path);
                }
                else
                {
                    using (var br = new SolidBrush(Color.FromArgb(hover ? 60 : 25, 255, 255, 255)))
                        g.FillPath(br, path);
                    using (var pen = new Pen(Color.FromArgb(170, 255, 255, 255), 1.4f))
                        g.DrawPath(pen, path);
                }
            }
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }

    // ---------------------------------------------------------------
    //  Dark purple / neon theme for the Flight Data left panel
    //  (HUD + Quick tab tiles + tabs), like the CDA mock-up.
    // ---------------------------------------------------------------
    public static class CdaFlightTheme
    {
        static readonly Color Bg = Color.FromArgb(238, 232, 252);
        static readonly Color TileBg = Color.FromArgb(22, 8, 54);
        static readonly Color[] Neon =
        {
            Color.FromArgb(170, 80, 235),  // Altitude      - purple
            Color.FromArgb(40, 200, 240),  // GroundSpeed   - cyan
            Color.FromArgb(100, 120, 245), // Dist to WP    - blue
            Color.FromArgb(60, 220, 130),  // Yaw           - green
            Color.FromArgb(230, 60, 220),  // Vertical sp.  - magenta
            Color.FromArgb(40, 190, 255)   // DistToMAV     - cyan
        };

        static readonly HashSet<Control> tabsDone = new HashSet<Control>();
        static System.Windows.Forms.Timer timer;

        // Flight Data screen loads late, so try for 90 seconds until it exists.
        public static void EnsureApplied(MainV2 mp)
        {
            if (timer != null) return;
            DateTime started = DateTime.Now;
            timer = new System.Windows.Forms.Timer { Interval = 1500 };
            timer.Tick += (o, ev) =>
            {
                Apply(mp);
                if ((DateTime.Now - started).TotalSeconds > 90)
                {
                    timer.Stop();
                    timer.Dispose();
                    timer = null;
                }
            };
            timer.Start();
        }

        static void Collect(Control root, string typeName, List<Control> list)
        {
            foreach (Control c in root.Controls)
            {
                if (c.GetType().Name == typeName) list.Add(c);
                Collect(c, typeName, list);
            }
        }

        static void SetColorProp(object o, string name, Color col)
        {
            try
            {
                var p = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
                if (p != null && p.CanWrite && p.PropertyType == typeof(Color))
                    p.SetValue(o, col, null);
            }
            catch { }
        }

        public static void Apply(MainV2 mp)
        {
            try
            {
                var found = new List<Control>();
                Collect(mp, "QuickView", found);
                if (found.Count == 0) return;

                // order tiles top-left to bottom-right
                found.Sort((a, b) =>
                {
                    Point pa = a.PointToScreen(Point.Empty), pb = b.PointToScreen(Point.Empty);
                    int d = pa.Y.CompareTo(pb.Y);
                    return d != 0 ? d : pa.X.CompareTo(pb.X);
                });

                // find the left column that holds the HUD + tabs
                TabControl tc = null;
                for (Control p = found[0].Parent; p != null; p = p.Parent)
                    if (p is TabControl) { tc = (TabControl)p; break; }

                Control top = tc != null ? (Control)tc : found[0].Parent;
                while (top.Parent != null && top.Parent != mp && top.Parent.Width <= mp.Width * 0.45)
                    top = top.Parent;

                PurpleAll(top);

                for (int i = 0; i < found.Count; i++)
                    StyleTile(found[i], Neon[i % Neon.Length]);

                // HUD colours (dark purple sky, purple ground)
                var huds = new List<Control>();
                Collect(mp, "HUD", huds);
                foreach (var h in huds)
                {
                    SetColorProp(h, "skyColor1", Color.FromArgb(120, 95, 225));
                    SetColorProp(h, "skyColor2", Color.FromArgb(225, 170, 225));
                    SetColorProp(h, "groundColor1", Color.FromArgb(165, 150, 200));
                    SetColorProp(h, "groundColor2", Color.FromArgb(120, 105, 165));
                    SetColorProp(h, "hudcolor", Color.White);
                    h.Invalidate();
                }
            }
            catch { }
        }

        static void PurpleAll(Control c)
        {
            string tn = c.GetType().Name;
            if (c is ComboBox || c is TextBoxBase || c is NumericUpDown || c is PictureBox) return;
            if (tn.Contains("HUD") || tn.Contains("GMap") || tn.Contains("Zed") || tn.Contains("OpenGL")) return;
            if (tn == "QuickView") return; // styled separately

            try
            {
                var tp = c as TabPage;
                if (tp != null) tp.UseVisualStyleBackColor = false;

                if (c is CheckBox || c is RadioButton || c is Label || c is GroupBox)
                {
                    c.BackColor = Bg;
                    c.ForeColor = Color.FromArgb(70, 55, 110);
                }
                else if (c is Button)
                {
                    c.BackColor = Color.FromArgb(110, 60, 210);
                    c.ForeColor = Color.White;
                    SetColorProp(c, "BGGradTop", Color.FromArgb(140, 90, 240));
                    SetColorProp(c, "BGGradBot", Color.FromArgb(90, 50, 200));
                }
                else
                {
                    c.BackColor = Bg;
                }
                c.BackgroundImage = null;
            }
            catch { }

            var tabs = c as TabControl;
            if (tabs != null) StyleTabs(tabs);

            foreach (Control ch in c.Controls)
                PurpleAll(ch);
        }

        static void StyleTabs(TabControl tc)
        {
            try
            {
                tc.BackColor = Bg;
                if (tabsDone.Contains(tc)) return;
                tabsDone.Add(tc);

                tc.DrawMode = TabDrawMode.OwnerDrawFixed;
                tc.DrawItem += (s, e) =>
                {
                    var t = (TabControl)s;
                    bool sel = e.Index == t.SelectedIndex;
                    Rectangle full = t.GetTabRect(e.Index);
                    Rectangle rect = full;
                    rect.Inflate(-2, -2);
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var back = new SolidBrush(Bg))
                        e.Graphics.FillRectangle(back, full);
                    using (var path = CdaTheme.Round(rect, 10))
                    using (var br = new SolidBrush(sel ? Color.FromArgb(140, 100, 240) : Color.FromArgb(250, 247, 255)))
                        e.Graphics.FillPath(br, path);
                    TextRenderer.DrawText(e.Graphics, t.TabPages[e.Index].Text, t.Font, rect,
                        sel ? Color.White : Color.FromArgb(110, 90, 170),
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                };
                tc.Invalidate();
            }
            catch { }
        }

        // QuickView now draws itself (neon border, icon, glow) - we only give it colours
        static void StyleTile(Control t, Color col)
        {
            try
            {
                t.BackColor = Bg;
                t.ForeColor = Color.FromArgb(70, 55, 110);
                SetColorProp(t, "numberColor", col);
                SetColorProp(t, "numberColorBackup", col);
                t.Invalidate();
            }
            catch { }
        }
    }

    // ---------------------------------------------------------------
    //  Plain black renderer for a ToolStrip based menu bar
    // ---------------------------------------------------------------
    public class CdaBlackRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            e.Graphics.Clear(Color.Black);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e) { }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var r = new Rectangle(Point.Empty, e.Item.Size);
            using (var br = new SolidBrush((e.Item.Selected || e.Item.Pressed) ? Color.FromArgb(60, 60, 60) : Color.Black))
                e.Graphics.FillRectangle(br, r);
        }
    }

    // ---------------------------------------------------------------
    //  Black window title bar (the white line on top of Mission Planner)
    // ---------------------------------------------------------------
    public static class CdaTitleBar
    {
        [DllImport("dwmapi.dll")]
        static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int val, int size);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr after, int x, int y, int cx, int cy, uint flags);

        static bool refreshed;

        public static void Black(Form f)
        {
            try
            {
                IntPtr h = f.Handle;
                int on = 1;
                DwmSetWindowAttribute(h, 20, ref on, 4); // dark title bar (Win10 20H1+ / Win11)
                DwmSetWindowAttribute(h, 19, ref on, 4); // older Win10 builds

                int black = 0x000000;                    // COLORREF 0x00BBGGRR
                int white = 0xFFFFFF;
                DwmSetWindowAttribute(h, 35, ref black, 4); // caption colour (Win11)
                DwmSetWindowAttribute(h, 36, ref white, 4); // caption text colour (Win11)
                DwmSetWindowAttribute(h, 34, ref black, 4); // window border (Win11)

                if (!refreshed)
                {
                    refreshed = true;
                    // 0x37 = NOMOVE | NOSIZE | NOZORDER | NOACTIVATE | FRAMECHANGED -> repaint title bar
                    SetWindowPos(h, IntPtr.Zero, 0, 0, 0, 0, 0x37);
                }
            }
            catch { }
        }
    }

    // ---------------------------------------------------------------
    //  Purple rounded menu button (same look as the selected menu tile)
    // ---------------------------------------------------------------
    public class CdaMenuButton : Button
    {
        bool hover;

        public CdaMenuButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            try { ButtonRenderer.DrawParentBackground(g, ClientRectangle, this); } catch { }

            // flat like the other menu buttons: bar colour shows through,
            // only a faint highlight on hover
            if (hover)
            {
                var r = new Rectangle(2, 2, Width - 5, Height - 5);
                using (var path = CdaTheme.Round(r, 8))
                using (var br = new SolidBrush(Color.FromArgb(55, 255, 255, 255)))
                    g.FillPath(br, path);
            }

            if (Image != null)
            {
                // keep the icon's aspect ratio (about 40 px wide)
                int iw = 40, ih = (int)(40f * Image.Height / Image.Width);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(Image, (Width - iw) / 2, 5, iw, ih);
            }

            TextRenderer.DrawText(g, Text, Font,
                new Rectangle(0, Height - 24, Width, 20), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }

    // ---------------------------------------------------------------
    //  HOME button inside the Mission Planner top menu bar.
    //  Click -> main window hides, Home screen shows again.
    // ---------------------------------------------------------------
    public static class CdaHomeNav
    {
        public static bool Attached;
        static System.Windows.Forms.Timer attachTimer;
        static bool going;

        // waits for the main window, then adds the HOME button once
        public static void EnsureAttached()
        {
            if (Attached || attachTimer != null) return;

            DateTime started = DateTime.Now;
            attachTimer = new System.Windows.Forms.Timer { Interval = 700 };
            attachTimer.Tick += (o, ev) =>
            {
                bool stop = false;
                try
                {
                    var mp = MainV2.instance;
                    if (mp != null && mp.IsHandleCreated && mp.Visible)
                    {
                        stop = TryAttach(mp);
                        Attached = stop;
                    }
                }
                catch { }

                if (stop || (DateTime.Now - started).TotalSeconds > 120)
                {
                    attachTimer.Stop();
                    attachTimer.Dispose();
                    attachTimer = null;
                }
            };
            attachTimer.Start();
        }

        static object GetMember(object o, string name)
        {
            var t = o.GetType();
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var f = t.GetField(name, flags);
            if (f != null) return f.GetValue(o);
            var p = t.GetProperty(name, flags);
            if (p != null) return p.GetValue(o, null);
            return null;
        }

        // Turns the WHOLE purple top menu bar of Mission Planner black
        // (the bar itself + every button / panel / strip inside it).
        public static void BlackBar(MainV2 mp)
        {
            try
            {
                object help = GetMember(mp, "MenuHelp");
                Control bar = null;
                var c = help as Control;
                var it = help as ToolStripItem;
                if (c != null) bar = c.Parent;
                else if (it != null) bar = it.Owner;
                if (bar == null) return;

                // climb to the outermost container that is still bar-sized (<= 100 px high)
                Control top = bar;
                while (top.Parent != null && top.Parent != mp && top.Parent.Height <= 100)
                    top = top.Parent;

                BlackAll(top);
                top.Invalidate(true);
            }
            catch { }
        }

        static void SetColorProp(object o, string name, Color col)
        {
            try
            {
                var p = o.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
                if (p != null && p.CanWrite && p.PropertyType == typeof(Color))
                    p.SetValue(o, col, null);
            }
            catch { }
        }

        static void BlackAll(Control c)
        {
            if (c is CdaMenuButton) return;
            // keep dropdowns, text boxes and the logo picture as they are
            if (c is ComboBox || c is TextBoxBase || c is NumericUpDown || c is PictureBox) return;

            try { c.BackColor = Color.Black; } catch { }
            try { c.BackgroundImage = null; } catch { }

            // Mission Planner's own gradient buttons (MyButton etc.) paint with these colours
            SetColorProp(c, "BGGradTop", Color.Black);
            SetColorProp(c, "BGGradBot", Color.Black);
            SetColorProp(c, "ColorMouseOver", Color.FromArgb(60, 60, 60));
            SetColorProp(c, "ColorMouseDown", Color.FromArgb(90, 90, 90));

            var bt = c as Button;
            if (bt != null)
            {
                try { bt.FlatAppearance.MouseOverBackColor = Color.FromArgb(60, 60, 60); } catch { }
            }

            var ts = c as ToolStrip;
            if (ts != null)
            {
                try
                {
                    ts.Renderer = new CdaBlackRenderer();
                    foreach (ToolStripItem i in ts.Items)
                    {
                        i.BackColor = Color.Black;
                        i.BackgroundImage = null;
                    }
                }
                catch { }
            }

            foreach (Control ch in c.Controls)
                BlackAll(ch);
        }

        // HOME icon (light purple house with ring) embedded in the code - no file needed
        const string HomeIconB64 = "iVBORw0KGgoAAAANSUhEUgAAADEAAAAtCAYAAAAHiIP8AAAWfmNhQlgAABZ+anVtYgAAAB5qdW1kYzJwYQARABCAAACqADibcQNjMnBhAAAAFlhqdW1iAAAAR2p1bWRjMm1hABEAEIAAAKoAOJtxA3VybjpjMnBhOmQxOTQ0NjY0LWE0MDgtNDMyNi1hYjAxLWMwMGUzNzJlOTc2YQAAAAOTanVtYgAAAClqdW1kYzJhcwARABCAAACqADibcQNjMnBhLmFzc2VydGlvbnMAAAAAuGp1bWIAAABEanVtZGNib3IAEQAQgAAAqgA4m3ETYzJwYS5pbmdyZWRpZW50LnYzAAAAABhjMnNo5OZ4zpPGXnqH2rYKLEi0VwAAAGxjYm9yo2lkYzpmb3JtYXRpaW1hZ2UvcG5namluc3RhbmNlSUR4LHhtcDppaWQ6MjI0ZmRjZmItNWRmMi00NTExLThhY2EtY2E4ODk4ZDdkNzBkbHJlbGF0aW9uc2hpcGhwYXJlbnRPZgAAAeJqdW1iAAAAQWp1bWRjYm9yABEAEIAAAKoAOJtxE2MycGEuYWN0aW9ucy52MgAAAAAYYzJzaLejYFPnvreAXolMv0uyba0AAAGZY2JvcqJnYWN0aW9uc4KiZmFjdGlvbmtjMnBhLm9wZW5lZGpwYXJhbWV0ZXJzoWtpbmdyZWRpZW50c4GiY3VybHgtc2VsZiNqdW1iZj1jMnBhLmFzc2VydGlvbnMvYzJwYS5pbmdyZWRpZW50LnYzZGhhc2hYIL4FcwiyYelxCPdAk3yC4OebGHmMqBl5wtCzgg4OuyfXpGZhY3Rpb254HWNvbS5hbnRocm9waWMuY2xhdWRlLnByb3ZpZGVkanBhcmFtZXRlcnOheB9jb20uYW50aHJvcGljLm9yaWdpbi1jb25maWRlbmNlZ3Vua25vd25rZGVzY3JpcHRpb254ZkNsYXVkZSBwcm92aWRlZCB0aGlzIGZpbGUgYXQgdGhlIHJlcXVlc3Qgb2YgYSB1c2VyIGFuZCBtYXkgaGF2ZSBjcmVhdGVkIG9yIG1vZGlmaWVkIHRoZSBmaWxlIGNvbnRlbnRzLm1zb2Z0d2FyZUFnZW50oWRuYW1lZkNsYXVkZXJhbGxBY3Rpb25zSW5jbHVkZWT1AAAAyGp1bWIAAABAanVtZGNib3IAEQAQgAAAqgA4m3ETYzJwYS5oYXNoLmRhdGEAAAAAGGMyc2hDtAMAwkdgU2+fnJIZnS6TAAAAgGNib3KlY2FsZ2ZzaGEyNTZjcGFkTQAAAAAAAAAAAAAAAABkaGFzaFggztfV5PaYHIofzr2a3KTnw+3AMWoWzdFsGSEYk7xwA4NkbmFtZW5qdW1iZiBtYW5pZmVzdGpleGNsdXNpb25zgaJlc3RhcnQYIWZsZW5ndGgZFooAAAI+anVtYgAAACdqdW1kYzJjbAARABCAAACqADibcQNjMnBhLmNsYWltLnYyAAAAAg9jYm9ypWNhbGdmc2hhMjU2aXNpZ25hdHVyZXhNc2VsZiNqdW1iZj0vYzJwYS91cm46YzJwYTpkMTk0NDY2NC1hNDA4LTQzMjYtYWIwMS1jMDBlMzcyZTk3NmEvYzJwYS5zaWduYXR1cmVqaW5zdGFuY2VJRHgseG1wOmlpZDo5NjI5ZGNiZi1kYmM4LTRiMWMtODVlYS05MmRmMmY3MzNkNjlyY3JlYXRlZF9hc3NlcnRpb25zg6JjdXJseC1zZWxmI2p1bWJmPWMycGEuYXNzZXJ0aW9ucy9jMnBhLmluZ3JlZGllbnQudjNkaGFzaFggvgVzCLJh6XEI90CTfILg55sYeYyoGXnC0LOCDg67J9eiY3VybHgqc2VsZiNqdW1iZj1jMnBhLmFzc2VydGlvbnMvYzJwYS5hY3Rpb25zLnYyZGhhc2hYIPqvthD++mmlpZ7pf00U0JXWtAUGJGn0cuRnrJ9tS6JvomN1cmx4KXNlbGYjanVtYmY9YzJwYS5hc3NlcnRpb25zL2MycGEuaGFzaC5kYXRhZGhhc2hYIE20ahVvo6BO7M2otXdrF0oCYOy5LmFwRGoLOEiCzwHadGNsYWltX2dlbmVyYXRvcl9pbmZvo2RuYW1lb0FudGhyb3BpYyBGaWxlc2d2ZXJzaW9uZTEuMC4wa3NwZWNWZXJzaW9uZTIuNC4wAAAQOGp1bWIAAAAoanVtZGMyY3MAEQAQgAAAqgA4m3EDYzJwYS5zaWduYXR1cmUAAAAQCGNib3LShFkCEqIBJhghWQIKMIICBjCCAY2gAwIBAgIUQOWgCu7COdC+uIP6BkIFPWdVEwAwCgYIKoZIzj0EAwMwSTEXMBUGA1UEChMOQW50aHJvcGljLCBQQkMxLjAsBgNVBAMTJUFudGhyb3BpYyBDb250ZW50IENyZWRlbnRpYWxzIFJvb3QgQ0EwHhcNMjYwODA3MTg0MzU2WhcNMjgwODA2MTk0MzU2WjBEMRcwFQYDVQQKEw5BbnRocm9waWMsIFBCQzEpMCcGA1UEAxMgQW50aHJvcGljIENsYXVkZSBDb250ZW50IFNpZ25pbmcwWTATBgcqhkjOPQIBBggqhkjOPQMBBwNCAASYegpry1AYBRTVNL1CpTlbROnY3dey+UrsF9C3phYrATN3ZHf93Mo8RQN0KOUuOn19P4oWNFWe5n2/She9N7eTo1gwVjAOBgNVHQ8BAf8EBAMCB4AwFQYDVR0lBA4wDAYKKwYBBAGD6F4CATAMBgNVHRMBAf8EAjAAMB8GA1UdIwQYMBaAFM5R4gSBTmRbI/jjxM+aPpzB11zCMAoGCCqGSM49BAMDA2cAMGQCMDFzHRSeAXrSy1WOzkbhPZ6Km2wGTmZ/2gK18k8BQGXyqz88Rdrz6CTX9flAnYNVxgIwcF9c3fVhqmJKpi+UhasNUMko69cyX6STPfta3Q8EjyzDjzoyrol46FP6VFHhvUcJoWNwYWRZDZ4AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA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";

        static Bitmap MakeIcon()
        {
            try
            {
                using (var ms = new MemoryStream(Convert.FromBase64String(HomeIconB64)))
                using (var img = Image.FromStream(ms))
                    return new Bitmap(img);
            }
            catch { }
            return MakeDrawnIcon(); // fallback
        }

        static Bitmap MakeDrawnIcon()
        {
            var bmp = new Bitmap(32, 32);
            using (var g = Graphics.FromImage(bmp))
            using (var pen = new Pen(Color.White, 2.4f))
            {
                pen.LineJoin = LineJoin.Round;
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLines(pen, new[] { new PointF(3, 16), new PointF(16, 4), new PointF(29, 16) });
                g.DrawLines(pen, new[] { new PointF(7, 14), new PointF(7, 28), new PointF(25, 28), new PointF(25, 14) });
                g.DrawRectangle(pen, 13f, 19f, 6f, 9f);
            }
            return bmp;
        }

        // copies the look of the HELP button and puts HOME right after it
        static bool TryAttach(MainV2 mp)
        {
            CdaTitleBar.Black(mp); // black title bar
            BlackBar(mp); // purple menu bar -> black
            CdaFlightTheme.EnsureApplied(mp); // dark purple neon Flight Data panel
            mp.Activated += (s, a) => { CdaTitleBar.Black(mp); BlackBar(mp); CdaFlightTheme.Apply(mp); }; // re-apply after Hide/Show
            object help = GetMember(mp, "MenuHelp");
            var icon = MakeIcon();
            EventHandler click = (o, ev) => GoHome();

            var ctl = help as Control;
            var item = help as ToolStripItem;

            if (ctl != null && ctl.Parent != null)
            {
                var bb = ctl as ButtonBase;
                var b = new CdaMenuButton
                {
                    Name = "cdaHomeButton",
                    Text = "HOME",
                    Size = ctl.Size,
                    Font = ctl.Font,
                    ForeColor = ctl.ForeColor,
                    BackColor = ctl.BackColor,
                    FlatStyle = FlatStyle.Flat,
                    Image = icon,
                    ImageAlign = bb != null ? bb.ImageAlign : ContentAlignment.TopCenter,
                    TextAlign = bb != null ? bb.TextAlign : ContentAlignment.BottomCenter,
                    TextImageRelation = bb != null ? bb.TextImageRelation : TextImageRelation.ImageAboveText,
                    Cursor = Cursors.Hand,
                    Margin = ctl.Margin,
                    UseVisualStyleBackColor = false
                };
                b.FlatAppearance.BorderSize = 0;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
                b.Click += click;

                var parent = ctl.Parent;
                int idx = parent.Controls.GetChildIndex(ctl);
                b.Anchor = ctl.Anchor;
                b.Dock = ctl.Dock;
                parent.Controls.Add(b);
                if (ctl.Dock == DockStyle.Left)
                {
                    parent.Controls.SetChildIndex(b, idx);
                }
                else if (parent is FlowLayoutPanel)
                {
                    parent.Controls.SetChildIndex(b, idx + 1);
                }
                else
                {
                    b.Location = new Point(ctl.Right + 4, ctl.Top);
                    b.BringToFront();
                }
                return true;
            }

            if (item != null && item.Owner != null)
            {
                var tb = new ToolStripButton("HOME", icon)
                {
                    Name = "cdaHomeButton",
                    DisplayStyle = item.DisplayStyle,
                    TextImageRelation = item.TextImageRelation,
                    ImageScaling = item.ImageScaling,
                    ImageAlign = item.ImageAlign,
                    TextAlign = item.TextAlign,
                    Font = item.Font,
                    ForeColor = item.ForeColor,
                    BackColor = item.BackColor,
                    AutoSize = item.AutoSize,
                    Margin = item.Margin,
                    Padding = item.Padding
                };
                if (!item.AutoSize) tb.Size = item.Size;
                tb.Click += click;
                var owner = item.Owner;
                owner.Items.Insert(owner.Items.IndexOf(item) + 1, tb);
                return true;
            }

            // fallback: floating button in the empty part of the top bar
            var fb = new Button
            {
                Name = "cdaHomeButton",
                Text = "HOME",
                Size = new Size(70, 60),
                Location = new Point(340, 2),
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(38, 39, 40),
                FlatStyle = FlatStyle.Flat,
                Image = icon,
                ImageAlign = ContentAlignment.TopCenter,
                TextAlign = ContentAlignment.BottomCenter,
                TextImageRelation = TextImageRelation.ImageAboveText,
                Cursor = Cursors.Hand
            };
            fb.FlatAppearance.BorderSize = 0;
            fb.Click += click;
            mp.Controls.Add(fb);
            fb.BringToFront();
            return true;
        }

        // Hide Mission Planner, show the Home screen.
        // OPEN / tile -> back to Mission Planner, LOGOUT -> login, X -> close the app.
        public static void GoHome()
        {
            if (going) return;
            var mp = MainV2.instance;
            if (mp == null) return;

            going = true;
            try
            {
                mp.Hide();
                while (true)
                {
                    DialogResult res;
                    using (var home = new HomeForm()) res = home.ShowDialog();

                    if (res == DialogResult.OK)
                    {
                        mp.Show();
                        mp.Activate();
                        return;
                    }

                    if (res == DialogResult.Retry)
                    {
                        AuthService.UserEmail = null;
                        AuthService.IdToken = null;
                        DialogResult lr;
                        using (var login = new LoginForm()) lr = login.ShowDialog();
                        if (lr == DialogResult.OK) continue;
                    }

                    // X on the home/login screen = exit the application
                    mp.Show();
                    mp.Close();
                    return;
                }
            }
            finally
            {
                going = false;
            }
        }
    }

    // ---------------------------------------------------------------
    //  Home screen shown after login.
    //  DialogResult: OK = open Ground Control Station, Retry = logout, Cancel = exit.
    //  If a tile is clicked, StartScreen holds the Mission Planner screen to jump to.
    //
    //  BACKGROUND: put  home_bg.jpg  in the same folder as MissionPlanner.exe
    //  (also searched: Resources\ and images\ sub-folders).
    // ---------------------------------------------------------------
    public class HomeForm : Form
    {
        public static string StartScreen;

        static System.Windows.Forms.Timer applyTimer;

        Bitmap bg;
        Image logo;
        string email;
        Rectangle panelRect;
        float titleSz;
        int yWelcome, yTitle, yTag;

        static readonly Color Lavender = Color.FromArgb(214, 198, 255);

        public HomeForm()
        {
            StartScreen = null;
            CdaHomeNav.EnsureAttached(); // adds a HOME button to the Mission Planner top menu

            Text = "CHENNAIDRONEACADEMY";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Bounds = Screen.PrimaryScreen.Bounds;
            TopMost = true;
            DoubleBuffered = true;
            KeyPreview = true;
            BackColor = Color.FromArgb(58, 28, 128);
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }

            email = AuthService.UserEmail ?? "";
            logo = Program.IconFile;
            BuildBackground();

            // ---- layout numbers ----
            float sc = Math.Min(1.4f, Math.Max(1f, Height / 820f));
            titleSz = Math.Max(28f, Height / 20f);
            int cx = Width / 2;
            yWelcome = (int)(Height * 0.17);
            yTitle = yWelcome + 34;
            yTag = yTitle + (int)(titleSz * 1.9f);

            int tile = (int)(112 * sc), gap = (int)(14 * sc), pad = (int)(22 * sc);
            int tilesW = 6 * tile + 5 * gap;
            panelRect = new Rectangle((Width - tilesW - 2 * pad) / 2, yTag + 46, tilesW + 2 * pad, tile + 2 * pad);

            // ---- exit X (top right) ----
            var close = new Label
            {
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter,
                Bounds = new Rectangle(Width - 64, 32, 40, 36)
            };
            close.Click += (o, ev) => { DialogResult = DialogResult.Cancel; Close(); };
            Controls.Add(close);

            // ---- the 6 option tiles ----
            string[] names = { "DATA", "PLAN", "SETUP", "CONFIG", "SIMULATION", "HELP" };
            string[] kinds = { "data", "plan", "setup", "config", "simulation", "help" };
            string[] screens = { "FlightData", "FlightPlanner", "HWConfig", "SWConfig", "Simulation", "Help" };

            for (int i = 0; i < 6; i++)
            {
                string scr = screens[i];
                var t = new CdaTile
                {
                    Text = names[i],
                    Kind = kinds[i],
                    Bounds = new Rectangle(panelRect.X + pad + i * (tile + gap), panelRect.Y + pad, tile, tile)
                };
                t.Click += (o, ev) =>
                {
                    StartScreen = scr;
                    ScheduleStartScreen(scr);
                    DialogResult = DialogResult.OK;
                    Close();
                };
                Controls.Add(t);
            }

            // ---- open GCS + logout ----
            int btnY = panelRect.Bottom + 34;
            var open = new CdaPillButton
            {
                Text = "OPEN GROUND CONTROL STATION   \u2192",
                Bounds = new Rectangle(cx - 200, btnY, 400, 50)
            };
            open.Click += (o, ev) => { DialogResult = DialogResult.OK; Close(); };
            Controls.Add(open);

            var logout = new CdaPillButton
            {
                Text = "LOGOUT",
                Outline = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Bounds = new Rectangle(cx - 80, btnY + 66, 160, 38)
            };
            logout.Click += (o, ev) => { DialogResult = DialogResult.Retry; Close(); };
            Controls.Add(logout);

            KeyDown += (o, ev) =>
            {
                if (ev.KeyCode == Keys.Enter) { DialogResult = DialogResult.OK; Close(); }
            };

            FormClosed += (o, ev) => { if (bg != null) bg.Dispose(); };
            Shown += (o, ev) => Activate();
        }

        // After a tile is clicked the main window loads (5-10 sec).
        // This timer waits for it and then jumps to the chosen screen.
        static void ScheduleStartScreen(string screen)
        {
            if (applyTimer != null) { applyTimer.Stop(); applyTimer.Dispose(); }

            DateTime started = DateTime.Now;
            DateTime ready = DateTime.MinValue;
            int delay = CdaHomeNav.Attached ? 300 : 2000;
            applyTimer = new System.Windows.Forms.Timer { Interval = 500 };
            applyTimer.Tick += (o, ev) =>
            {
                bool done = false;
                try
                {
                    var mp = MainV2.instance;
                    object view = (mp != null && mp.IsHandleCreated && mp.Visible) ? GetMember(mp, "MyView") : null;
                    if (view != null)
                    {
                        if (ready == DateTime.MinValue) ready = DateTime.Now;
                        if ((DateTime.Now - ready).TotalMilliseconds >= delay)
                        {
                            var m = view.GetType().GetMethod("ShowScreen",
                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                null, new[] { typeof(string) }, null);
                            if (m != null) m.Invoke(view, new object[] { screen });
                            done = true;
                        }
                    }
                }
                catch { }

                if (done || (DateTime.Now - started).TotalSeconds > 90)
                {
                    applyTimer.Stop();
                    applyTimer.Dispose();
                    applyTimer = null;
                }
            };
            applyTimer.Start();
        }

        static object GetMember(object o, string name)
        {
            var t = o.GetType();
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var f = t.GetField(name, flags);
            if (f != null) return f.GetValue(o);
            var p = t.GetProperty(name, flags);
            if (p != null) return p.GetValue(o, null);
            return null;
        }

        // Looks for the image in the exe folder first, then a few common sub-folders.
        // Returns a copy, so the file is never locked.
        static Image LoadImg(params string[] names)
        {
            string[] folders =
            {
                Application.StartupPath,
                AppDomain.CurrentDomain.BaseDirectory,
                Path.GetDirectoryName(Application.ExecutablePath),
                Path.Combine(Application.StartupPath, "Resources"),
                Path.Combine(Application.StartupPath, "images"),
                Path.Combine(Application.StartupPath, "Images")
            };

            foreach (var folder in folders)
            {
                if (string.IsNullOrEmpty(folder)) continue;
                foreach (var n in names)
                {
                    try
                    {
                        var p = Path.Combine(folder, n);
                        if (File.Exists(p))
                        {
                            using (var img = Image.FromFile(p)) return new Bitmap(img);
                        }
                    }
                    catch { }
                }
            }
            return null;
        }

        // Purple sunset gradient first; then home_bg.jpg (the Chennai Drone Academy
        // sunset photo) is drawn on top, scaled to cover the full screen,
        // with a light purple tint so the white text stays readable.
        void BuildBackground()
        {
            bg = new Bitmap(Width, Height);
            using (var g = Graphics.FromImage(bg))
            {
                var rect = new Rectangle(0, 0, Width, Height);

                // fallback gradient (used when the photo is missing)
                using (var br = new LinearGradientBrush(rect, Color.FromArgb(52, 24, 120), Color.FromArgb(236, 168, 204), 90f))
                {
                    var cb = new ColorBlend(3);
                    cb.Colors = new[] { Color.FromArgb(52, 24, 120), Color.FromArgb(124, 82, 214), Color.FromArgb(236, 168, 204) };
                    cb.Positions = new[] { 0f, 0.55f, 1f };
                    br.InterpolationColors = cb;
                    g.FillRectangle(br, rect);
                }

                var img = LoadImg("home_bg.jpg", "home_bg.jpeg", "home_bg.png", "home_bg.jpg.jpg");
                if (img != null)
                {
                    using (img)
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        // "cover" fit: fill the screen, keep the aspect ratio,
                        // anchored to the bottom so the team stays visible
                        float s = Math.Max((float)Width / img.Width, (float)Height / img.Height);
                        int w = (int)(img.Width * s), h = (int)(img.Height * s);
                        g.DrawImage(img, (Width - w) / 2, Height - h, w, h);
                    }

                    // lighter purple tint: drone + photo stay clearly visible
                    using (var ov = new LinearGradientBrush(rect, Color.FromArgb(70, 40, 16, 100), Color.FromArgb(20, 70, 30, 130), 90f))
                        g.FillRectangle(ov, rect);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (bg != null) e.Graphics.DrawImageUnscaled(bg, 0, 0);
            else base.OnPaintBackground(e);
        }

        float TwoPartWidth(Graphics g, string a, string b, Font fa, Font fb, float gap)
        {
            var tf = StringFormat.GenericTypographic;
            return g.MeasureString(a, fa, 4000, tf).Width + gap + g.MeasureString(b, fb, 4000, tf).Width;
        }

        void DrawTwoPart(Graphics g, string a, string b, Font fa, Font fb, Color ca, Color cb, float x, float y, float gap)
        {
            var tf = StringFormat.GenericTypographic;
            float wa = g.MeasureString(a, fa, 4000, tf).Width;
            using (var b1 = new SolidBrush(ca)) g.DrawString(a, fa, b1, x, y, tf);
            using (var b2 = new SolidBrush(cb)) g.DrawString(b, fb, b2, x + wa + gap, y, tf);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            var tf = StringFormat.GenericTypographic;

            // ---- header (no bar, so the drone stays visible) ----
            Color shadow = Color.FromArgb(130, 0, 0, 0);

            if (logo != null)
                g.DrawImage(logo, new Rectangle(38, 24, 52, 52));

            // left corner: CHENNAI DRONE ACADEMY + tagline
            using (var f1 = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (var f2 = new Font("Segoe UI", 15f, FontStyle.Bold | FontStyle.Italic))
            using (var f3 = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            using (var lav = new SolidBrush(Lavender))
            using (var sh = new SolidBrush(shadow))
            {
                // shadow first, then the real text
                DrawTwoPart(g, "CHENNAI DRONE", "ACADEMY", f1, f2, shadow, shadow, 103, 24, 8);
                DrawTwoPart(g, "CHENNAI DRONE", "ACADEMY", f1, f2, Color.White, Lavender, 102, 23, 8);

                g.DrawString("EASY TO LEARN AND EASY TO FLY", f3, sh, 103, 53, tf);
                g.DrawString("EASY TO LEARN AND EASY TO FLY", f3, lav, 102, 52, tf);
            }

            // right corner: small user block (email + avatar)
            using (var fe = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (var fs = new Font("Segoe UI", 7.5f, FontStyle.Regular))
            using (var lav = new SolidBrush(Lavender))
            using (var white = new SolidBrush(Color.White))
            using (var sh = new SolidBrush(shadow))
            using (var far = new StringFormat { Alignment = StringAlignment.Far })
            {
                string sub = "Ground Control Station";
                float blockW = Math.Max(g.MeasureString(email, fe, 4000, tf).Width, g.MeasureString(sub, fs, 4000, tf).Width);
                float textRight = Width - 40;
                float uy = Height - 80; // bottom right corner

                g.DrawString(email, fe, sh, new RectangleF(textRight - 399, uy + 1, 400, 20), far);
                g.DrawString(email, fe, white, new RectangleF(textRight - 400, uy, 400, 20), far);
                g.DrawString(sub, fs, sh, new RectangleF(textRight - 399, uy + 20, 400, 18), far);
                g.DrawString(sub, fs, lav, new RectangleF(textRight - 400, uy + 19, 400, 18), far);

                float ax = textRight - blockW - 46;
                var av = new RectangleF(ax, uy + 4, 34, 34);
                using (var ab = new SolidBrush(Color.FromArgb(150, 108, 245)))
                    g.FillEllipse(ab, av);
                using (var ap = new Pen(Color.FromArgb(200, 255, 255, 255), 1.5f))
                    g.DrawEllipse(ap, av);
                string initial = email.Length > 0 ? email.Substring(0, 1).ToUpper() : "?";
                using (var fi = new Font("Segoe UI", 11f, FontStyle.Bold))
                using (var cs = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString(initial, fi, white, av, cs);
            }

            // ---- welcome block ----
            using (var lav = new SolidBrush(Lavender))
            using (var fw = new Font("Segoe UI", 14f, FontStyle.Bold))
            using (var center = new StringFormat { Alignment = StringAlignment.Center })
            {
                g.DrawString("W E L C O M E   T O", fw, lav, new RectangleF(0, yWelcome, Width, 34), center);
            }

            using (var ft1 = new Font("Segoe UI", titleSz, FontStyle.Bold))
            using (var ft2 = new Font("Segoe UI", titleSz, FontStyle.Bold | FontStyle.Italic))
            {
                float gap = titleSz * 0.3f;
                float w = TwoPartWidth(g, "CHENNAI DRONE", "ACADEMY", ft1, ft2, gap);
                DrawTwoPart(g, "CHENNAI DRONE", "ACADEMY", ft1, ft2, Color.White, Lavender, (Width - w) / 2f, yTitle, gap);
            }

            using (var lav = new SolidBrush(Lavender))
            using (var ftag = new Font("Segoe UI", 11f, FontStyle.Bold))
            using (var line = new Pen(Color.FromArgb(130, 255, 255, 255), 1.2f))
            {
                string tag = "EASY TO LEARN AND EASY TO FLY";
                float tw = g.MeasureString(tag, ftag, 4000, tf).Width;
                float cx = Width / 2f;
                g.DrawString(tag, ftag, lav, cx - tw / 2f, yTag, tf);
                float ly = yTag + 10;
                g.DrawLine(line, cx - tw / 2f - 170, ly, cx - tw / 2f - 18, ly);
                g.DrawLine(line, cx + tw / 2f + 18, ly, cx + tw / 2f + 170, ly);
            }

            // ---- translucent panel behind the tiles ----
            using (var p = CdaTheme.Round(panelRect, 26))
            {
                using (var br = new SolidBrush(Color.FromArgb(48, 255, 255, 255)))
                    g.FillPath(br, p);
                using (var pen = new Pen(Color.FromArgb(80, 255, 255, 255), 1.2f))
                    g.DrawPath(pen, p);
            }
        }
    }
}