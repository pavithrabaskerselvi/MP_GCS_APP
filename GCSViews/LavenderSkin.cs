using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using MissionPlanner.Controls;

namespace MissionPlanner.GCSViews
{
    /// <summary>
    /// Dark-lavender (Skylynk) skin with an animated drone backdrop.
    /// Works on ANY page / UserControl:   LavenderSkin.Attach(this);
    /// Themes every control inside (including module pages the Setup
    /// BackstageView loads later) and keeps re-applying itself so
    /// ThemeManager can never turn the page back.
    /// </summary>
    public static class LavenderSkin
    {
        // ---------- Options ----------
        public static bool ChangeFonts = true;          // false if a label gets clipped
        public static bool EnableBackdrop = true;       // false = static background, no animation
        public static int AnimationInterval = 40;       // ms per frame (40 = 25 fps)
        private const string FontName = "Segoe UI";
        private const int ButtonRadius = 8;

        // ---------- Palette (medium-dark lavender) ----------
        public static readonly Color PageBg      = Color.FromArgb(0x2B, 0x23, 0x58);
        public static readonly Color PanelBg     = Color.FromArgb(0x38, 0x2D, 0x6B);
        public static readonly Color PanelBg2    = Color.FromArgb(0x21, 0x1A, 0x47); // left menu
        public static readonly Color PanelBorder = Color.FromArgb(0x5A, 0x4A, 0x9E);
        public static readonly Color Accent      = Color.FromArgb(0x8B, 0x6C, 0xF0);
        public static readonly Color AccentDark  = Color.FromArgb(0x6A, 0x4B, 0xD8);
        public static readonly Color AccentLight = Color.FromArgb(0xA6, 0x8F, 0xFA);
        public static readonly Color InputBg     = Color.FromArgb(0xF3, 0xEF, 0xFF);
        public static readonly Color InputText   = Color.FromArgb(0x2F, 0x25, 0x60);
        public static readonly Color Text        = Color.FromArgb(0xF0, 0xEC, 0xFF);
        public static readonly Color TextSoft    = Color.FromArgb(0xC9, 0xB8, 0xFF);
        public static readonly Color GridBg      = Color.FromArgb(0x2B, 0x23, 0x58);
        public static readonly Color RowAlt      = Color.FromArgb(0x41, 0x35, 0x7A);
        public static readonly Color Selection   = Color.FromArgb(0x6A, 0x4B, 0xD8);
        public static readonly Color GridLine    = Color.FromArgb(0x4B, 0x3E, 0x8A);

        private static readonly HashSet<Control> attached = new HashSet<Control>();
        private static readonly HashSet<Control> rounded = new HashSet<Control>();
        private static readonly HashSet<Control> buffered = new HashSet<Control>();
        private static readonly Dictionary<Control, List<LavenderBackdrop>> backdrops = new Dictionary<Control, List<LavenderBackdrop>>();
        private static readonly Dictionary<Control, Control[]> hostOverrides = new Dictionary<Control, Control[]>();
        private static readonly HashSet<Control> hostSet = new HashSet<Control>();
        private static readonly HashSet<Control> chips = new HashSet<Control>();
        private static bool bigGridSeen;

        // =====================================================================
        //  Attach: apply now, again after ThemeManager, and keep guarding
        // =====================================================================
        public static void Attach(Control root)
        {
            Attach(root, null, null);
        }

