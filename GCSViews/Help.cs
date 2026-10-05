using MissionPlanner.Controls;
using MissionPlanner.Properties;
using MissionPlanner.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    public partial class Help : MyUserControl, IActivate
    {
        // edit these links to your own CDA links
        private const string UrlPlanner = "https://ardupilot.org/planner";
        private const string UrlFaq = "https://ardupilot.org/planner";
        private const string UrlStart = "https://ardupilot.org/planner";
        private const string UrlTrouble = "https://discuss.ardupilot.org";
        private const string UrlContact = "https://discuss.ardupilot.org";
        private const string UrlVideos = "https://www.youtube.com/results?search_query=mission+planner+tutorial";
        private const string UrlDownloads = "https://firmware.ardupilot.org/Tools/MissionPlanner/";

        private bool _built;
        private bool _rtfLoaded;
        private string _mainTitle = "Quick Help";
        private string _mainSub = "Common topics to get you started";
        private string _mainIcon = "bulb";

        public Help()
        {
            InitializeComponent();
            _built = true;
            ApplyColors();
            SelectNav(navHome);
            LayoutAll();
        }

        public void Activate()
        {
            try
            {
                CHK_showconsole.Checked = Settings.Instance.GetBoolean("showconsole");
            }
            catch
            {
            }

            if (Program.WindowsStoreApp)
            {
                BUT_betaupdate.Visible = false;
                BUT_updatecheck.Visible = false;
            }

            ApplyColors();
            LayoutAll();
        }

        public void BUT_updatecheck_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.WindowsStoreApp)
                {
                    return;
                }
                Utilities.Update.CheckForUpdate(true);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.ToString(), Strings.ERROR);
            }
        }

        private void CHK_showconsole_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Instance["showconsole"] = CHK_showconsole.Checked.ToString();
        }

        private void Help_Load(object sender, EventArgs e)
        {
            ApplyColors();
            LayoutAll();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Open("https://firmware.ardupilot.org/Tools/MissionPlanner/upgrade/ChangeLog.txt");
        }

        private void BUT_betaupdate_Click(object sender, EventArgs e)
        {
            try
            {
                Utilities.Update.dobeta = true;
                if (Control.ModifierKeys == Keys.Control)
                {
                    Utilities.Update.domaster = true;
                    CustomMessageBox.Show("This will update to MASTER release");
                }

                Utilities.Update.DoUpdate();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.ToString(), Strings.ERROR);
            }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Open(e.LinkText);
        }

        private void BUT_contact_Click(object sender, EventArgs e)
        {
            Open(UrlContact);
        }

        private void Nav_Click(object sender, EventArgs e)
        {
            HlpNav n = (HlpNav)sender;
            switch ((string)n.Tag)
            {
                case "home":
                    SelectNav(n);
                    ShowHome();
                    break;
                case "guide":
                    SelectNav(n);
                    ShowGuide();
                    break;
                case "faq":
                    Open(UrlFaq);
                    break;
                case "start":
                    Open(UrlStart);
                    break;
                case "trouble":
                    Open(UrlTrouble);
                    break;
                case "contact":
                    Open(UrlContact);
                    break;
            }
        }

        private void Card_Click(object sender, EventArgs e)
        {
            HlpCard c = (HlpCard)sender;
            switch ((string)c.Tag)
            {
                case "start":
                    Open(UrlStart);
                    break;
                case "setup":
                    GoTo("HWConfig");
                    break;
                case "sim":
                    GoTo("Simulation");
                    break;
                case "config":
                    GoTo("SWConfig");
                    break;
                case "trouble":
                    Open(UrlTrouble);
                    break;
                case "support":
                    Open(UrlContact);
                    break;
            }
        }

        private void Link_Click(object sender, EventArgs e)
        {
            HlpLink l = (HlpLink)sender;
            switch ((string)l.Tag)
            {
                case "manual":
                    Open(UrlPlanner);
                    break;
                case "video":
                    Open(UrlVideos);
                    break;
                case "download":
                    Open(UrlDownloads);
                    break;
            }
        }

        private static void Open(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.Message, Strings.ERROR);
            }
        }

        private static void GoTo(string screen)
        {
            try
            {
                MainV2.View.ShowScreen(screen);
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show(ex.Message, Strings.ERROR);
            }
        }

        private void SelectNav(HlpNav active)
        {
            HlpNav[] all = { navHome, navFaq, navStart, navGuide, navTrouble, navContact };
            foreach (HlpNav n in all)
            {
                n.Selected = (n == active);
            }
        }

        private void ShowHome()
        {
            _mainTitle = "Quick Help";
            _mainSub = "Common topics to get you started";
            _mainIcon = "bulb";
            richTextBox1.Visible = false;
            foreach (HlpCard c in Cards())
            {
                c.Visible = true;
            }
            pnlMain.Invalidate();
        }

        private void ShowGuide()
        {
            _mainTitle = "System Guide";
            _mainSub = "Shortcuts and usage reference";
            _mainIcon = "gear";

            if (!_rtfLoaded)
            {
                try
                {
                    richTextBox1.Rtf = Resources.help_text;
                    richTextBox1.SelectAll();
                    richTextBox1.SelectionColor = HlpTheme.Ink;
                    richTextBox1.Select(0, 0);
                    _rtfLoaded = true;
                }
                catch
                {
                }
            }

            foreach (HlpCard c in Cards())
            {
                c.Visible = false;
            }
            richTextBox1.Visible = true;
            richTextBox1.BringToFront();
            pnlMain.Invalidate();
        }

        private HlpCard[] Cards()
        {
            return new HlpCard[] { cardStart, cardSetup, cardSim, cardConfig, cardTrouble, cardSupport };
        }

        private void ApplyColors()
        {
            BackColor = HlpTheme.Bg;
            ForeColor = HlpTheme.Ink;

            richTextBox1.BackColor = Color.White;
            richTextBox1.ForeColor = HlpTheme.Ink;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Font = HlpTheme.F("Segoe UI", 10.5f, FontStyle.Regular);

            CHK_showconsole.Font = HlpTheme.F("Segoe UI", 9.5f, FontStyle.Regular);
            CHK_showconsole.BackColor = Color.Transparent;
            CHK_showconsole.ForeColor = HlpTheme.Ink;
            CHK_showconsole.FlatStyle = FlatStyle.Flat;
            CHK_showconsole.FlatAppearance.BorderColor = HlpTheme.Primary;
            CHK_showconsole.FlatAppearance.CheckedBackColor = HlpTheme.Soft;
            CHK_showconsole.FlatAppearance.MouseOverBackColor = HlpTheme.Soft;

            linkLabel1.Font = HlpTheme.F("Segoe UI", 9.5f, FontStyle.Regular);
            linkLabel1.BackColor = Color.Transparent;
            linkLabel1.LinkColor = HlpTheme.Primary;
            linkLabel1.ActiveLinkColor = HlpTheme.PrimaryDark;
            linkLabel1.VisitedLinkColor = HlpTheme.Primary;
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            LayoutAll();
        }

        private void LayoutAll()
        {
            if (!_built) return;
            int W = ClientSize.Width;
            int H = ClientSize.Height;
            if (W < 300 || H < 300) return;

            const int m = 16;
            const int gap = 14;
            const int navW = 220;
            const int rightW = 300;
            const int footH = 54;
            const int supH = 160;
            const int updH = 200;

            int heroH = H < 680 ? 150 : 190;
            int top = m + heroH + gap;
            int bodyH = Math.Max(H - footH - top, 544);

            pnlHero.SetBounds(m, m, W - 2 * m, heroH);
            pnlFooter.SetBounds(0, top + bodyH, W, footH);

            pnlNav.SetBounds(m, top, navW, bodyH);
            HlpNav[] navs = { navHome, navFaq, navStart, navGuide, navTrouble, navContact };
            for (int i = 0; i < navs.Length; i++)
            {
                navs[i].SetBounds(14, 22 + i * 52, navW - 28, 44);
            }

            int mainX = m + navW + gap;
            int mainW = Math.Max(W - m - rightW - gap - mainX, 360);
            pnlMain.SetBounds(mainX, top, mainW, bodyH);

            const int pad = 22;
            const int hdr = 80;
            const int cg = 16;
            int cw = (mainW - 2 * pad - 2 * cg) / 3;
            int availH = bodyH - hdr - pad;
            int ch = Math.Max(100, Math.Min(150, (availH - cg) / 2));
            HlpCard[] cards = Cards();
            for (int i = 0; i < cards.Length; i++)
            {
                int col = i % 3;
                int row = i / 3;
                cards[i].SetBounds(pad + col * (cw + cg), hdr + row * (ch + cg), cw, ch);
            }
            richTextBox1.SetBounds(pad, hdr, mainW - 2 * pad, bodyH - hdr - pad);

            int rx = W - m - rightW;
            int resH = Math.Max(156, bodyH - supH - updH - 2 * gap);
            pnlSupport.SetBounds(rx, top, rightW, supH);
            pnlResources.SetBounds(rx, top + supH + gap, rightW, resH);
            pnlUpdates.SetBounds(rx, top + supH + resH + 2 * gap, rightW, updH);

            BUT_contact.SetBounds(18, supH - 52, rightW - 36, 36);

            HlpLink[] links = { linkManual, linkVideos, linkDownloads };
            for (int i = 0; i < links.Length; i++)
            {
                links[i].SetBounds(10, 48 + i * 36, rightW - 20, 32);
            }

            int ux = 16;
            int uw = rightW - 32;
            int y = 52;
            if (BUT_updatecheck.Visible)
            {
                BUT_updatecheck.SetBounds(ux, y, uw, 36);
                y += 44;
            }
            if (BUT_betaupdate.Visible)
            {
                BUT_betaupdate.SetBounds(ux, y, uw, 36);
                y += 44;
            }
            CHK_showconsole.Location = new Point(ux, y + 4);
            linkLabel1.Location = new Point(ux + 2, y + 34);
        }

        private void pnlHero_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            int w = pnlHero.Width;
            int h = pnlHero.Height;
            bool big = h >= 180;

            using (SolidBrush cloud = new SolidBrush(Color.FromArgb(120, 255, 255, 255)))
            {
                g.FillEllipse(cloud, w * 0.30f, h * 0.70f, w * 0.18f, h * 0.70f);
                g.FillEllipse(cloud, w * 0.44f, h * 0.55f, w * 0.20f, h * 0.90f);
                g.FillEllipse(cloud, w * 0.62f, h * 0.75f, w * 0.18f, h * 0.70f);
                g.FillEllipse(cloud, w * 0.80f, h * 0.60f, w * 0.22f, h * 0.90f);
            }

            int x = 32;
            int y = big ? 24 : 14;

            Rectangle badge = new Rectangle(x, y, 150, 26);
            using (GraphicsPath bp = HlpTheme.Round(badge, 13))
            using (LinearGradientBrush bb = new LinearGradientBrush(badge, HlpTheme.Primary, HlpTheme.PrimaryLight, 0f))
            {
                g.FillPath(bb, bp);
            }
            HlpTheme.DrawTextC(g, "HELP & SUPPORT", HlpTheme.F("Segoe UI", 8.5f, FontStyle.Bold), Color.White, badge);

            Font ft = HlpTheme.F("Segoe UI", big ? 28f : 22f, FontStyle.Bold);
            float ty = y + (big ? 32 : 30);
            SizeF s1 = g.MeasureString("We're Here to", ft, 3000, StringFormat.GenericTypographic);
            using (SolidBrush ink = new SolidBrush(HlpTheme.Ink))
            {
                g.DrawString("We're Here to", ft, ink, x, ty, StringFormat.GenericTypographic);
            }
            using (SolidBrush pb = new SolidBrush(HlpTheme.Primary))
            {
                g.DrawString("Help!", ft, pb, x + s1.Width + 10, ty, StringFormat.GenericTypographic);
            }

            float dy = big ? 106 : 84;
            float dw = Math.Min(w * 0.42f, 560f);
            HlpTheme.DrawText(g,
                "Find answers to common questions, get support, and make the most of your learning journey with Chennai Drone Academy.",
                HlpTheme.F("Segoe UI", 9.5f, FontStyle.Regular), Color.FromArgb(70, 58, 130),
                new RectangleF(x, dy, dw, 40));

            if (big)
            {
                HlpTheme.DrawText(g, "Fly Higher with Us", HlpTheme.F("Segoe Script", 15f, FontStyle.Regular),
                    HlpTheme.Primary, new RectangleF(x, h - 46, 300, 34));
            }

            if (w >= 1200)
            {
                DrawBubble(g, w - 170, 28);
                DrawGirl(g, w - 420, h);
                DrawDrone(g, w - 720, 34);
            }
        }

        private static void DrawBubble(Graphics g, float x, float y)
        {
            using (SolidBrush b = new SolidBrush(Color.FromArgb(200, 183, 252)))
            using (Pen spark = new Pen(Color.White, 2.5f))
            {
                spark.StartCap = LineCap.Round;
                spark.EndCap = LineCap.Round;
                g.FillEllipse(b, x, y, 92, 92);
                g.FillPolygon(b, new PointF[] { new PointF(x + 14, y + 70), new PointF(x + 6, y + 100), new PointF(x + 38, y + 84) });
                g.DrawLine(spark, x - 26, y + 26, x - 14, y + 32);
                g.DrawLine(spark, x - 28, y + 44, x - 14, y + 44);
                g.DrawLine(spark, x - 26, y + 62, x - 14, y + 56);
            }
            HlpTheme.DrawTextC(g, "?", HlpTheme.F("Segoe UI", 40f, FontStyle.Bold), Color.White, new RectangleF(x, y, 92, 92));
        }

        private static void DrawDrone(Graphics g, float x, float y)
        {
            using (Pen arm = new Pen(Color.FromArgb(238, 233, 252), 5f))
            using (Pen outline = new Pen(Color.FromArgb(185, 170, 240), 1.5f))
            using (SolidBrush body = new SolidBrush(Color.White))
            using (SolidBrush prop = new SolidBrush(Color.FromArgb(170, 255, 255, 255)))
            using (SolidBrush dark = new SolidBrush(Color.FromArgb(60, 48, 110)))
            using (SolidBrush lens = new SolidBrush(Color.FromArgb(150, 130, 230)))
            {
                arm.StartCap = LineCap.Round;
                arm.EndCap = LineCap.Round;
                PointF[] hubs =
                {
                    new PointF(x + 15, y + 22), new PointF(x + 135, y + 22),
                    new PointF(x + 30, y + 66), new PointF(x + 120, y + 66)
                };
                foreach (PointF hp in hubs)
                {
                    g.DrawLine(arm, x + 75, y + 48, hp.X, hp.Y);
                }
                foreach (PointF hp in hubs)
                {
                    g.FillEllipse(prop, hp.X - 28, hp.Y - 5, 56, 10);
                    g.DrawEllipse(outline, hp.X - 28, hp.Y - 5, 56, 10);
                }
                g.FillEllipse(body, x + 53, y + 34, 44, 28);
                g.DrawEllipse(outline, x + 53, y + 34, 44, 28);
                g.FillEllipse(dark, x + 66, y + 54, 18, 18);
                g.FillEllipse(lens, x + 71, y + 59, 8, 8);
            }
        }

        private static void DrawGirl(Graphics g, float cx, float h)
        {
            using (SolidBrush hair = new SolidBrush(Color.FromArgb(78, 52, 140)))
            using (SolidBrush skin = new SolidBrush(Color.FromArgb(255, 226, 208)))
            using (SolidBrush hood = new SolidBrush(Color.FromArgb(150, 124, 240)))
            using (SolidBrush cup = new SolidBrush(HlpTheme.Primary))
            using (SolidBrush dark = new SolidBrush(Color.FromArgb(60, 40, 110)))
            using (SolidBrush lap = new SolidBrush(Color.FromArgb(232, 226, 252)))
            using (SolidBrush logo = new SolidBrush(Color.FromArgb(160, 138, 245)))
            using (Pen band = new Pen(HlpTheme.Primary, 4f))
            using (Pen mic = new Pen(HlpTheme.Primary, 2.5f))
            using (Pen smile = new Pen(Color.FromArgb(200, 110, 110), 1.8f))
            {
                g.FillEllipse(hair, cx - 44, h - 168, 88, 96);
                g.FillEllipse(hood, cx - 92, h - 66, 184, 130);
                g.FillRectangle(skin, cx - 9, h - 84, 18, 24);
                g.FillEllipse(skin, cx - 32, h - 150, 64, 72);
                g.FillPie(hair, cx - 36, h - 156, 72, 70, 180f, 180f);
                g.FillEllipse(dark, cx - 15, h - 110, 5, 7);
                g.FillEllipse(dark, cx + 10, h - 110, 5, 7);
                g.DrawArc(smile, cx - 8, h - 100, 16, 10, 20f, 140f);
                g.DrawArc(band, cx - 46, h - 160, 92, 92, 195f, 150f);
                g.FillEllipse(cup, cx - 50, h - 118, 16, 28);
                g.FillEllipse(cup, cx + 34, h - 118, 16, 28);
                g.DrawLine(mic, cx - 42, h - 92, cx - 24, h - 80);
                g.FillEllipse(cup, cx - 28, h - 84, 8, 8);
                using (GraphicsPath lp = HlpTheme.Round(new Rectangle((int)cx - 78, (int)h - 36, 156, 60), 10))
                {
                    g.FillPath(lap, lp);
                }
                g.FillEllipse(logo, cx - 8, h - 24, 16, 16);
            }
        }

        private void pnlNav_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            int h = pnlNav.Height;
            Font fs = HlpTheme.F("Segoe Script", 12f, FontStyle.Regular);
            HlpTheme.DrawText(g, "We're Glad", fs, HlpTheme.Primary, new RectangleF(26, h - 80, 180, 26));
            HlpTheme.DrawText(g, "You're Here!", fs, HlpTheme.Primary, new RectangleF(44, h - 58, 150, 26));
            HlpTheme.DrawText(g, "\u2665", HlpTheme.F("Segoe UI Symbol", 11f, FontStyle.Regular), HlpTheme.PrimaryLight,
                new RectangleF(160, h - 56, 26, 24));
        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            int w = pnlMain.Width;
            using (SolidBrush b = new SolidBrush(HlpTheme.Soft))
            {
                g.FillEllipse(b, 22, 16, 46, 46);
            }
            HlpTheme.DrawIcon(g, _mainIcon, new RectangleF(33, 27, 24, 24), HlpTheme.Primary);
            HlpTheme.DrawText(g, _mainTitle, HlpTheme.F("Segoe UI", 16f, FontStyle.Bold), HlpTheme.Ink,
                new RectangleF(82, 14, w - 100, 30));
            HlpTheme.DrawText(g, _mainSub, HlpTheme.F("Segoe UI", 9.5f, FontStyle.Regular), HlpTheme.Muted,
                new RectangleF(82, 44, w - 100, 20));
        }

        private void pnlSupport_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            int w = pnlSupport.Width;
            using (SolidBrush b = new SolidBrush(Color.White))
            {
                g.FillEllipse(b, 18, 16, 44, 44);
            }
            HlpTheme.DrawIcon(g, "chat", new RectangleF(28, 26, 24, 24), HlpTheme.Primary);
            HlpTheme.DrawText(g, "Still Need Help?", HlpTheme.F("Segoe UI", 12.5f, FontStyle.Bold), HlpTheme.Ink,
                new RectangleF(74, 14, w - 90, 26));
            HlpTheme.DrawText(g, "Our team is ready to assist you with any questions or issues.",
                HlpTheme.F("Segoe UI", 9f, FontStyle.Regular), HlpTheme.Muted,
                new RectangleF(74, 42, w - 90, 56));
        }

        private void pnlResources_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            using (SolidBrush b = new SolidBrush(HlpTheme.Soft))
            {
                g.FillEllipse(b, 14, 10, 30, 30);
            }
            HlpTheme.DrawIcon(g, "book", new RectangleF(19, 15, 20, 20), HlpTheme.Primary);
            HlpTheme.DrawText(g, "Useful Resources", HlpTheme.F("Segoe UI", 11.5f, FontStyle.Bold), HlpTheme.Ink,
                new RectangleF(54, 12, pnlResources.Width - 70, 26));
        }

        private void pnlUpdates_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            using (SolidBrush b = new SolidBrush(HlpTheme.Soft))
            {
                g.FillEllipse(b, 14, 10, 30, 30);
            }
            HlpTheme.DrawIcon(g, "download", new RectangleF(19, 15, 20, 20), HlpTheme.Primary);
            HlpTheme.DrawText(g, "Software Updates", HlpTheme.F("Segoe UI", 11.5f, FontStyle.Bold), HlpTheme.Ink,
                new RectangleF(54, 12, pnlUpdates.Width - 70, 26));
        }

        private void pnlFooter_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            int w = pnlFooter.Width;
            float cx = w / 2f;
            using (Pen p = new Pen(HlpTheme.Border, 1.5f))
            {
                g.DrawLine(p, cx - 260, 16, cx - 28, 16);
                g.DrawLine(p, cx + 28, 16, cx + 260, 16);
            }
            HlpTheme.DrawIcon(g, "drone", new RectangleF(cx - 11, 5, 22, 22), HlpTheme.Primary);
            HlpTheme.DrawTextC(g, "Learn   \u00B7   Practice   \u00B7   Fly",
                HlpTheme.F("Segoe UI", 9.5f, FontStyle.Regular), HlpTheme.Muted,
                new RectangleF(0, 28, w, 22));
        }
    }

    internal static class HlpTheme
    {
        public static readonly Color Bg = Color.FromArgb(243, 240, 255);
        public static readonly Color Soft = Color.FromArgb(237, 232, 255);
        public static readonly Color Border = Color.FromArgb(226, 219, 250);
        public static readonly Color Primary = Color.FromArgb(111, 84, 230);
        public static readonly Color PrimaryLight = Color.FromArgb(155, 123, 255);
        public static readonly Color PrimaryDark = Color.FromArgb(76, 50, 190);
        public static readonly Color Ink = Color.FromArgb(40, 24, 110);
        public static readonly Color Muted = Color.FromArgb(110, 102, 150);

        private static readonly Dictionary<string, Font> Fonts = new Dictionary<string, Font>();

        public static Font F(string family, float pt, FontStyle style)
        {
            string key = family + "|" + pt + "|" + (int)style;
            Font f;
            if (!Fonts.TryGetValue(key, out f))
            {
                f = new Font(family, pt * 96f / 72f, style, GraphicsUnit.Pixel);
                Fonts[key] = f;
            }
            return f;
        }

        public static GraphicsPath Round(Rectangle rc, int r)
        {
            GraphicsPath p = new GraphicsPath();
            int d = Math.Min(r * 2, Math.Min(rc.Width, rc.Height));
            if (r <= 0 || d <= 0)
            {
                p.AddRectangle(rc);
                return p;
            }
            p.AddArc(rc.X, rc.Y, d, d, 180, 90);
            p.AddArc(rc.Right - d, rc.Y, d, d, 270, 90);
            p.AddArc(rc.Right - d, rc.Bottom - d, d, d, 0, 90);
            p.AddArc(rc.X, rc.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        public static void DrawText(Graphics g, string s, Font f, Color c, RectangleF r)
        {
            DrawText(g, s, f, c, r, StringAlignment.Near, StringAlignment.Near);
        }

        public static void DrawTextC(Graphics g, string s, Font f, Color c, RectangleF r)
        {
            DrawText(g, s, f, c, r, StringAlignment.Center, StringAlignment.Center);
        }

        public static void DrawText(Graphics g, string s, Font f, Color c, RectangleF r, StringAlignment h, StringAlignment v)
        {
            using (SolidBrush b = new SolidBrush(c))
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = h;
                sf.LineAlignment = v;
                sf.Trimming = StringTrimming.EllipsisWord;
                g.DrawString(s, f, b, r, sf);
            }
        }

        private static PointF[] Pts(params float[] v)
        {
            PointF[] r = new PointF[v.Length / 2];
            for (int i = 0; i < r.Length; i++)
            {
                r[i] = new PointF(v[2 * i], v[2 * i + 1]);
            }
            return r;
        }

        public static void DrawIcon(Graphics g, string name, RectangleF r, Color c)
        {
            GraphicsState st = g.Save();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(r.X, r.Y);
            g.ScaleTransform(r.Width / 24f, r.Height / 24f);
            using (Pen p = new Pen(c, 1.9f))
            using (SolidBrush b = new SolidBrush(c))
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                p.LineJoin = LineJoin.Round;
                switch (name)
                {
                    case "home":
                        g.DrawLines(p, Pts(3, 12, 12, 3, 21, 12));
                        g.DrawLines(p, Pts(5, 10, 5, 21, 19, 21, 19, 10));
                        g.DrawRectangle(p, 10f, 14f, 4f, 7f);
                        break;
                    case "faq":
                        g.DrawEllipse(p, 3f, 3f, 18f, 18f);
                        g.DrawArc(p, 9f, 7f, 6f, 6f, 180f, 270f);
                        g.DrawLine(p, 12f, 13f, 12f, 15f);
                        g.FillEllipse(b, 11f, 17f, 2f, 2f);
                        break;
                    case "cap":
                        g.DrawPolygon(p, Pts(12, 5, 23, 10, 12, 15, 1, 10));
                        g.DrawLines(p, Pts(6, 12.5f, 6, 17, 12, 20, 18, 17, 18, 12.5f));
                        break;
                    case "gear":
                        using (Pen t = new Pen(c, 3.4f))
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                double a = i * Math.PI / 4.0;
                                g.DrawLine(t,
                                    (float)(12 + 6.5 * Math.Cos(a)), (float)(12 + 6.5 * Math.Sin(a)),
                                    (float)(12 + 10 * Math.Cos(a)), (float)(12 + 10 * Math.Sin(a)));
                            }
                        }
                        g.DrawEllipse(p, 5.5f, 5.5f, 13f, 13f);
                        g.DrawEllipse(p, 9f, 9f, 6f, 6f);
                        break;
                    case "wrench":
                        g.DrawLine(p, 4f, 20f, 13f, 11f);
                        g.DrawEllipse(p, 12f, 3f, 8f, 8f);
                        break;
                    case "mail":
                        g.DrawRectangle(p, 3f, 5f, 18f, 14f);
                        g.DrawLines(p, Pts(3, 6, 12, 13, 21, 6));
                        break;
                    case "book":
                        g.DrawLines(p, Pts(12, 6, 7, 4, 3, 5, 3, 18, 7, 17, 12, 20));
                        g.DrawLines(p, Pts(12, 6, 17, 4, 21, 5, 21, 18, 17, 17, 12, 20));
                        g.DrawLine(p, 12f, 6f, 12f, 20f);
                        break;
                    case "drone":
                        g.DrawEllipse(p, 9f, 9f, 6f, 6f);
                        g.DrawLine(p, 10f, 10f, 6f, 6f);
                        g.DrawLine(p, 14f, 10f, 18f, 6f);
                        g.DrawLine(p, 10f, 14f, 6f, 18f);
                        g.DrawLine(p, 14f, 14f, 18f, 18f);
                        g.DrawEllipse(p, 2f, 2f, 6f, 6f);
                        g.DrawEllipse(p, 16f, 2f, 6f, 6f);
                        g.DrawEllipse(p, 2f, 16f, 6f, 6f);
                        g.DrawEllipse(p, 16f, 16f, 6f, 6f);
                        break;
                    case "game":
                        using (GraphicsPath gp = Round(new Rectangle(2, 7, 20, 11), 5))
                        {
                            g.DrawPath(p, gp);
                        }
                        g.DrawLine(p, 7f, 10f, 7f, 15f);
                        g.DrawLine(p, 4.5f, 12.5f, 9.5f, 12.5f);
                        g.FillEllipse(b, 14.5f, 10f, 2.4f, 2.4f);
                        g.FillEllipse(b, 17.5f, 13f, 2.4f, 2.4f);
                        break;
                    case "warn":
                        g.DrawPolygon(p, Pts(12, 3, 22, 20, 2, 20));
                        g.DrawLine(p, 12f, 9f, 12f, 14f);
                        g.FillEllipse(b, 11f, 16f, 2f, 2f);
                        break;
                    case "headset":
                        g.DrawArc(p, 4f, 3f, 16f, 16f, 180f, 180f);
                        g.DrawRectangle(p, 3f, 11f, 4f, 7f);
                        g.DrawRectangle(p, 17f, 11f, 4f, 7f);
                        g.DrawLines(p, Pts(19, 18, 19, 20, 14, 21));
                        break;
                    case "doc":
                        g.DrawLines(p, Pts(6, 3, 14, 3, 19, 8, 19, 21, 6, 21, 6, 3));
                        g.DrawLines(p, Pts(14, 3, 14, 8, 19, 8));
                        g.DrawLine(p, 9f, 13f, 16f, 13f);
                        g.DrawLine(p, 9f, 17f, 16f, 17f);
                        break;
                    case "video":
                        g.DrawRectangle(p, 3f, 6f, 18f, 12f);
                        g.FillPolygon(b, Pts(10, 9, 15.5f, 12, 10, 15));
                        break;
                    case "download":
                        g.DrawLine(p, 12f, 3f, 12f, 15f);
                        g.DrawLines(p, Pts(7, 10, 12, 15, 17, 10));
                        g.DrawLines(p, Pts(4, 15, 4, 20, 20, 20, 20, 15));
                        break;
                    case "bulb":
                        g.DrawEllipse(p, 6f, 2f, 12f, 12f);
                        g.DrawLine(p, 9f, 17.5f, 15f, 17.5f);
                        g.DrawLine(p, 10f, 21f, 14f, 21f);
                        g.DrawLine(p, 10f, 14f, 10f, 17.5f);
                        g.DrawLine(p, 14f, 14f, 14f, 17.5f);
                        break;
                    case "chat":
                        g.DrawRectangle(p, 3f, 4f, 18f, 12f);
                        g.DrawLines(p, Pts(8, 16, 8, 21, 13, 16));
                        break;
                    case "plane":
                        g.DrawPolygon(p, Pts(3, 11, 21, 3, 14, 21, 11, 13));
                        g.DrawLine(p, 11f, 13f, 21f, 3f);
                        break;
                    case "arrow":
                        g.DrawLine(p, 5f, 12f, 19f, 12f);
                        g.DrawLines(p, Pts(13, 6, 19, 12, 13, 18));
                        break;
                    case "chevron":
                        g.DrawLines(p, Pts(9, 5, 16, 12, 9, 19));
                        break;
                }
            }
            g.Restore(st);
        }
    }

    internal class HlpPanel : Panel
    {
        public int Radius { get; set; }
        public Color FillTop { get; set; }
        public Color FillBottom { get; set; }
        public Color BorderColor { get; set; }
        public bool Horizontal { get; set; }

        public HlpPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);
            Radius = 20;
            FillTop = Color.White;
            FillBottom = Color.White;
            BorderColor = HlpTheme.Border;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width < 4 || Height < 4) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(HlpTheme.Bg);
            Rectangle rc = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = HlpTheme.Round(rc, Radius))
            {
                using (LinearGradientBrush br = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), FillTop, FillBottom, Horizontal ? 0f : 90f))
                {
                    g.FillPath(br, path);
                }
                GraphicsState st = g.Save();
                g.SetClip(path, CombineMode.Intersect);
                base.OnPaint(e);
                g.Restore(st);
                if (BorderColor.A > 0)
                {
                    using (Pen p = new Pen(BorderColor, 1f))
                    {
                        g.DrawPath(p, path);
                    }
                }
            }
        }
    }

    internal abstract class HlpControl : Control
    {
        protected bool Hot;
        protected bool Pressed;

        protected HlpControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw | ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, false);
            Cursor = Cursors.Hand;
        }

        public override Color BackColor
        {
            get { return Color.Transparent; }
            set { }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Hot = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Hot = false;
            Pressed = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Pressed = true;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Pressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            Invalidate();
            base.OnTextChanged(e);
        }

        protected void Prepare(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
        }
    }

    internal class HlpPill : HlpControl
    {
        private bool _outlined;

        public bool Outlined
        {
            get { return _outlined; }
            set { _outlined = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width < 8 || Height < 8) return;
            Graphics g = e.Graphics;
            Prepare(g);
            Rectangle rc = new Rectangle(0, 0, Width - 1, Height - 1);
            Color fg = Color.White;
            using (GraphicsPath path = HlpTheme.Round(rc, Height / 2))
            {
                if (!Enabled)
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(214, 210, 232)))
                    {
                        g.FillPath(b, path);
                    }
                }
                else if (_outlined)
                {
                    using (SolidBrush b = new SolidBrush(Hot ? HlpTheme.Soft : Color.White))
                    {
                        g.FillPath(b, path);
                    }
                    using (Pen p = new Pen(HlpTheme.Primary, 1.5f))
                    {
                        g.DrawPath(p, path);
                    }
                    fg = HlpTheme.Primary;
                }
                else
                {
                    Color c1 = Pressed || Hot ? HlpTheme.PrimaryDark : HlpTheme.Primary;
                    Color c2 = Pressed || Hot ? HlpTheme.Primary : HlpTheme.PrimaryLight;
                    using (LinearGradientBrush b = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), c1, c2, 0f))
                    {
                        g.FillPath(b, path);
                    }
                }
            }
            HlpTheme.DrawTextC(g, Text, HlpTheme.F("Segoe UI", 9.5f, FontStyle.Bold), fg, new RectangleF(8, 0, Width - 16, Height));
        }
    }

    internal class HlpNav : HlpControl
    {
        private bool _selected;
        private string _iconName = "home";

        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; Invalidate(); }
        }

        public string IconName
        {
            get { return _iconName; }
            set { _iconName = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width < 8 || Height < 8) return;
            Graphics g = e.Graphics;
            Prepare(g);
            Rectangle rc = new Rectangle(0, 0, Width - 1, Height - 1);
            Color fg = HlpTheme.Ink;
            Color ic = HlpTheme.Primary;
            using (GraphicsPath path = HlpTheme.Round(rc, 14))
            {
                if (_selected)
                {
                    using (LinearGradientBrush b = new LinearGradientBrush(new Rectangle(0, 0, Width, Height), HlpTheme.Primary, HlpTheme.PrimaryLight, 0f))
                    {
                        g.FillPath(b, path);
                    }
                    fg = Color.White;
                    ic = Color.White;
                }
                else if (Hot)
                {
                    using (SolidBrush b = new SolidBrush(HlpTheme.Soft))
                    {
                        g.FillPath(b, path);
                    }
                }
            }
            HlpTheme.DrawIcon(g, _iconName, new RectangleF(16, (Height - 22) / 2f, 22, 22), ic);
            HlpTheme.DrawText(g, Text, HlpTheme.F("Segoe UI", 10f, _selected ? FontStyle.Bold : FontStyle.Regular), fg,
                new RectangleF(50, 0, Width - 56, Height), StringAlignment.Near, StringAlignment.Center);
        }
    }

    internal class HlpCard : HlpControl
    {
        private string _iconName = "book";
        private string _title = "";
        private string _desc = "";

        public string IconName
        {
            get { return _iconName; }
            set { _iconName = value; Invalidate(); }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; Invalidate(); }
        }

        public string Desc
        {
            get { return _desc; }
            set { _desc = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width < 40 || Height < 40) return;
            Graphics g = e.Graphics;
            Prepare(g);
            Rectangle rc = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = HlpTheme.Round(rc, 16))
            {
                using (SolidBrush b = new SolidBrush(Hot ? Color.FromArgb(244, 240, 255) : Color.FromArgb(250, 248, 255)))
                {
                    g.FillPath(b, path);
                }
                using (Pen p = new Pen(Hot ? HlpTheme.PrimaryLight : HlpTheme.Border, Hot ? 1.5f : 1f))
                {
                    g.DrawPath(p, path);
                }
            }
            using (SolidBrush ib = new SolidBrush(HlpTheme.Soft))
            {
                g.FillEllipse(ib, 16, 16, 52, 52);
            }
            HlpTheme.DrawIcon(g, _iconName, new RectangleF(29, 29, 26, 26), HlpTheme.Primary);

            float tx = 82;
            HlpTheme.DrawText(g, _title, HlpTheme.F("Segoe UI", 11f, FontStyle.Bold), HlpTheme.Ink,
                new RectangleF(tx, 18, Width - tx - 14, 24));
            HlpTheme.DrawText(g, _desc, HlpTheme.F("Segoe UI", 8.75f, FontStyle.Regular), HlpTheme.Muted,
                new RectangleF(tx, 44, Width - tx - 50, Height - 56));

            Rectangle ar = new Rectangle(Width - 46, Height - 46, 30, 30);
            using (SolidBrush ab = new SolidBrush(Hot ? HlpTheme.Primary : HlpTheme.Soft))
            {
                g.FillEllipse(ab, ar);
            }
            HlpTheme.DrawIcon(g, "arrow", new RectangleF(ar.X + 8, ar.Y + 8, 14, 14), Hot ? Color.White : HlpTheme.Primary);
        }
    }

    internal class HlpLink : HlpControl
    {
        private string _iconName = "doc";

        public string IconName
        {
            get { return _iconName; }
            set { _iconName = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width < 40 || Height < 16) return;
            Graphics g = e.Graphics;
            Prepare(g);
            Rectangle rc = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = HlpTheme.Round(rc, 10))
            using (SolidBrush b = new SolidBrush(Hot ? HlpTheme.Soft : Color.FromArgb(248, 245, 255)))
            {
                g.FillPath(b, path);
            }
            HlpTheme.DrawIcon(g, _iconName, new RectangleF(12, (Height - 18) / 2f, 18, 18), HlpTheme.Primary);
            HlpTheme.DrawText(g, Text, HlpTheme.F("Segoe UI", 9.5f, FontStyle.Regular), HlpTheme.Ink,
                new RectangleF(42, 0, Width - 76, Height), StringAlignment.Near, StringAlignment.Center);
            HlpTheme.DrawIcon(g, "chevron", new RectangleF(Width - 26, (Height - 12) / 2f, 12, 12), HlpTheme.Muted);
        }
    }
}