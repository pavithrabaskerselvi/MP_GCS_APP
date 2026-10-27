using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MissionPlanner
{
    // ---------------------------------------------------------------
    //  Shared theme helpers (used by LoginForm and RegisterForm)
    // ---------------------------------------------------------------
    public static class CdaTheme
    {
        public static readonly Color Bg1 = Color.FromArgb(240, 236, 253);
        public static readonly Color Bg2 = Color.FromArgb(212, 202, 247);
        public static readonly Color Primary = Color.FromArgb(124, 92, 235);
        public static readonly Color PrimaryDark = Color.FromArgb(98, 66, 218);
        public static readonly Color TextDark = Color.FromArgb(36, 38, 88);
        public static readonly Color TextMuted = Color.FromArgb(110, 114, 140);
        public static readonly Color Border = Color.FromArgb(222, 220, 240);
        public static readonly Color TabBg = Color.FromArgb(238, 234, 252);
        public static readonly Color TabHover = Color.FromArgb(226, 220, 248);
        public static readonly Color Error = Color.FromArgb(220, 60, 80);
        public static readonly Color Ok = Color.FromArgb(30, 150, 90);

        public static GraphicsPath Round(Rectangle r, int radius)
        {
            int d = radius * 2;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        static bool _cueUnsupported; static void SafeCue(IntPtr h, string text) { if (_cueUnsupported) return; try { SendMessage(h, 0x1501, (IntPtr)1, text); } catch (DllNotFoundException) { _cueUnsupported = true; } catch (EntryPointNotFoundException) { _cueUnsupported = true; } catch (Exception) { } }
        // placeholder text inside a TextBox
        public static void Cue(TextBox t, string text)
        {
            t.HandleCreated += (s, e) => SafeCue(t.Handle, text);
            if (t.IsHandleCreated) SafeCue(t.Handle, text);
        }

        // full-page login background (the Chennai Drone Academy hero picture).
        // Optional override: put login_bg.jpg (or .png) next to MissionPlanner.exe.
        // Otherwise the picture built into LoginBackgroundData.cs is used.
        public static Image LoadBackground()
        {
            foreach (var n in new[] { "login_bg.jpg", "login_bg.jpeg", "login_bg.png" })
            {
                try
                {
                    var p = Path.Combine(Application.StartupPath, n);
                    if (File.Exists(p))
                    {
                        using (var img = Image.FromFile(p)) return new Bitmap(img);
                    }
                }
                catch { }
            }

            try
            {
                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                foreach (var res in asm.GetManifestResourceNames())
                {
                    if (res.EndsWith("login_bg.jpg", StringComparison.OrdinalIgnoreCase) ||
                        res.EndsWith("login_bg.png", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var st = asm.GetManifestResourceStream(res))
                        using (var img = Image.FromStream(st)) return new Bitmap(img);
                    }
                }
            }
            catch { }

            // always available: the picture compiled into LoginBackgroundData.cs
            return LoginBackgroundData.Load();
        }

        // logo for the card header: logo.png / cda_logo.png next to the exe, or an embedded resource
        public static Image LoadLogo()
        {
            foreach (var n in new[] { "cda_logo.png", "logo.png", "cda_logo.jpg" })
            {
                try
                {
                    var p = Path.Combine(Application.StartupPath, n);
                    if (File.Exists(p))
                    {
                        using (var img = Image.FromFile(p)) return new Bitmap(img);
                    }
                }
                catch { }
            }
            try
            {
                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                foreach (var res in asm.GetManifestResourceNames())
                {
                    if (res.EndsWith("cda_logo.png", StringComparison.OrdinalIgnoreCase) ||
                        res.EndsWith(".logo.png", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var st = asm.GetManifestResourceStream(res))
                        using (var img = Image.FromStream(st)) return new Bitmap(img);
                    }
                }
            }
            catch { }
            return LoginLogoData.Load();
        }

        // optional hero image: put login_hero.jpg (or .png) next to MissionPlanner.exe
        public static Image LoadHero()
        {
            foreach (var n in new[] { "login_hero.jpg", "login_hero.jpeg", "login_hero.png" })
            {
                try
                {
                    var p = Path.Combine(Application.StartupPath, n);
                    if (File.Exists(p))
                    {
                        using (var img = Image.FromFile(p)) return new Bitmap(img);
                    }
                }
                catch { }
            }
            return null;
        }
    }

    public class CdaRoundPanel : Panel
    {
        public int Radius = 16;
        public Color Fill = Color.White;
        public Color Stroke = Color.Transparent;

        public CdaRoundPanel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);
            BackColor = Color.Transparent;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var p = CdaTheme.Round(r, Radius))
            using (var b = new SolidBrush(Fill))
            {
                e.Graphics.FillPath(b, p);
                if (Stroke != Color.Transparent)
                    using (var pen = new Pen(Stroke, 1.2f)) e.Graphics.DrawPath(pen, p);
            }
            base.OnPaint(e);
        }
    }

    public class CdaRoundButton : Button
    {
        public Color Fill = CdaTheme.Primary;
        public Color HoverFill = CdaTheme.PrimaryDark;
        public Color TextColor = Color.White;
        public int Radius = 12;
        bool hover;

        public CdaRoundButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rp = Parent as CdaRoundPanel;
            e.Graphics.Clear(rp != null ? rp.Fill : (Parent != null ? Parent.BackColor : Color.White));
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Color c = !Enabled ? Color.FromArgb(180, 165, 230) : (hover ? HoverFill : Fill);
            using (var path = CdaTheme.Round(new Rectangle(0, 0, Width - 1, Height - 1), Radius))
            using (var b = new SolidBrush(c))
                e.Graphics.FillPath(b, path);
            // Centre by hand: TextRenderer's alignment flags are ignored on the Android WinForms layer,
            // which drew the text at the top-left corner and clipped the first letter.
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            var sz = e.Graphics.MeasureString(Text, Font);
            float tx = Math.Max(2f, (Width - sz.Width) / 2f);
            float ty = Math.Max(0f, (Height - sz.Height) / 2f);
            using (var tb = new SolidBrush(Enabled ? TextColor : Color.White))
                e.Graphics.DrawString(Text, Font, tb, tx, ty);
        }
    }

    public class CdaInputBox : CdaRoundPanel
    {
        public TextBox Inner;
        Label eye;

        public string Value
        {
            get { return Inner.Text; }
            set { Inner.Text = value; }
        }

        public CdaInputBox(string cue, bool password)
        {
            Radius = 10;
            Fill = Color.White;
            Stroke = CdaTheme.Border;

            Inner = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10.5f),
                BackColor = Color.White,
                ForeColor = CdaTheme.TextDark,
                UseSystemPasswordChar = password
            };
            Controls.Add(Inner);
            CdaTheme.Cue(Inner, cue);

            if (password)
            {
                eye = new Label
                {
                    Text = "Show",
                    ForeColor = CdaTheme.Primary,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.White,
                    Size = new Size(48, 24)
                };
                eye.Click += (s, e) =>
                {
                    Inner.UseSystemPasswordChar = !Inner.UseSystemPasswordChar;
                    eye.Text = Inner.UseSystemPasswordChar ? "Show" : "Hide";
                };
                Controls.Add(eye);
            }

            Inner.Enter += (s, e) => { Stroke = CdaTheme.Primary; Invalidate(); };
            Inner.Leave += (s, e) => { Stroke = CdaTheme.Border; Invalidate(); };
            SizeChanged += (s, e) => DoLayout();
        }

        void DoLayout()
        {
            int right = eye != null ? 62 : 14;
            Inner.Width = Math.Max(20, Width - 14 - right);
            Inner.Location = new Point(14, (Height - Inner.Height) / 2);
            if (eye != null) eye.Location = new Point(Width - 54, (Height - eye.Height) / 2);
        }
    }

    // ---------------------------------------------------------------
    //  Base form: lavender gradient, hero text on the left, white card
    // ---------------------------------------------------------------
    public class CdaThemedForm : Form
    {
        protected CdaRoundPanel Card;
        protected Control Host;         // helpers add their controls here (Card by default)
        protected float VS = 1f;        // vertical scale so the card fits short phone screens
        protected int Y(int v) { return (int)Math.Round(v * VS); }
        Image hero;
        Rectangle heroRect = Rectangle.Empty;
        Image bg;                       // full-page background picture
        Bitmap bgCache;                 // background pre-scaled to the form size
        Size bgCacheSize = Size.Empty;

        public CdaThemedForm(int cardHeight, bool showClose)
        {
            Text = "CHENNAIDRONEACADEMY";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            Bounds = Screen.PrimaryScreen.Bounds;
            TopMost = true;

            // phones are much shorter than a desktop: shrink the card layout to fit the screen height
            int screenH = Screen.PrimaryScreen.Bounds.Height;
            VS = Math.Max(0.6f, Math.Min(1f, (screenH - 24f) / cardHeight));
            cardHeight = (int)Math.Round(cardHeight * VS);
            DoubleBuffered = true;
            BackColor = CdaTheme.Bg1;
#if !NETSTANDARD2_0
            try { Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath); } catch { }
#endif

            int cardW = 400;
            int cx = Math.Min((int)(Width * 0.60), Width - cardW - 60);

            // The background picture already contains the headline, tagline and feature icons,
            // so the coded hero text is only built when the picture is missing.
            bg = CdaTheme.LoadBackground();
            if (bg == null)
            {
                hero = CdaTheme.LoadHero();
                BuildHero(cx);
            }

            Card = new CdaRoundPanel
            {
                Size = new Size(cardW, cardHeight),
                Radius = 22,
                Fill = Color.White,
                Stroke = CdaTheme.Border
            };
            Card.Location = new Point(cx, Math.Max(20, (Height - cardHeight) / 2));
            Controls.Add(Card);
            Host = Card;

            if (showClose)
            {
                var close = new Label
                {
                    Text = "X",
                    ForeColor = CdaTheme.TextDark,
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    BackColor = Color.Transparent,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Bounds = new Rectangle(Width - 60, 15, 40, 36)
                };
                close.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
                Controls.Add(close);
                close.BringToFront();
            }

            FormClosed += (s, e) =>
            {
                if (hero != null) hero.Dispose();
                if (bg != null) bg.Dispose();
                if (bgCache != null) bgCache.Dispose();
            };
        }

        protected Label MakeLabel(Control parent, string text, int x, int y, int w, int h,
            float size, FontStyle style, Color color, ContentAlignment align)
        {
            var l = new Label
            {
                Text = text,
                ForeColor = color,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", size, style),
                TextAlign = align,
                AutoSize = false,
                Bounds = new Rectangle(x, y, w, h)
            };
            parent.Controls.Add(l);
            return l;
        }

        void BuildHero(int cx)
        {
            int leftX = (int)(Width * 0.07);
            int leftW = Math.Max(300, cx - leftX - 60);
            float sz = Math.Max(24f, Height / 28f);
            int h1 = (int)(sz * 2.0f);
            int y = (int)(Height * 0.12);

            MakeLabel(this, "LEARN  \u00B7  FLY  \u00B7  BUILD  \u00B7  TOMORROW", leftX, y, leftW, 22,
                9f, FontStyle.Bold, CdaTheme.Primary, ContentAlignment.MiddleLeft);
            y += 34;
            MakeLabel(this, "Welcome to", leftX, y, leftW, h1,
                sz, FontStyle.Bold, CdaTheme.TextDark, ContentAlignment.MiddleLeft);
            y += h1;
            MakeLabel(this, "Chennai Drone Academy", leftX, y, leftW, h1,
                sz, FontStyle.Bold, CdaTheme.Primary, ContentAlignment.MiddleLeft);
            y += h1 + 4;
            MakeLabel(this, "Ground Control Station", leftX, y, leftW, 32,
                15f, FontStyle.Bold, CdaTheme.TextDark, ContentAlignment.MiddleLeft);
            y += 40;
            MakeLabel(this, "Professional drone training. Real-world operations.\nSafer skies for a smarter tomorrow.",
                leftX, y, leftW, 52, 11f, FontStyle.Regular, CdaTheme.TextMuted, ContentAlignment.TopLeft);
            y += 66;

            string[] feats = { "Training\nExcellence", "Real-World\nOperations", "Safety &\nCompliance", "Expert\nMentors" };
            int fw = Math.Min(150, leftW / 4);
            for (int i = 0; i < feats.Length; i++)
                MakeLabel(this, feats[i], leftX + i * fw, y, fw, 44,
                    10f, FontStyle.Bold, CdaTheme.TextDark, ContentAlignment.TopLeft);
            y += 70;

            int hh = Math.Min(420, Height - y - 60);
            if (hero != null && hh >= 120)
                heroRect = new Rectangle(leftX, y, leftW, hh);
        }

        // logo + title block at the top of the card
        protected void AddHeader()
        {
            Image logo = CdaTheme.LoadLogo();
            if (logo == null && Icon != null)
            {
                try { logo = Icon.ToBitmap(); } catch { }
            }
            if (logo != null)
            {
                int ls = Math.Max(32, Y(48));
                var pb = new PictureBox
                {
                    Image = logo,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    BackColor = Color.Transparent,
                    Bounds = new Rectangle((400 - ls) / 2, Y(20), ls, ls)
                };
                Host.Controls.Add(pb);
            }
            MakeLabel(Host, "CHENNAI DRONE ACADEMY", 0, Y(74), 400, Y(26), 12f, FontStyle.Bold,
                CdaTheme.TextDark, ContentAlignment.MiddleCenter);
            MakeLabel(Host, "Ground Control Station", 0, Y(100), 400, Y(22), 9.5f, FontStyle.Regular,
                CdaTheme.TextMuted, ContentAlignment.MiddleCenter);
        }

        // Login | Register pill tabs
        protected void AddTabs(bool loginActive, Action otherClick)
        {
            var tabs = new CdaRoundPanel
            {
                Radius = 22,
                Fill = CdaTheme.TabBg,
                Bounds = new Rectangle(30, Y(134), 340, Y(44))
            };
            var a = new CdaRoundButton { Text = "Login", Radius = 18, Bounds = new Rectangle(4, 4, 166, Y(36)) };
            var b = new CdaRoundButton { Text = "Register", Radius = 18, Bounds = new Rectangle(170, 4, 166, Y(36)) };
            var other = loginActive ? b : a;
            other.Fill = CdaTheme.TabBg;
            other.HoverFill = CdaTheme.TabHover;
            other.TextColor = CdaTheme.TextMuted;
            other.Click += (s, e) => otherClick();
            tabs.Controls.Add(a);
            tabs.Controls.Add(b);
            Host.Controls.Add(tabs);
        }

        protected CdaInputBox AddField(string label, string cue, bool password, int y)
        {
            MakeLabel(Host, label, 30, Y(y), 340, 20, 9.5f, FontStyle.Bold,
                CdaTheme.TextDark, ContentAlignment.MiddleLeft);
            var ib = new CdaInputBox(cue, password) { Bounds = new Rectangle(30, Y(y) + 22, 340, Math.Max(36, Y(44))) };
            Host.Controls.Add(ib);
            return ib;
        }

        // "Don't have an account? Register" - built from two plain Labels because LinkLabel
        // is clipped to half its height on the Android WinForms layer.
        protected Control AddFooterLink(string text, int linkStart, int linkLen, int y, Action click)
        {
            string pre = text.Substring(0, linkStart).TrimEnd();
            string link = text.Substring(linkStart, linkLen);
            string post = text.Substring(linkStart + linkLen);
            var fMuted = new Font("Segoe UI", 9.5f);
            var fLink = new Font("Segoe UI", 9.5f, FontStyle.Bold);

            float w1, w2, w3 = 0;
            try
            {
                using (var g = Card.CreateGraphics())
                {
                    w1 = g.MeasureString(pre, fMuted).Width;
                    w2 = g.MeasureString(link, fLink).Width;
                    if (post.Length > 0) w3 = g.MeasureString(post, fMuted).Width;
                }
            }
            catch
            {
                w1 = pre.Length * 7.5f; w2 = link.Length * 8f; w3 = post.Length * 7.5f;
            }

            int gap = 5, h = 26;
            int total = (int)(w1 + gap + w2 + (post.Length > 0 ? gap + w3 : 0));
            int x = Math.Max(0, (400 - total) / 2);
            int yy = Y(y);

            Func<string, Font, Color, int, int, Label> mk = (t, f, c, lx, lw) =>
            {
                var l = new Label
                {
                    Text = t, Font = f, ForeColor = c, BackColor = Color.White,
                    AutoSize = false, TextAlign = ContentAlignment.MiddleLeft,
                    Bounds = new Rectangle(lx, yy, lw, h)
                };
                Host.Controls.Add(l);
                return l;
            };

            mk(pre, fMuted, CdaTheme.TextMuted, x, (int)w1 + 4);
            var lnk = mk(link, fLink, CdaTheme.Primary, x + (int)w1 + gap, (int)w2 + 6);
            lnk.Cursor = Cursors.Hand;
            lnk.Click += (s2, e2) => click();
            if (post.Length > 0)
                mk(post, fMuted, CdaTheme.TextMuted, x + (int)w1 + gap + (int)w2 + gap, (int)w3 + 4);
            return lnk;
        }

        // scale the picture to "cover" the screen; the left edge stays anchored so the
        // headline text is never cut off (extra width is cropped from the right).
        void EnsureBgCache()
        {
            int W = ClientSize.Width, H = ClientSize.Height;
            if (bg == null || W <= 0 || H <= 0) return;
            if (bgCache != null && bgCacheSize.Width == W && bgCacheSize.Height == H) return;

            if (bgCache != null) bgCache.Dispose();
            bgCache = new Bitmap(W, H);
            bgCacheSize = new Size(W, H);

            using (var g = Graphics.FromImage(bgCache))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                float s = Math.Max((float)W / bg.Width, (float)H / bg.Height);
                int w = (int)Math.Ceiling(bg.Width * s), h = (int)Math.Ceiling(bg.Height * s);
                g.DrawImage(bg, 0, (H - h) / 2, w, h);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (bg != null)
            {
                EnsureBgCache();
                if (bgCache != null)
                {
                    e.Graphics.DrawImageUnscaled(bgCache, 0, 0);
                    return;
                }
            }

            if (ClientRectangle.Width <= 0 || ClientRectangle.Height <= 0)
            {
                base.OnPaintBackground(e);
                return;
            }
            using (var b = new LinearGradientBrush(ClientRectangle, CdaTheme.Bg1, CdaTheme.Bg2, 45f))
                e.Graphics.FillRectangle(b, ClientRectangle);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // soft lavender shadow so the white card lifts off the photo
            if (bg != null && Card != null)
            {
                var sg = e.Graphics;
                sg.SmoothingMode = SmoothingMode.AntiAlias;
                for (int i = 10; i >= 1; i--)
                {
                    var r = Card.Bounds;
                    r.Inflate(i * 2, i * 2);
                    r.Offset(0, 8);
                    using (var p = CdaTheme.Round(r, 22 + i * 2))
                    using (var b = new SolidBrush(Color.FromArgb(7, 40, 30, 110)))
                        sg.FillPath(b, p);
                }
            }

            if (hero != null && !heroRect.IsEmpty)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                using (var path = CdaTheme.Round(heroRect, 24))
                {
                    var old = g.Clip;
                    g.SetClip(path, CombineMode.Replace);
                    float s = Math.Max((float)heroRect.Width / hero.Width, (float)heroRect.Height / hero.Height);
                    int w = (int)(hero.Width * s), h = (int)(hero.Height * s);
                    g.DrawImage(hero, heroRect.X + (heroRect.Width - w) / 2, heroRect.Y + (heroRect.Height - h) / 2, w, h);
                    g.Clip = old;
                }
            }
        }
    }

    // ---------------------------------------------------------------
    //  Login screen
    // ---------------------------------------------------------------
    public class LoginForm : CdaThemedForm
    {
        // Login and Register are two pages inside the same card. A second full-screen form
        // (ShowDialog) never appeared on the Android WinForms layer, so everything lives here.
        const int LoginH = 520, RegH = 600;
        CdaRoundPanel pageLogin, pageReg;
        CdaInputBox txtEmail, txtPass;
        CdaInputBox regName, regEmail, regPass, regConfirm;
        CdaRoundButton btnLogin, btnRegister;
        Label lblMsg, lblRegMsg;

        public string RegisteredName { get; private set; }

        CdaRoundPanel NewPage()
        {
            var p = new CdaRoundPanel { Radius = 22, Fill = Color.White, Stroke = Color.Transparent };
            Card.Controls.Add(p);
            return p;
        }

        public LoginForm() : base(RegH, true)
        {
            // ---------- login page ----------
            pageLogin = NewPage();
            Host = pageLogin;
            AddHeader();
            AddTabs(true, () => SetMode(true));

            txtEmail = AddField("Email Address", "you@example.com", false, 196);
            txtPass = AddField("Password", "Enter your password", true, 276);

            lblMsg = MakeLabel(Host, "", 30, Y(350), 340, Y(36), 9.5f, FontStyle.Regular,
                CdaTheme.Error, ContentAlignment.MiddleCenter);

            btnLogin = new CdaRoundButton { Text = "Login", Bounds = new Rectangle(30, Y(396), 340, Math.Max(36, Y(46))) };
            btnLogin.Click += DoLogin;
            Host.Controls.Add(btnLogin);

            AddFooterLink("Don't have an account? Register", 23, 8, 462, () => SetMode(true));

            // ---------- register page ----------
            pageReg = NewPage();
            Host = pageReg;
            AddHeader();
            AddTabs(false, () => SetMode(false));

            regName = AddField("Full Name", "Enter your name", false, 184);
            regEmail = AddField("Email Address", "you@example.com", false, 258);
            regPass = AddField("Password (min 6 characters)", "Create a password", true, 332);
            regConfirm = AddField("Confirm Password", "Re-enter your password", true, 406);

            lblRegMsg = MakeLabel(Host, "", 30, Y(474), 340, Y(26), 9.5f, FontStyle.Regular,
                CdaTheme.Error, ContentAlignment.MiddleCenter);

            btnRegister = new CdaRoundButton { Text = "Register", Bounds = new Rectangle(30, Y(504), 340, Math.Max(36, Y(46))) };
            btnRegister.Click += DoRegister;
            Host.Controls.Add(btnRegister);

            AddFooterLink("Already have an account? Login", 25, 5, 560, () => SetMode(false));

            Host = Card;
            SetMode(false);

            Shown += (s, e) => { Activate(); txtEmail.Inner.Focus(); };
        }

        void SetMode(bool register)
        {
            int h = Y(register ? RegH : LoginH);
            Card.Height = h;
            Card.Top = Math.Max(12, (Height - h) / 2);
            pageLogin.Bounds = new Rectangle(0, 0, 400, h);
            pageReg.Bounds = new Rectangle(0, 0, 400, h);
            pageLogin.Visible = !register;
            pageReg.Visible = register;
            AcceptButton = register ? btnRegister : btnLogin;
            lblMsg.Text = "";
            lblRegMsg.Text = "";
            Invalidate();
            try { (register ? regName : txtEmail).Inner.Focus(); } catch { }
        }

        async void DoLogin(object sender, EventArgs e)
        {
            lblMsg.ForeColor = CdaTheme.Error;
            lblMsg.Text = "";
            if (txtEmail.Value.Trim() == "" || txtPass.Value == "")
            {
                lblMsg.Text = "Enter email and password";
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Please wait...";
            var err = await AuthService.SignIn(txtEmail.Value.Trim(), txtPass.Value);
            btnLogin.Enabled = true;
            btnLogin.Text = "Login";

            if (err == null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                lblMsg.Text = err;
            }
        }

        async void DoRegister(object sender, EventArgs e)
        {
            lblRegMsg.ForeColor = CdaTheme.Error;
            lblRegMsg.Text = "";
            string name = regName.Value.Trim();
            string email = regEmail.Value.Trim();

            if (name == "") { lblRegMsg.Text = "Enter your name"; return; }
            if (email == "" || email.IndexOf('@') < 1) { lblRegMsg.Text = "Enter a valid email"; return; }
            if (regPass.Value.Length < 6) { lblRegMsg.Text = "Password must be at least 6 characters"; return; }
            if (regPass.Value != regConfirm.Value) { lblRegMsg.Text = "Passwords do not match"; return; }

            btnRegister.Enabled = false;
            btnRegister.Text = "Please wait...";
            var err = await AuthService.SignUp(email, regPass.Value);
            btnRegister.Enabled = true;
            btnRegister.Text = "Register";

            if (err == null)
            {
                RegisteredName = name;
                AuthService.UserEmail = null;
                AuthService.IdToken = null;

                txtEmail.Value = email;
                txtPass.Value = "";
                regName.Value = ""; regEmail.Value = ""; regPass.Value = ""; regConfirm.Value = "";
                SetMode(false);
                lblMsg.ForeColor = CdaTheme.Ok;
                lblMsg.Text = "Account created. Please login.";
            }
            else
            {
                lblRegMsg.Text = err;
            }
        }
    }
}