        /// <param name="backdropHosts">panels that should show the animated background (page without BackstageView, e.g. Plan)</param>
        /// <param name="chipLabels">labels that sit on top of a map: get a solid dark "chip" background</param>
        public static void Attach(Control root, Control[] backdropHosts, Control[] chipLabels)
        {
            if (root == null || root.IsDisposed) return;

            if (backdropHosts != null)
            {
                hostOverrides[root] = backdropHosts;
                foreach (var h in backdropHosts) if (h != null) hostSet.Add(h);
            }
            if (chipLabels != null)
                foreach (var l in chipLabels) if (l != null) chips.Add(l);

            ApplyTree(root);

            if (root.IsHandleCreated)
                root.BeginInvoke((Action)(() => ApplyTree(root)));

            if (!attached.Add(root)) return;

            root.VisibleChanged += (s, e) => { if (root.Visible) ApplyTree(root); };

            var guard = new System.Windows.Forms.Timer { Interval = 400 };
            guard.Tick += (s, e) =>
            {
                if (root.IsDisposed) { guard.Stop(); guard.Dispose(); return; }
                if (!root.Visible) return;
                ApplyTree(root);
            };
            guard.Start();

            root.Disposed += (s, e) =>
            {
                guard.Stop();
                guard.Dispose();
                attached.Remove(root);
                List<LavenderBackdrop> list;
                if (backdrops.TryGetValue(root, out list))
                {
                    foreach (var b in list) b.Dispose();
                    backdrops.Remove(root);
                }
                hostOverrides.Remove(root);
            };
        }

        public static void ApplyTree(Control root)
        {
            if (root == null || root.IsDisposed) return;
            try
            {
                bigGridSeen = false;
                Walk(root, root);
                EnsureBackdrop(root);

                foreach (var list in backdrops.Values)
                    foreach (var bd in list)
                        bd.Paused = bigGridSeen;   // huge parameter tables: keep them smooth, pause animation
            }
            catch (Exception)
            {
                // theming must never crash the app
            }
        }

        private static void Walk(Control root, Control c)
        {
            Style(root, c);

            if (c is DataGridView) return;

            foreach (Control child in c.Controls)
                Walk(root, child);
        }

        // =====================================================================
        //  Animated backdrop host
        // =====================================================================
        private static void EnsureBackdrop(Control root)
        {
            if (!EnableBackdrop || backdrops.ContainsKey(root)) return;

            // pages that name their own hosts (Plan page)
            Control[] hosts;
            if (hostOverrides.TryGetValue(root, out hosts))
            {
                var made = new List<LavenderBackdrop>();
                foreach (var h in hosts)
                    if (h != null) made.Add(new LavenderBackdrop(h));
                backdrops[root] = made;
                return;
            }

            var bsv = FindByTypeName(root, "BackstageView");
            if (bsv == null) return;

            Control best = null;
            int area = 0;
            foreach (Control ch in bsv.Controls)
            {
                if (ch.GetType().Name.IndexOf("Button", StringComparison.OrdinalIgnoreCase) >= 0) continue;
                if (ch.Dock == DockStyle.Left) continue;
                int a = ch.Width * ch.Height;
                if (ch.Dock == DockStyle.Fill) a += 100000000;
                if (a > area) { area = a; best = ch; }
            }

            if (best != null)
                backdrops[root] = new List<LavenderBackdrop> { new LavenderBackdrop(best) };
        }

        private static Control FindByTypeName(Control root, string typeName)
        {
            foreach (Control c in root.Controls)
            {
                if (c.GetType().Name == typeName) return c;
                var r = FindByTypeName(c, typeName);
                if (r != null) return r;
            }
            return null;
        }

        internal static void EnableDoubleBuffer(Control c)
        {
            if (!buffered.Add(c)) return;
            try
            {
                typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(c, true, null);
            }
            catch { }
            c.Disposed += (s, e) => buffered.Remove(c);
        }

