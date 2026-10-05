namespace MissionPlanner.GCSViews
{
    partial class Help
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlHero = new MissionPlanner.GCSViews.HlpPanel();
            this.pnlNav = new MissionPlanner.GCSViews.HlpPanel();
            this.pnlMain = new MissionPlanner.GCSViews.HlpPanel();
            this.pnlSupport = new MissionPlanner.GCSViews.HlpPanel();
            this.pnlResources = new MissionPlanner.GCSViews.HlpPanel();
            this.pnlUpdates = new MissionPlanner.GCSViews.HlpPanel();
            this.pnlFooter = new MissionPlanner.GCSViews.HlpPanel();
            this.navHome = new MissionPlanner.GCSViews.HlpNav();
            this.navFaq = new MissionPlanner.GCSViews.HlpNav();
            this.navStart = new MissionPlanner.GCSViews.HlpNav();
            this.navGuide = new MissionPlanner.GCSViews.HlpNav();
            this.navTrouble = new MissionPlanner.GCSViews.HlpNav();
            this.navContact = new MissionPlanner.GCSViews.HlpNav();
            this.cardStart = new MissionPlanner.GCSViews.HlpCard();
            this.cardSetup = new MissionPlanner.GCSViews.HlpCard();
            this.cardSim = new MissionPlanner.GCSViews.HlpCard();
            this.cardConfig = new MissionPlanner.GCSViews.HlpCard();
            this.cardTrouble = new MissionPlanner.GCSViews.HlpCard();
            this.cardSupport = new MissionPlanner.GCSViews.HlpCard();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.BUT_contact = new MissionPlanner.GCSViews.HlpPill();
            this.linkManual = new MissionPlanner.GCSViews.HlpLink();
            this.linkVideos = new MissionPlanner.GCSViews.HlpLink();
            this.linkDownloads = new MissionPlanner.GCSViews.HlpLink();
            this.BUT_updatecheck = new MissionPlanner.GCSViews.HlpPill();
            this.BUT_betaupdate = new MissionPlanner.GCSViews.HlpPill();
            this.CHK_showconsole = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.pnlNav.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlSupport.SuspendLayout();
            this.pnlResources.SuspendLayout();
            this.pnlUpdates.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHero
            // 
            this.pnlHero.BorderColor = System.Drawing.Color.Transparent;
            this.pnlHero.FillBottom = System.Drawing.Color.FromArgb(241, 237, 255);
            this.pnlHero.FillTop = System.Drawing.Color.FromArgb(214, 201, 255);
            this.pnlHero.Horizontal = true;
            this.pnlHero.Location = new System.Drawing.Point(16, 16);
            this.pnlHero.Name = "pnlHero";
            this.pnlHero.Radius = 24;
            this.pnlHero.Size = new System.Drawing.Size(968, 190);
            this.pnlHero.TabIndex = 0;
            this.pnlHero.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHero_Paint);
            // 
            // pnlNav
            // 
            this.pnlNav.BorderColor = System.Drawing.Color.FromArgb(226, 219, 250);
            this.pnlNav.Controls.Add(this.navContact);
            this.pnlNav.Controls.Add(this.navTrouble);
            this.pnlNav.Controls.Add(this.navGuide);
            this.pnlNav.Controls.Add(this.navStart);
            this.pnlNav.Controls.Add(this.navFaq);
            this.pnlNav.Controls.Add(this.navHome);
            this.pnlNav.FillBottom = System.Drawing.Color.White;
            this.pnlNav.FillTop = System.Drawing.Color.White;
            this.pnlNav.Location = new System.Drawing.Point(16, 220);
            this.pnlNav.Name = "pnlNav";
            this.pnlNav.Radius = 20;
            this.pnlNav.Size = new System.Drawing.Size(220, 544);
            this.pnlNav.TabIndex = 1;
            this.pnlNav.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlNav_Paint);
            // 
            // navHome
            // 
            this.navHome.IconName = "home";
            this.navHome.Location = new System.Drawing.Point(14, 22);
            this.navHome.Name = "navHome";
            this.navHome.Size = new System.Drawing.Size(192, 44);
            this.navHome.TabIndex = 0;
            this.navHome.Tag = "home";
            this.navHome.Text = "Help Home";
            this.navHome.Click += new System.EventHandler(this.Nav_Click);
            // 
            // navFaq
            // 
            this.navFaq.IconName = "faq";
            this.navFaq.Location = new System.Drawing.Point(14, 74);
            this.navFaq.Name = "navFaq";
            this.navFaq.Size = new System.Drawing.Size(192, 44);
            this.navFaq.TabIndex = 1;
            this.navFaq.Tag = "faq";
            this.navFaq.Text = "FAQs";
            this.navFaq.Click += new System.EventHandler(this.Nav_Click);
            // 
            // navStart
            // 
            this.navStart.IconName = "cap";
            this.navStart.Location = new System.Drawing.Point(14, 126);
            this.navStart.Name = "navStart";
            this.navStart.Size = new System.Drawing.Size(192, 44);
            this.navStart.TabIndex = 2;
            this.navStart.Tag = "start";
            this.navStart.Text = "Getting Started";
            this.navStart.Click += new System.EventHandler(this.Nav_Click);
            // 
            // navGuide
            // 
            this.navGuide.IconName = "gear";
            this.navGuide.Location = new System.Drawing.Point(14, 178);
            this.navGuide.Name = "navGuide";
            this.navGuide.Size = new System.Drawing.Size(192, 44);
            this.navGuide.TabIndex = 3;
            this.navGuide.Tag = "guide";
            this.navGuide.Text = "System Guide";
            this.navGuide.Click += new System.EventHandler(this.Nav_Click);
            // 
            // navTrouble
            // 
            this.navTrouble.IconName = "wrench";
            this.navTrouble.Location = new System.Drawing.Point(14, 230);
            this.navTrouble.Name = "navTrouble";
            this.navTrouble.Size = new System.Drawing.Size(192, 44);
            this.navTrouble.TabIndex = 4;
            this.navTrouble.Tag = "trouble";
            this.navTrouble.Text = "Troubleshooting";
            this.navTrouble.Click += new System.EventHandler(this.Nav_Click);
            // 
            // navContact
            // 
            this.navContact.IconName = "mail";
            this.navContact.Location = new System.Drawing.Point(14, 282);
            this.navContact.Name = "navContact";
            this.navContact.Size = new System.Drawing.Size(192, 44);
            this.navContact.TabIndex = 5;
            this.navContact.Tag = "contact";
            this.navContact.Text = "Contact Us";
            this.navContact.Click += new System.EventHandler(this.Nav_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.BorderColor = System.Drawing.Color.FromArgb(226, 219, 250);
            this.pnlMain.Controls.Add(this.richTextBox1);
            this.pnlMain.Controls.Add(this.cardSupport);
            this.pnlMain.Controls.Add(this.cardTrouble);
            this.pnlMain.Controls.Add(this.cardConfig);
            this.pnlMain.Controls.Add(this.cardSim);
            this.pnlMain.Controls.Add(this.cardSetup);
            this.pnlMain.Controls.Add(this.cardStart);
            this.pnlMain.FillBottom = System.Drawing.Color.White;
            this.pnlMain.FillTop = System.Drawing.Color.White;
            this.pnlMain.Location = new System.Drawing.Point(250, 220);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Radius = 20;
            this.pnlMain.Size = new System.Drawing.Size(434, 544);
            this.pnlMain.TabIndex = 2;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            // 
            // cardStart
            // 
            this.cardStart.Desc = "Learn how to use the system step by step.";
            this.cardStart.IconName = "book";
            this.cardStart.Location = new System.Drawing.Point(22, 80);
            this.cardStart.Name = "cardStart";
            this.cardStart.Size = new System.Drawing.Size(120, 150);
            this.cardStart.TabIndex = 0;
            this.cardStart.Tag = "start";
            this.cardStart.Title = "Getting Started";
            this.cardStart.Click += new System.EventHandler(this.Card_Click);
            // 
            // cardSetup
            // 
            this.cardSetup.Desc = "Guidance for connecting, calibration and setup.";
            this.cardSetup.IconName = "drone";
            this.cardSetup.Location = new System.Drawing.Point(158, 80);
            this.cardSetup.Name = "cardSetup";
            this.cardSetup.Size = new System.Drawing.Size(120, 150);
            this.cardSetup.TabIndex = 1;
            this.cardSetup.Tag = "setup";
            this.cardSetup.Title = "Drone Setup";
            this.cardSetup.Click += new System.EventHandler(this.Card_Click);
            // 
            // cardSim
            // 
            this.cardSim.Desc = "Learn about simulation and practice modes.";
            this.cardSim.IconName = "game";
            this.cardSim.Location = new System.Drawing.Point(294, 80);
            this.cardSim.Name = "cardSim";
            this.cardSim.Size = new System.Drawing.Size(120, 150);
            this.cardSim.TabIndex = 2;
            this.cardSim.Tag = "sim";
            this.cardSim.Title = "Simulation";
            this.cardSim.Click += new System.EventHandler(this.Card_Click);
            // 
            // cardConfig
            // 
            this.cardConfig.Desc = "Settings, parameters and preferences.";
            this.cardConfig.IconName = "gear";
            this.cardConfig.Location = new System.Drawing.Point(22, 246);
            this.cardConfig.Name = "cardConfig";
            this.cardConfig.Size = new System.Drawing.Size(120, 150);
            this.cardConfig.TabIndex = 3;
            this.cardConfig.Tag = "config";
            this.cardConfig.Title = "Configuration";
            this.cardConfig.Click += new System.EventHandler(this.Card_Click);
            // 
            // cardTrouble
            // 
            this.cardTrouble.Desc = "Fix common issues and errors.";
            this.cardTrouble.IconName = "warn";
            this.cardTrouble.Location = new System.Drawing.Point(158, 246);
            this.cardTrouble.Name = "cardTrouble";
            this.cardTrouble.Size = new System.Drawing.Size(120, 150);
            this.cardTrouble.TabIndex = 4;
            this.cardTrouble.Tag = "trouble";
            this.cardTrouble.Title = "Troubleshooting";
            this.cardTrouble.Click += new System.EventHandler(this.Card_Click);
            // 
            // cardSupport
            // 
            this.cardSupport.Desc = "Still need help? Reach out to us.";
            this.cardSupport.IconName = "headset";
            this.cardSupport.Location = new System.Drawing.Point(294, 246);
            this.cardSupport.Name = "cardSupport";
            this.cardSupport.Size = new System.Drawing.Size(120, 150);
            this.cardSupport.TabIndex = 5;
            this.cardSupport.Tag = "support";
            this.cardSupport.Title = "Contact Support";
            this.cardSupport.Click += new System.EventHandler(this.Card_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.Default;
            this.richTextBox1.DetectUrls = true;
            this.richTextBox1.Location = new System.Drawing.Point(22, 80);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(390, 442);
            this.richTextBox1.TabIndex = 6;
            this.richTextBox1.Visible = false;
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // pnlSupport
            // 
            this.pnlSupport.BorderColor = System.Drawing.Color.FromArgb(226, 219, 250);
            this.pnlSupport.Controls.Add(this.BUT_contact);
            this.pnlSupport.FillBottom = System.Drawing.Color.FromArgb(247, 243, 255);
            this.pnlSupport.FillTop = System.Drawing.Color.FromArgb(230, 223, 255);
            this.pnlSupport.Location = new System.Drawing.Point(698, 220);
            this.pnlSupport.Name = "pnlSupport";
            this.pnlSupport.Radius = 20;
            this.pnlSupport.Size = new System.Drawing.Size(300, 160);
            this.pnlSupport.TabIndex = 3;
            this.pnlSupport.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlSupport_Paint);
            // 
            // BUT_contact
            // 
            this.BUT_contact.Location = new System.Drawing.Point(18, 108);
            this.BUT_contact.Name = "BUT_contact";
            this.BUT_contact.Size = new System.Drawing.Size(264, 36);
            this.BUT_contact.TabIndex = 0;
            this.BUT_contact.Text = "Contact Support  \u2192";
            this.BUT_contact.Click += new System.EventHandler(this.BUT_contact_Click);
            // 
            // pnlResources
            // 
            this.pnlResources.BorderColor = System.Drawing.Color.FromArgb(226, 219, 250);
            this.pnlResources.Controls.Add(this.linkDownloads);
            this.pnlResources.Controls.Add(this.linkVideos);
            this.pnlResources.Controls.Add(this.linkManual);
            this.pnlResources.FillBottom = System.Drawing.Color.White;
            this.pnlResources.FillTop = System.Drawing.Color.White;
            this.pnlResources.Location = new System.Drawing.Point(698, 394);
            this.pnlResources.Name = "pnlResources";
            this.pnlResources.Radius = 20;
            this.pnlResources.Size = new System.Drawing.Size(300, 156);
            this.pnlResources.TabIndex = 4;
            this.pnlResources.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlResources_Paint);
            // 
            // linkManual
            // 
            this.linkManual.IconName = "doc";
            this.linkManual.Location = new System.Drawing.Point(10, 48);
            this.linkManual.Name = "linkManual";
            this.linkManual.Size = new System.Drawing.Size(280, 32);
            this.linkManual.TabIndex = 0;
            this.linkManual.Tag = "manual";
            this.linkManual.Text = "User Manual";
            this.linkManual.Click += new System.EventHandler(this.Link_Click);
            // 
            // linkVideos
            // 
            this.linkVideos.IconName = "video";
            this.linkVideos.Location = new System.Drawing.Point(10, 84);
            this.linkVideos.Name = "linkVideos";
            this.linkVideos.Size = new System.Drawing.Size(280, 32);
            this.linkVideos.TabIndex = 1;
            this.linkVideos.Tag = "video";
            this.linkVideos.Text = "Video Tutorials";
            this.linkVideos.Click += new System.EventHandler(this.Link_Click);
            // 
            // linkDownloads
            // 
            this.linkDownloads.IconName = "download";
            this.linkDownloads.Location = new System.Drawing.Point(10, 120);
            this.linkDownloads.Name = "linkDownloads";
            this.linkDownloads.Size = new System.Drawing.Size(280, 32);
            this.linkDownloads.TabIndex = 2;
            this.linkDownloads.Tag = "download";
            this.linkDownloads.Text = "Download Center";
            this.linkDownloads.Click += new System.EventHandler(this.Link_Click);
            // 
            // pnlUpdates
            // 
            this.pnlUpdates.BorderColor = System.Drawing.Color.FromArgb(226, 219, 250);
            this.pnlUpdates.Controls.Add(this.linkLabel1);
            this.pnlUpdates.Controls.Add(this.CHK_showconsole);
            this.pnlUpdates.Controls.Add(this.BUT_betaupdate);
            this.pnlUpdates.Controls.Add(this.BUT_updatecheck);
            this.pnlUpdates.FillBottom = System.Drawing.Color.White;
            this.pnlUpdates.FillTop = System.Drawing.Color.White;
            this.pnlUpdates.Location = new System.Drawing.Point(698, 564);
            this.pnlUpdates.Name = "pnlUpdates";
            this.pnlUpdates.Radius = 20;
            this.pnlUpdates.Size = new System.Drawing.Size(300, 200);
            this.pnlUpdates.TabIndex = 5;
            this.pnlUpdates.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlUpdates_Paint);
            // 
            // BUT_updatecheck
            // 
            this.BUT_updatecheck.Location = new System.Drawing.Point(16, 52);
            this.BUT_updatecheck.Name = "BUT_updatecheck";
            this.BUT_updatecheck.Size = new System.Drawing.Size(268, 36);
            this.BUT_updatecheck.TabIndex = 0;
            this.BUT_updatecheck.Text = "Check for Updates";
            this.BUT_updatecheck.Click += new System.EventHandler(this.BUT_updatecheck_Click);
            // 
            // BUT_betaupdate
            // 
            this.BUT_betaupdate.Location = new System.Drawing.Point(16, 96);
            this.BUT_betaupdate.Name = "BUT_betaupdate";
            this.BUT_betaupdate.Outlined = true;
            this.BUT_betaupdate.Size = new System.Drawing.Size(268, 36);
            this.BUT_betaupdate.TabIndex = 1;
            this.BUT_betaupdate.Text = "Check for BETA Updates";
            this.BUT_betaupdate.Click += new System.EventHandler(this.BUT_betaupdate_Click);
            // 
            // CHK_showconsole
            // 
            this.CHK_showconsole.AutoSize = true;
            this.CHK_showconsole.BackColor = System.Drawing.Color.Transparent;
            this.CHK_showconsole.Location = new System.Drawing.Point(16, 144);
            this.CHK_showconsole.Name = "CHK_showconsole";
            this.CHK_showconsole.Size = new System.Drawing.Size(220, 20);
            this.CHK_showconsole.TabIndex = 2;
            this.CHK_showconsole.Text = "Show Console Window (restart)";
            this.CHK_showconsole.UseVisualStyleBackColor = false;
            this.CHK_showconsole.CheckedChanged += new System.EventHandler(this.CHK_showconsole_CheckedChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Location = new System.Drawing.Point(18, 174);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(70, 16);
            this.linkLabel1.TabIndex = 3;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Change Log";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // pnlFooter
            // 
            this.pnlFooter.BorderColor = System.Drawing.Color.Transparent;
            this.pnlFooter.FillBottom = System.Drawing.Color.FromArgb(243, 240, 255);
            this.pnlFooter.FillTop = System.Drawing.Color.FromArgb(243, 240, 255);
            this.pnlFooter.Location = new System.Drawing.Point(0, 778);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Radius = 0;
            this.pnlFooter.Size = new System.Drawing.Size(1000, 54);
            this.pnlFooter.TabIndex = 6;
            this.pnlFooter.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlFooter_Paint);
            // 
            // Help
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(243, 240, 255);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlUpdates);
            this.Controls.Add(this.pnlResources);
            this.Controls.Add(this.pnlSupport);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlNav);
            this.Controls.Add(this.pnlHero);
            this.Name = "Help";
            this.Size = new System.Drawing.Size(1000, 832);
            this.Load += new System.EventHandler(this.Help_Load);
            this.pnlNav.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlSupport.ResumeLayout(false);
            this.pnlResources.ResumeLayout(false);
            this.pnlUpdates.ResumeLayout(false);
            this.pnlUpdates.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HlpPanel pnlHero;
        private HlpPanel pnlNav;
        private HlpPanel pnlMain;
        private HlpPanel pnlSupport;
        private HlpPanel pnlResources;
        private HlpPanel pnlUpdates;
        private HlpPanel pnlFooter;
        private HlpNav navHome;
        private HlpNav navFaq;
        private HlpNav navStart;
        private HlpNav navGuide;
        private HlpNav navTrouble;
        private HlpNav navContact;
        private HlpCard cardStart;
        private HlpCard cardSetup;
        private HlpCard cardSim;
        private HlpCard cardConfig;
        private HlpCard cardTrouble;
        private HlpCard cardSupport;
        private HlpPill BUT_contact;
        private HlpLink linkManual;
        private HlpLink linkVideos;
        private HlpLink linkDownloads;
        private HlpPill BUT_updatecheck;
        private HlpPill BUT_betaupdate;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox CHK_showconsole;
        private System.Windows.Forms.LinkLabel linkLabel1;

    }
}