using System;
using System.Drawing;
using System.Windows.Forms;
using MissionPlanner.Controls;

namespace MissionPlanner.GCSViews
{
    // Chennai Drone Academy themed SITL page.
    // Control names and event handlers are unchanged, so SITL.cs works as-is
    // (Activate() calls ApplyCdaTheme() instead of ThemeManager.ApplyThemeTo).
    partial class SITL
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Themed layout

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // ---------- controls used by SITL.cs (names must not change) ----------
            this.myGMAP1 = new MissionPlanner.Controls.myGMAP();
            this.pictureBoxplane = new CdaTile();
            this.pictureBoxrover = new CdaTile();
            this.pictureBoxquad = new CdaTile();
            this.pictureBoxheli = new CdaTile();
            this.NUM_heading = new System.Windows.Forms.NumericUpDown();
            this.cmb_version = new System.Windows.Forms.ComboBox();
            this.num_simspeed = new System.Windows.Forms.NumericUpDown();
            this.cmb_model = new System.Windows.Forms.ComboBox();
            this.txt_cmdline = new System.Windows.Forms.TextBox();
            this.chk_wipe = new System.Windows.Forms.CheckBox();
            this.but_swarmseq = new CdaPillButton();
            this.but_swarmlink = new CdaPillButton();
            this.but_swarmplane = new CdaPillButton();
            this.but_swarmrover = new CdaPillButton();

            ((System.ComponentModel.ISupportInitialize)(this.NUM_heading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_simspeed)).BeginInit();

            // ---------- map ----------
            this.myGMAP1.Bearing = 0F;
            this.myGMAP1.CanDragMap = true;
            this.myGMAP1.Dock = DockStyle.Fill;
            this.myGMAP1.EmptyTileColor = Color.FromArgb(224, 220, 244);
            this.myGMAP1.GrayScaleMode = false;
            this.myGMAP1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.myGMAP1.HoldInvalidation = false;
            this.myGMAP1.LevelsKeepInMemmory = 5;
            this.myGMAP1.MarkersEnabled = true;
            this.myGMAP1.MaxZoom = 2;
            this.myGMAP1.MinZoom = 2;
            this.myGMAP1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionWithoutCenter;
            this.myGMAP1.Name = "myGMAP1";
            this.myGMAP1.NegativeMode = false;
            this.myGMAP1.PolygonsEnabled = true;
            this.myGMAP1.RetryLoadTile = 0;
            this.myGMAP1.RoutesEnabled = true;
            this.myGMAP1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.myGMAP1.SelectedAreaFillColor = Color.FromArgb(33, 65, 105, 225);
            this.myGMAP1.ShowTileGridLines = false;
            this.myGMAP1.Zoom = 0D;
            this.myGMAP1.OnMarkerEnter += new GMap.NET.WindowsForms.MarkerEnter(this.myGMAP1_OnMarkerEnter);
            this.myGMAP1.OnMarkerLeave += new GMap.NET.WindowsForms.MarkerLeave(this.myGMAP1_OnMarkerLeave);
            this.myGMAP1.MouseDown += new MouseEventHandler(this.myGMAP1_MouseDown);
            this.myGMAP1.MouseMove += new MouseEventHandler(this.myGMAP1_MouseMove);
            this.myGMAP1.MouseUp += new MouseEventHandler(this.myGMAP1_MouseUp);
            this.myGMAP1.MouseMove += (s, e) => { if (mousedown) UpdateLocationChip(); };
            this.myGMAP1.MouseUp += (s, e) => UpdateLocationChip();

            // ---------- overlays on the map ----------
            this.cdaChip = new CdaLocationChip { Location = new Point(14, 12) };
            this.cdaToggle = new CdaSegmented { Size = new Size(124, 28) };
            this.cdaToggle.SelectionChanged += (s, e) =>
            {
                myGMAP1.MapProvider = cdaToggle.RightSelected
                    ? (GMap.NET.MapProviders.GMapProvider)GMap.NET.MapProviders.GoogleSatelliteMapProvider.Instance
                    : GMap.NET.MapProviders.GoogleMapProvider.Instance;
            };
            this.cdaZoomIn = new CdaRoundButton { Glyph = "+" };
            this.cdaZoomOut = new CdaRoundButton { Glyph = "\u2212" };
            this.cdaLocate = new CdaRoundButton { Glyph = "\u25CE" };
            this.cdaZoomIn.Click += (s, e) => { if (myGMAP1.Zoom < myGMAP1.MaxZoom) myGMAP1.Zoom += 1; };
            this.cdaZoomOut.Click += (s, e) => { if (myGMAP1.Zoom > myGMAP1.MinZoom) myGMAP1.Zoom -= 1; };
            this.cdaLocate.Click += (s, e) => { myGMAP1.Position = homemarker.Position; };
            this.myGMAP1.Controls.AddRange(new Control[] { cdaChip, cdaToggle, cdaZoomIn, cdaZoomOut, cdaLocate });
            this.myGMAP1.Resize += (s, e) =>
            {
                cdaToggle.Location = new Point(myGMAP1.Width - cdaToggle.Width - 14, 12);
                int x = myGMAP1.Width - 32 - 14;
                cdaLocate.Location = new Point(x, myGMAP1.Height - 32 - 14);
                cdaZoomOut.Location = new Point(x, cdaLocate.Top - 40);
                cdaZoomIn.Location = new Point(x, cdaZoomOut.Top - 40);
            };