        // =====================================================================
        //  Per-control styling
        // =====================================================================
        private static void Style(Control root, Control c)
        {
            string typeName = c.GetType().Name;

            // ---- Setup left menu (BackstageView) ----
            if (typeName.IndexOf("Backstage", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                StyleBackstage(c, typeName);
                return;
            }

            // ---- Map control: never recolour / make transparent ----
            if (typeName.IndexOf("GMap", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                SetProp(c, "EmptyTileColor", PanelBg2);
                return;
            }

            SetFont(c);

            // ---- Buttons ----
            if (c is MyButton mb) { StyleMyButton(mb); return; }
            if (c is Button b) { StyleButton(b); return; }

            // ---- Inputs (light fields, dark text: always readable) ----
            if (c is TextBox tb)
            {
                Back(tb, InputBg); Fore(tb, InputText);
                if (tb.BorderStyle != BorderStyle.FixedSingle) tb.BorderStyle = BorderStyle.FixedSingle;
                return;
            }
            if (c is RichTextBox rtb) { Back(rtb, Color.FromArgb(0x1A, 0x14, 0x36)); Fore(rtb, Text); return; }
            if (c is ComboBox cb)
            {
                Back(cb, InputBg); Fore(cb, InputText);
                if (cb.FlatStyle != FlatStyle.Flat) cb.FlatStyle = FlatStyle.Flat;
                return;
            }
            if (c is NumericUpDown || c is ListBox || c is ListView || c is TreeView || c is DateTimePicker)
            {
                Back(c, InputBg); Fore(c, InputText);
                return;
            }

            // ---- Toggles ----
            if (c is CheckBox || c is RadioButton)
            {
                Back(c, Color.Transparent);
                if (IsNeutral(c.ForeColor)) Fore(c, Text);
                return;
            }

            // ---- Labels ----
            if (c is LinkLabel ll)
            {
                if (ll.LinkColor != AccentLight) ll.LinkColor = AccentLight;
                if (ll.ActiveLinkColor != Color.White) ll.ActiveLinkColor = Color.White;
                if (ll.VisitedLinkColor != TextSoft) ll.VisitedLinkColor = TextSoft;
                Back(ll, Color.Transparent);
                return;
            }
            if (c is Label && chips.Contains(c))
            {
                Back(c, PanelBg2);            // solid chip so text stays readable over the map
                Fore(c, Text);
                return;
            }
            if (c is Label)
            {
                Back(c, Color.Transparent);
                if (IsNeutral(c.ForeColor)) Fore(c, Text);   // keep red / green status colours
                return;
            }

            // ---- Containers ----
            if (c is GroupBox gb)
            {
                EnableDoubleBuffer(gb);
                Back(gb, Color.Transparent);   // lets the drone show through
                Fore(gb, TextSoft);
                return;
            }
            if (c is TabPage tp) { Back(tp, PanelBg); Fore(tp, Text); return; }
            if (c is TabControl) return;
            if (c is SplitContainer sc) { Back(sc, PanelBorder); return; }
            if (c is Splitter sp) { Back(sp, PanelBorder); return; }

            if (c is DataGridView g)
            {
                if (g.Visible && g.RowCount > 300) bigGridSeen = true;
                StyleGrid(g);
                return;
            }

            // ---- Custom progress / gauge controls (via reflection) ----
            if (typeName.IndexOf("ProgressBar", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (c is ProgressBar) { Fore(c, AccentLight); Back(c, PanelBg2); }
                SetProp(c, "BackgroundColor", PanelBg2);
                SetProp(c, "ValueColor", AccentLight);
                SetProp(c, "Outline", Accent);
                SetProp(c, "TextColor", Text);
                return;
            }

            if (c is Panel pnl)
            {
                EnableDoubleBuffer(pnl);
                if (hostSet.Contains(pnl))
                    Back(pnl, PageBg);
                else if (ParentIsBackstage(pnl))
                    Back(pnl, pnl.Dock == DockStyle.Left ? PanelBg2 : PageBg);
                else if (IsNeutral(pnl.BackColor))
                    Back(pnl, Color.Transparent);
                return;
            }

            if (c is UserControl uc)
            {
                EnableDoubleBuffer(uc);
                if (c == root) Back(uc, PageBg);
                else Back(uc, Color.Transparent);   // module pages show the animated backdrop
                Fore(uc, Text);
                return;
            }
        }

        // ---------- helpers ----------
        private static bool ParentIsBackstage(Control c)
        {
            return c.Parent != null &&
                   c.Parent.GetType().Name.IndexOf("Backstage", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsNeutral(Color col)
        {
            int max = Math.Max(col.R, Math.Max(col.G, col.B));
            int min = Math.Min(col.R, Math.Min(col.G, col.B));
            return max - min < 40;
        }

        private static void Back(Control c, Color col)
        {
            try { if (c.BackColor != col) c.BackColor = col; } catch { }
        }

        private static void Fore(Control c, Color col)
        {
            try { if (c.ForeColor != col) c.ForeColor = col; } catch { }
        }

        private static void SetFont(Control c)
        {
            if (!ChangeFonts) return;
            try
            {
                var f = c.Font;
                if (f != null && f.Name != FontName)
                    c.Font = new Font(FontName, f.Size, f.Style);
            }
            catch { }
        }

        private static bool SetProp(object o, string name, object value)
        {
            try
            {
                var p = o.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                if (p == null || !p.CanWrite || !p.CanRead) return false;
                if (!p.PropertyType.IsAssignableFrom(value.GetType())) return false;
                if (Equals(p.GetValue(o, null), value)) return false;
                p.SetValue(o, value, null);
                return true;
            }
            catch { return false; }
        }

        // =====================================================================
        //  Right-click menus (call from the page: LavenderSkin.StyleMenu(myMenu))
        // =====================================================================
        public static void StyleMenu(ContextMenuStrip m)
        {
            if (m == null) return;
            try
            {
                if (!(m.Renderer is DarkMenuRenderer)) m.Renderer = new DarkMenuRenderer();
                m.BackColor = PanelBg2;
                m.ForeColor = Text;
                foreach (ToolStripItem i in m.Items) StyleMenuItem(i);
            }
            catch { }
        }

        private static void StyleMenuItem(ToolStripItem i)
        {
            i.BackColor = PanelBg2;
            i.ForeColor = Text;
            var mi = i as ToolStripMenuItem;
            if (mi != null)
            {
                if (!(mi.DropDown.Renderer is DarkMenuRenderer)) mi.DropDown.Renderer = new DarkMenuRenderer();
                mi.DropDown.BackColor = PanelBg2;
                foreach (ToolStripItem sub in mi.DropDownItems) StyleMenuItem(sub);
            }
        }

        private class DarkMenuRenderer : ToolStripProfessionalRenderer
        {
            public DarkMenuRenderer() : base(new DarkMenuColors()) { }
        }

        private class DarkMenuColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected { get { return AccentDark; } }
            public override Color MenuItemSelectedGradientBegin { get { return AccentDark; } }
            public override Color MenuItemSelectedGradientEnd { get { return AccentDark; } }
            public override Color MenuItemBorder { get { return AccentLight; } }
            public override Color MenuBorder { get { return PanelBorder; } }
            public override Color ToolStripDropDownBackground { get { return PanelBg2; } }
            public override Color ImageMarginGradientBegin { get { return PanelBg; } }
            public override Color ImageMarginGradientMiddle { get { return PanelBg; } }
            public override Color ImageMarginGradientEnd { get { return PanelBg; } }
            public override Color SeparatorDark { get { return PanelBorder; } }
            public override Color SeparatorLight { get { return PanelBorder; } }
            public override Color CheckBackground { get { return Accent; } }
            public override Color CheckSelectedBackground { get { return AccentDark; } }
            public override Color CheckPressedBackground { get { return AccentDark; } }
        }

        // =====================================================================
        //  Setup left menu (BackstageView + its buttons)
        // =====================================================================
        private static void StyleBackstage(Control c, string typeName)
        {
            bool changed = false;

            if (typeName == "BackstageView")
                changed |= SetProp(c, "BackColor", PageBg);

            changed |= SetProp(c, "ForeColor", Color.White);              // menu text: white on dark = visible
            changed |= SetProp(c, "HighlightColor1", Accent);             // selected item gradient
            changed |= SetProp(c, "HighlightColor2", AccentDark);
            changed |= SetProp(c, "ButtonsAreaBgColor", PanelBg2);        // dark menu column
            changed |= SetProp(c, "ButtonsAreaPencilColor", AccentLight);
            changed |= SetProp(c, "MenuBackColor", PanelBg2);
            changed |= SetProp(c, "SelectedForeColor", Color.White);

            if (changed) c.Invalidate(true);
        }

        // =====================================================================
        //  Buttons
        // =====================================================================
        private static void StyleMyButton(MyButton b)
        {
            if (b.BGGradTop != AccentLight) b.BGGradTop = AccentLight;
            if (b.BGGradBot != Accent) b.BGGradBot = Accent;
            if (b.Outline != AccentDark) b.Outline = AccentDark;
            if (b.TextColor != Color.White) b.TextColor = Color.White;
            if (b.ColorMouseOver != AccentDark) b.ColorMouseOver = AccentDark;
            var down = Color.FromArgb(0x48, 0x32, 0xA8);
            if (b.ColorMouseDown != down) b.ColorMouseDown = down;
            var dis = Color.FromArgb(0x5A, 0x4E, 0x8F);
            if (b.ColorNotEnabled != dis) b.ColorNotEnabled = dis;
            Back(b, Color.Transparent);
            RoundCorners(b, ButtonRadius);
        }

        private static void StyleButton(Button b)
        {
            if (b.FlatStyle != FlatStyle.Flat) b.FlatStyle = FlatStyle.Flat;
            Back(b, Accent);
            Fore(b, Color.White);
            b.FlatAppearance.BorderColor = AccentLight;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = AccentDark;
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(0x48, 0x32, 0xA8);
            RoundCorners(b, ButtonRadius);
        }

        private static void RoundCorners(Control c, int radius)
        {
            if (rounded.Add(c))
            {
                ApplyRegion(c, radius);
                c.SizeChanged += (s, e) => ApplyRegion(c, radius);
                c.Disposed += (s, e) => rounded.Remove(c);
            }
        }

        private static void ApplyRegion(Control c, int r)
        {
            if (c.Width < 4 * r || c.Height < 2 * r) return;
            using (var p = new GraphicsPath())
            {
                int d = r * 2;
                p.AddArc(0, 0, d, d, 180, 90);
                p.AddArc(c.Width - d, 0, d, d, 270, 90);
                p.AddArc(c.Width - d, c.Height - d, d, d, 0, 90);
                p.AddArc(0, c.Height - d, d, d, 90, 90);
                p.CloseFigure();
                c.Region = new Region(p);
            }
        }

        // =====================================================================
        //  Grids
        // =====================================================================
        private static void StyleGrid(DataGridView g)
        {
            if (g.ColumnHeadersDefaultCellStyle.BackColor == Accent && g.BackgroundColor == GridBg)
                return;

            g.EnableHeadersVisualStyles = false;
            g.BackgroundColor = GridBg;
            g.GridColor = GridLine;
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            var hdr = g.ColumnHeadersDefaultCellStyle;
            hdr.BackColor = Accent;
            hdr.ForeColor = Color.White;
            hdr.SelectionBackColor = Accent;
            hdr.SelectionForeColor = Color.White;
            hdr.Font = new Font(FontName, 9f, FontStyle.Bold);

            var rh = g.RowHeadersDefaultCellStyle;
            rh.BackColor = AccentDark;
            rh.ForeColor = Color.White;
            rh.SelectionBackColor = Accent;
            rh.SelectionForeColor = Color.White;

            foreach (var st in new[] { g.DefaultCellStyle, g.RowsDefaultCellStyle })
            {
                st.BackColor = PanelBg;
                st.ForeColor = Text;
                st.SelectionBackColor = Selection;
                st.SelectionForeColor = Color.White;
            }

            var alt = g.AlternatingRowsDefaultCellStyle;
            alt.BackColor = RowAlt;
            alt.ForeColor = Text;
            alt.SelectionBackColor = Selection;
            alt.SelectionForeColor = Color.White;

            foreach (DataGridViewColumn col in g.Columns)
                if (col is DataGridViewComboBoxColumn cc)
                    cc.FlatStyle = FlatStyle.Flat;

            g.Invalidate();
        }
    }

    // =========================================================================
    //  Animated backdrop: gradient + glow + drifting particles + flight path +
    //  three hovering quadcopters with spinning rotors and blinking LEDs.
    //  Painted from the host panel's Paint event; transparent module pages
    //  inside the host show it automatically.
    // =========================================================================
    internal sealed class LavenderBackdrop : IDisposable
    {
        private struct Particle { public float X, Y, Speed, Size, Phase; }
        private struct DroneSpec
        {
            public float Cx, Cy, Ax, Ay, W1, W2, P1, P2, Scale, Spin;
            public int Alpha;
        }

        private readonly Control host;
        private readonly System.Windows.Forms.Timer timer;
        private readonly Stopwatch clock = Stopwatch.StartNew();
        private readonly Particle[] particles = new Particle[46];
        private readonly DroneSpec[] drones;
        private Bitmap cache;
        private Size cacheSize = Size.Empty;
        public bool Paused;

        public LavenderBackdrop(Control host)
        {
            this.host = host;

            var rnd = new Random(11);
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i] = new Particle
                {
                    X = (float)rnd.NextDouble(),
                    Y = (float)rnd.NextDouble(),
                    Speed = 0.012f + (float)rnd.NextDouble() * 0.03f,
                    Size = 2f + (float)rnd.NextDouble() * 4f,
                    Phase = (float)rnd.NextDouble() * 6.28f
                };
            }

            drones = new[]
            {
                // big, slow, faint (lower right)
                new DroneSpec { Cx = 0.78f, Cy = 0.68f, Ax = 0.08f, Ay = 0.05f, W1 = 0.35f, W2 = 0.55f, P1 = 0f,   P2 = 1f,   Scale = 1.9f, Alpha = 70,  Spin = 38f },
                // medium (upper left)
                new DroneSpec { Cx = 0.30f, Cy = 0.30f, Ax = 0.20f, Ay = 0.08f, W1 = 0.22f, W2 = 0.40f, P1 = 2f,   P2 = 0.5f, Scale = 1.0f, Alpha = 105, Spin = 44f },
                // small, quick, sweeps across the top
                new DroneSpec { Cx = 0.55f, Cy = 0.14f, Ax = 0.34f, Ay = 0.05f, W1 = 0.17f, W2 = 0.60f, P1 = 4f,   P2 = 2f,   Scale = 0.6f, Alpha = 130, Spin = 52f }
            };

            LavenderSkin.EnableDoubleBuffer(host);
            host.Paint += OnHostPaint;
            host.Resize += (s, e) => host.Invalidate(true);

            timer = new System.Windows.Forms.Timer { Interval = Math.Max(16, LavenderSkin.AnimationInterval) };
            timer.Tick += (s, e) =>
            {
                if (Paused || host.IsDisposed || !host.IsHandleCreated || !host.Visible) return;
                host.Invalidate(true);
            };
            timer.Start();

            host.Disposed += (s, e) => Dispose();
        }

        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
            if (cache != null) { cache.Dispose(); cache = null; }
        }

        private void OnHostPaint(object sender, PaintEventArgs e)
        {
            try { Draw(e.Graphics); } catch { }
        }

        // ---------------------------------------------------------------------
        private void Draw(Graphics g)
        {
            int w = host.ClientSize.Width, h = host.ClientSize.Height;
            if (w < 8 || h < 8) return;

            EnsureCache(w, h);
            if (cache != null) g.DrawImageUnscaled(cache, 0, 0);

            var oldSm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float t = (float)clock.Elapsed.TotalSeconds;

            // shrink the drones on small panels (Plan page bottom bar / sidebar)
            float u = Math.Max(0.28f, Math.Min(1f, Math.Min(w, h) / 560f));

            DrawParticles(g, w, h, t);
            DrawFlightPath(g, w, h, t);

            for (int i = 0; i < drones.Length; i++)
            {
                var d = drones[i];
                float x = (d.Cx + d.Ax * (float)Math.Sin(d.W1 * t + d.P1)) * w;
                float y = (d.Cy + d.Ay * (float)Math.Sin(d.W2 * t + d.P2)) * h;
                float vx = d.Ax * d.W1 * (float)Math.Cos(d.W1 * t + d.P1);
                float tilt = Math.Max(-14f, Math.Min(14f, vx * 220f));

                if (i == 0) DrawScanRings(g, x, y, d.Scale * u, t);
                DrawDrone(g, x, y, d.Scale * u, d.Alpha, tilt, d.Spin, t + i * 0.7f);
            }

            g.SmoothingMode = oldSm;
        }

        // ---------------------------------------------------------------------
        private void EnsureCache(int w, int h)
        {
            if (cache != null && cacheSize.Width == w && cacheSize.Height == h) return;

            if (cache != null) cache.Dispose();
            cache = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            cacheSize = new Size(w, h);

            using (var gr = Graphics.FromImage(cache))
            {
                gr.SmoothingMode = SmoothingMode.AntiAlias;

                using (var br = new LinearGradientBrush(new Rectangle(0, 0, w, h),
                    Color.FromArgb(0x1F, 0x17, 0x47), Color.FromArgb(0x45, 0x36, 0x8E), 45f))
                    gr.FillRectangle(br, 0, 0, w, h);

                int big = Math.Max(w, h);
                Glow(gr, w * 0.80f, h * 0.75f, big * 0.45f, Color.FromArgb(95, 0x9B, 0x82, 0xF2));
                Glow(gr, w * 0.12f, h * 0.08f, big * 0.35f, Color.FromArgb(70, 0xC0, 0x84, 0xFC));
                Glow(gr, w * 0.50f, h * 0.55f, big * 0.30f, Color.FromArgb(35, 0x7C, 0x5C, 0xE6));

                // faint blueprint grid
                using (var pen = new Pen(Color.FromArgb(14, 255, 255, 255)))
                {
                    for (int x = 0; x < w; x += 48) gr.DrawLine(pen, x, 0, x, h);
                    for (int y = 0; y < h; y += 48) gr.DrawLine(pen, 0, y, w, y);
                }
            }
        }

        private static void Glow(Graphics g, float cx, float cy, float r, Color c)
        {
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(cx - r, cy - r, r * 2, r * 2);
                using (var pgb = new PathGradientBrush(path))
                {
                    pgb.CenterColor = c;
                    pgb.SurroundColors = new[] { Color.FromArgb(0, c) };
                    g.FillPath(pgb, path);
                }
            }
        }

