using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Reflection;
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

        static Bitmap MakeIcon()
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
            object help = GetMember(mp, "MenuHelp");
            var icon = MakeIcon();
            EventHandler click = (o, ev) => GoHome();

            var ctl = help as Control;
            var item = help as ToolStripItem;

            if (ctl != null && ctl.Parent != null)
            {
                var bb = ctl as ButtonBase;
                var b = new Button
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

            // ---- exit X (top right, inside the header bar) ----
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

                    // light purple tint: darker on top (title area), lighter at the bottom (photo shows)
                    using (var ov = new LinearGradientBrush(rect, Color.FromArgb(120, 40, 16, 100), Color.FromArgb(35, 70, 30, 130), 90f))
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

            // ---- header bar ----
            var hr = new Rectangle(20, 16, Width - 40, 68);
            using (var p = CdaTheme.Round(hr, 20))
            {
                using (var br = new LinearGradientBrush(hr, Color.FromArgb(235, 62, 30, 140), Color.FromArgb(235, 108, 70, 208), 0f))
                    g.FillPath(br, p);
                using (var pen = new Pen(Color.FromArgb(60, 255, 255, 255), 1.2f))
                    g.DrawPath(pen, p);
            }

            if (logo != null)
                g.DrawImage(logo, new Rectangle(38, 24, 52, 52));

            using (var f1 = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (var f2 = new Font("Segoe UI", 15f, FontStyle.Bold | FontStyle.Italic))
            using (var f3 = new Font("Segoe UI", 7.5f, FontStyle.Bold))
            using (var white = new SolidBrush(Color.White))
            using (var lav = new SolidBrush(Lavender))
            {
                DrawTwoPart(g, "CHENNAI DRONE", "ACADEMY", f1, f2, Color.White, Lavender, 102, 23, 8);
                g.DrawString("EASY TO LEARN AND EASY TO FLY", f3, lav, 102, 52, tf);
            }

            // user block (right side of header)
            using (var fe = new Font("Segoe UI", 9f, FontStyle.Bold))
            using (var fs = new Font("Segoe UI", 7.5f, FontStyle.Regular))
            using (var lav = new SolidBrush(Lavender))
            using (var white = new SolidBrush(Color.White))
            using (var far = new StringFormat { Alignment = StringAlignment.Far })
            {
                string sub = "Ground Control Station";
                float blockW = Math.Max(g.MeasureString(email, fe, 4000, tf).Width, g.MeasureString(sub, fs, 4000, tf).Width);
                float textRight = Width - 84;
                g.DrawString(email, fe, white, new RectangleF(textRight - 400, 29, 400, 20), far);
                g.DrawString(sub, fs, lav, new RectangleF(textRight - 400, 48, 400, 18), far);

                float ax = textRight - blockW - 46;
                var av = new RectangleF(ax, 33, 34, 34);
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