            var mapCard = new CdaCard { Dock = DockStyle.Fill, Padding = new Padding(5), Radius = 14 };
            mapCard.Controls.Add(this.myGMAP1);

            // ---------- breadcrumb ----------
            var crumb = new CdaBreadcrumb { Dock = DockStyle.Fill };

            // ---------- Options card ----------
            StyleNumeric(this.NUM_heading, 0, 360, 0);
            this.NUM_heading.Name = "NUM_heading";
            StyleNumeric(this.num_simspeed, 1, 100, 1);
            this.num_simspeed.Name = "num_simspeed";
            StyleCombo(this.cmb_version);
            this.cmb_version.Name = "cmb_version";
            this.cmb_version.DropDownStyle = ComboBoxStyle.DropDownList;

            var optionsCard = new CdaCard { Dock = DockStyle.Fill, Padding = new Padding(16, 10, 16, 10) };
            var opt = new CdaTable { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 3 };
            opt.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 54));
            opt.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 84));
            opt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            opt.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            opt.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            opt.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            var optTitle = SectionTitle("\u2699  Options");
            opt.Controls.Add(optTitle, 0, 0);
            opt.SetColumnSpan(optTitle, 3);
            opt.Controls.Add(FieldLabel("Heading"), 0, 1);
            opt.Controls.Add(new CdaField(this.NUM_heading) { Dock = DockStyle.Fill }, 1, 1);
            opt.Controls.Add(Pad(new CdaField(this.cmb_version), 10, 0, 0, 0), 2, 1);
            optionsCard.Controls.Add(opt);

            // ---------- Advanced card ----------
            StyleCombo(this.cmb_model);
            this.cmb_model.Name = "cmb_model";
            this.cmb_model.Items.AddRange(new object[]
            {
                "+", "X", "quad", "hexa", "octa", "octa-quad", "deca", "dodeca-hexa", "tri", "y6",
                "heli", "heli-dual", "heli-compound", "singlecopter", "coaxcopter",
                "plane", "plane-elevon", "plane-vtail", "plane-tailsitter", "quadplane", "firefly",
                "rover", "rover-skid", "sailboat", "balloon", "tracker",
                "jsbsim", "flightaxis", "gazebo", "last_letter", "crrcsim", "xplane", "airsim", "calibration"
            });
            this.cmb_model.Text = "";

            this.txt_cmdline.Name = "txt_cmdline";
            this.txt_cmdline.BorderStyle = BorderStyle.None;
            this.txt_cmdline.Font = CdaTheme.Ui(8.5f);
            CdaTheme.Guard(this.txt_cmdline, CdaTheme.Field, CdaTheme.Text);
            CdaTheme.SetCue(this.txt_cmdline, "(Optional)");

            this.chk_wipe.Name = "chk_wipe";
            this.chk_wipe.Text = "Wipe";
            this.chk_wipe.AutoSize = true;
            this.chk_wipe.Font = CdaTheme.Ui(8f, FontStyle.Bold);
            this.chk_wipe.UseVisualStyleBackColor = false;
            CdaTheme.Guard(this.chk_wipe, Color.Transparent, CdaTheme.Text);

            ConfigureSwarm(this.but_swarmseq, "but_swarmseq", "Copter Swarm", "Single link", CdaIcons.Kind.Quad, true);
            ConfigureSwarm(this.but_swarmlink, "but_swarmlink", "Copter Swarm", "Multi link", CdaIcons.Kind.Quad, false);
            ConfigureSwarm(this.but_swarmplane, "but_swarmplane", "Plane Swarm", "Multi link", CdaIcons.Kind.Plane, false);
            ConfigureSwarm(this.but_swarmrover, "but_swarmrover", "Rover Swarm", "Multi link", CdaIcons.Kind.Car, false);
            this.but_swarmrover.Click += new EventHandler(this.but_swarmrover_Click);
            this.but_swarmplane.Click += new EventHandler(this.but_swarmplane_Click);
            this.but_swarmseq.Click += new EventHandler(this.but_swarmseq_Click);
            this.but_swarmlink.Click += new EventHandler(this.but_swarmlink_Click);

            var advCard = new CdaCard { Dock = DockStyle.Fill, Padding = new Padding(16, 8, 16, 8) };
            var adv = new CdaTable { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4 };
            adv.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            adv.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24));
            adv.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42));
            adv.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16));
            adv.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            adv.RowStyles.Add(new RowStyle(SizeType.Absolute, 16));
            adv.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            adv.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var advTitle = SectionTitle("\u2261  Advanced users only");
            adv.Controls.Add(advTitle, 0, 0);
            adv.SetColumnSpan(advTitle, 4);
            adv.Controls.Add(FieldLabel("Sim Speed"), 0, 1);
            adv.Controls.Add(FieldLabel("Model"), 1, 1);
            adv.Controls.Add(FieldLabel("Extra command line"), 2, 1);
            adv.Controls.Add(Pad(new CdaField(this.num_simspeed), 0, 0, 10, 0), 0, 2);
            adv.Controls.Add(Pad(new CdaField(this.cmb_model), 0, 0, 10, 0), 1, 2);
            adv.Controls.Add(Pad(new CdaField(this.txt_cmdline), 0, 0, 10, 0), 2, 2);
            var wipeHost = new CdaSurface { Dock = DockStyle.Fill };
            this.chk_wipe.Location = new Point(0, 8);
            wipeHost.Controls.Add(this.chk_wipe);
            adv.Controls.Add(wipeHost, 3, 2);

            var swarm = new CdaTable { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1 };
            for (int i = 0; i < 4; i++) swarm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
            swarm.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            swarm.Controls.Add(Pad(this.but_swarmseq, 0, 6, 8, 0), 0, 0);
            swarm.Controls.Add(Pad(this.but_swarmlink, 0, 6, 8, 0), 1, 0);
            swarm.Controls.Add(Pad(this.but_swarmplane, 0, 6, 8, 0), 2, 0);
            swarm.Controls.Add(Pad(this.but_swarmrover, 0, 6, 0, 0), 3, 0);
            adv.Controls.Add(swarm, 0, 3);
            adv.SetColumnSpan(swarm, 4);
            advCard.Controls.Add(adv);

            var optionsRow = new CdaTable { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            optionsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340));
            optionsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            optionsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            optionsRow.Controls.Add(Pad(optionsCard, 0, 0, 8, 0), 0, 0);
            optionsRow.Controls.Add(advCard, 1, 0);

            // ---------- firmware tiles ----------
            ConfigureTile(this.pictureBoxplane, "pictureBoxplane", "plane", "Plane", CdaIcons.Kind.Plane);
            ConfigureTile(this.pictureBoxrover, "pictureBoxrover", "rover", "Rover", CdaIcons.Kind.Car);
            ConfigureTile(this.pictureBoxquad, "pictureBoxquad", "copter", "Multirotor", CdaIcons.Kind.Quad);
            ConfigureTile(this.pictureBoxheli, "pictureBoxheli", "heli", "Helicopter", CdaIcons.Kind.Heli);
            this.pictureBoxplane.Click += new EventHandler(this.pictureBoxplane_Click);
            this.pictureBoxrover.Click += new EventHandler(this.pictureBoxrover_Click);
            this.pictureBoxquad.Click += new EventHandler(this.pictureBoxquad_Click);
            this.pictureBoxheli.Click += new EventHandler(this.pictureBoxheli_Click);

            var fwCard = new CdaCard
            {
                Dock = DockStyle.Fill,
                Gradient = true,
                Top = Color.FromArgb(250, 248, 255),
                Bottom = Color.FromArgb(226, 220, 250),
                Padding = new Padding(16, 8, 16, 12)
            };
            var fw = new CdaTable { Dock = DockStyle.Fill, ColumnCount = 6, RowCount = 2 };
            fw.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (int i = 0; i < 4; i++) fw.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
            fw.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            fw.RowStyles.Add(new RowStyle(SizeType.Absolute, 26));
            fw.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            var fwTitle = SectionTitle("\u2708  Please select a firmware to run");
            fw.Controls.Add(fwTitle, 0, 0);
            fw.SetColumnSpan(fwTitle, 6);
            fw.Controls.Add(Pad(this.pictureBoxplane, 4, 0, 4, 0), 1, 1);
            fw.Controls.Add(Pad(this.pictureBoxrover, 4, 0, 4, 0), 2, 1);
            fw.Controls.Add(Pad(this.pictureBoxquad, 4, 0, 4, 0), 3, 1);
            fw.Controls.Add(Pad(this.pictureBoxheli, 4, 0, 4, 0), 4, 1);
            fwCard.Controls.Add(fw);

            // ---------- page ----------
            var page = new CdaTable
            {
                Opaque = true,
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(14, 8, 14, 10)
            };
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            page.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 172));
            page.RowStyles.Add(new RowStyle(SizeType.Absolute, 140));
            page.Controls.Add(crumb, 0, 0);
            page.Controls.Add(Pad(mapCard, 0, 8, 0, 8), 0, 1);
            page.Controls.Add(Pad(optionsRow, 0, 0, 0, 8), 0, 2);
            page.Controls.Add(fwCard, 0, 3);

            this.Controls.Add(page);
            this.Name = "SITL";
            this.Size = new Size(1000, 700);

            ((System.ComponentModel.ISupportInitialize)(this.NUM_heading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_simspeed)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        // ======================================================================
        // Hand-written helpers
        // ======================================================================

        private CdaLocationChip cdaChip;
        private CdaSegmented cdaToggle;
        private CdaRoundButton cdaZoomIn, cdaZoomOut, cdaLocate;

        private void UpdateLocationChip()
        {
            if (cdaChip == null || homemarker == null) return;
            cdaChip.SetLocation(homemarker.Position.Lat, homemarker.Position.Lng);
        }

        /// <summary>Called from Activate() in place of ThemeManager.ApplyThemeTo(this).</summary>
        private void ApplyCdaTheme()
        {
            foreach (var n in new[] { NUM_heading, num_simspeed })
                CdaTheme.GuardDeep(n, CdaTheme.Field, CdaTheme.Text);
            foreach (var c in new[] { cmb_version, cmb_model })
            {
                c.FlatStyle = FlatStyle.Flat;
                CdaTheme.Guard(c, CdaTheme.Field, CdaTheme.Text);
            }
            UpdateLocationChip();

            // re-assert the map view once layout has settled
            if (IsHandleCreated)
                BeginInvoke(new Action(() =>
                {
                    try
                    {
                        myGMAP1.Position = homemarker.Position;
                        myGMAP1.Zoom = 16;
                        myGMAP1.Invalidate();
                    }
                    catch { }
                }));
        }

        private static Control Pad(Control c, int left, int top, int right, int bottom)
        {
            var host = new CdaSurface { Dock = DockStyle.Fill, Padding = new Padding(left, top, right, bottom) };
            c.Dock = DockStyle.Fill;
            host.Controls.Add(c);
            return host;
        }

        private static Control SectionTitle(string text)
        {
            return new CdaLabel { Text = text, Dock = DockStyle.Fill, Font = CdaTheme.Ui(9f, FontStyle.Bold), TextColor = CdaTheme.Text };
        }

        private static Control FieldLabel(string text)
        {
            return new CdaLabel { Text = text, Dock = DockStyle.Fill, Font = CdaTheme.Ui(7.5f, FontStyle.Bold), TextColor = CdaTheme.TextMuted };
        }

        private static void StyleNumeric(NumericUpDown n, int min, int max, int value)
        {
            n.Minimum = min;
            n.Maximum = max;
            n.Value = value;
            n.Font = CdaTheme.Ui(8.5f);
            n.BorderStyle = BorderStyle.None;
            CdaTheme.GuardDeep(n, CdaTheme.Field, CdaTheme.Text);
        }

        private static void StyleCombo(ComboBox c)
        {
            c.FormattingEnabled = true;
            c.FlatStyle = FlatStyle.Flat;
            c.Font = CdaTheme.Ui(8.5f);
            CdaTheme.Guard(c, CdaTheme.Field, CdaTheme.Text);
        }

        private static void ConfigureSwarm(CdaPillButton b, string name, string l1, string l2, CdaIcons.Kind icon, bool selected)
        {
            b.Name = name;
            b.Line1 = l1;
            b.Line2 = l2;
            b.Glyph = CdaIcons.Make(icon, 64, selected ? Color.White : CdaTheme.Primary);
            b.Selected = selected;
            b.Text = "";
        }

        private static void ConfigureTile(CdaTile t, string name, string tag, string caption, CdaIcons.Kind icon)
        {
            t.Name = name;
            t.Tag = tag;
            t.Caption = caption;
            t.Picture = CdaIcons.Make(icon, 96, CdaTheme.Primary);
        }

        private myGMAP myGMAP1;
        private CdaTile pictureBoxheli;
        private CdaTile pictureBoxquad;
        private CdaTile pictureBoxrover;
        private CdaTile pictureBoxplane;
        private System.Windows.Forms.NumericUpDown NUM_heading;
        private System.Windows.Forms.ComboBox cmb_version;
        private System.Windows.Forms.NumericUpDown num_simspeed;
        private System.Windows.Forms.ComboBox cmb_model;
        private System.Windows.Forms.TextBox txt_cmdline;
        private System.Windows.Forms.CheckBox chk_wipe;
        private CdaPillButton but_swarmseq;
        private CdaPillButton but_swarmlink;
        private CdaPillButton but_swarmrover;
        private CdaPillButton but_swarmplane;
    }
}