        // ---------------------------------------------------------------------
        private void DrawParticles(Graphics g, int w, int h, float t)
        {
            using (var br = new SolidBrush(Color.White))
            {
                foreach (var p in particles)
                {
                    float y = ((p.Y - t * p.Speed) % 1f + 1f) % 1f;
                    float x = p.X + 0.01f * (float)Math.Sin(t * 0.6f + p.Phase);
                    int a = (int)(35 + 70 * (0.5f + 0.5f * Math.Sin(t * 1.5f + p.Phase)));
                    br.Color = Color.FromArgb(Math.Max(0, Math.Min(255, a)), 0xD8, 0xCC, 0xFF);
                    g.FillEllipse(br, x * w - p.Size / 2, y * h - p.Size / 2, p.Size, p.Size);
                }
            }
        }

        private void DrawFlightPath(Graphics g, int w, int h, float t)
        {
            var pts = new List<PointF>();
            for (int x = 0; x <= w; x += 24)
                pts.Add(new PointF(x, PathY(x, w, h, t)));

            if (pts.Count < 3) return;

            using (var pen = new Pen(Color.FromArgb(55, 0xC9, 0xB8, 0xFF), 2f) { DashStyle = DashStyle.Dash })
                g.DrawCurve(pen, pts.ToArray());

            // waypoints
            float[] fr = { 0.1f, 0.3f, 0.5f, 0.7f, 0.9f };
            using (var ring = new Pen(Color.FromArgb(90, 0xD8, 0xCC, 0xFF), 1.5f))
            using (var fill = new SolidBrush(Color.FromArgb(60, 0x8B, 0x6C, 0xF0)))
            {
                for (int i = 0; i < fr.Length; i++)
                {
                    float x = fr[i] * w;
                    float y = PathY(x, w, h, t);
                    float pulse = 5f + 2f * (float)Math.Sin(t * 2f + i);
                    g.FillEllipse(fill, x - pulse, y - pulse, pulse * 2, pulse * 2);
                    g.DrawEllipse(ring, x - pulse, y - pulse, pulse * 2, pulse * 2);
                }
            }
        }

        private static float PathY(float x, int w, int h, float t)
        {
            return h * 0.90f + (float)Math.Sin(x / w * 6.283f * 1.5f + t * 0.3f) * h * 0.03f;
        }

        private void DrawScanRings(Graphics g, float x, float y, float s, float t)
        {
            for (int k = 0; k < 2; k++)
            {
                float ph = (t * 0.35f + k * 0.5f) % 1f;
                float r = (20f + ph * 130f) * s;
                int a = (int)((1f - ph) * 55);
                using (var pen = new Pen(Color.FromArgb(Math.Max(0, a), 0xC9, 0xB8, 0xFF), 2f))
                    g.DrawEllipse(pen, x - r, y + 75f * s - r * 0.32f, r * 2, r * 0.64f);
            }
        }

        private static int A(int v) { return Math.Max(0, Math.Min(255, v)); }

        private static void DrawDrone(Graphics g, float cx, float cy, float s, int alpha, float tilt, float spin, float t)
        {
            var state = g.Save();
            g.TranslateTransform(cx, cy);
            g.RotateTransform(tilt);
            g.ScaleTransform(s, s);

            // ground shadow
            using (var sh = new SolidBrush(Color.FromArgb(A(alpha / 4), 0, 0, 0)))
                g.FillEllipse(sh, -48, 66, 96, 16);

            float[] mx = { -62, 62, -62, 62 };
            float[] my = { -22, -22, 22, 22 };

            // arms
            using (var armPen = new Pen(Color.FromArgb(A(alpha), 0xB9, 0xA9, 0xFF), 6f)
                   { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                for (int i = 0; i < 4; i++) g.DrawLine(armPen, 0, 0, mx[i], my[i]);
            }

            // rotors
            using (var disc = new SolidBrush(Color.FromArgb(A(alpha / 4), 0xD8, 0xCC, 0xFF)))
            using (var motor = new SolidBrush(Color.FromArgb(A(alpha + 40), 0xE4, 0xDC, 0xFF)))
            using (var blade = new Pen(Color.FromArgb(A(alpha), 255, 255, 255), 2f))
            {
                for (int i = 0; i < 4; i++)
                {
                    g.FillEllipse(disc, mx[i] - 34, my[i] - 12, 68, 24);
                    g.FillEllipse(motor, mx[i] - 6, my[i] - 4, 12, 8);
                    for (int b = 0; b < 2; b++)
                    {
                        float a = spin * t + b * 1.5708f + i * 0.7f;
                        float dx = (float)Math.Cos(a) * 34f;
                        float dy = (float)Math.Sin(a) * 12f;
                        g.DrawLine(blade, mx[i] - dx, my[i] - dy, mx[i] + dx, my[i] + dy);
                    }
                }
            }

            // body
            using (var bb = new LinearGradientBrush(new RectangleF(-30, -16, 60, 32),
                Color.FromArgb(A(alpha + 40), 0xF1, 0xEC, 0xFF), Color.FromArgb(A(alpha), 0x9B, 0x82, 0xF2), 90f))
                g.FillEllipse(bb, -30, -16, 60, 32);

            // camera gimbal
            using (var lens = new SolidBrush(Color.FromArgb(A(alpha + 60), 0x2B, 0x23, 0x58)))
                g.FillEllipse(lens, -7, 8, 14, 14);
            using (var glint = new SolidBrush(Color.FromArgb(A(alpha + 80), 255, 255, 255)))
                g.FillEllipse(glint, -3, 11, 4, 4);

            // blinking nav LEDs
            bool on = ((int)(t * 2f)) % 2 == 0;
            int la = on ? 255 : 80;
            using (var red = new SolidBrush(Color.FromArgb(la, 255, 80, 90)))
            using (var green = new SolidBrush(Color.FromArgb(la, 80, 255, 160)))
            {
                g.FillEllipse(red, -25, -4, 6, 6);
                g.FillEllipse(green, 19, -4, 6, 6);
            }

            g.Restore(state);
        }
    }
}