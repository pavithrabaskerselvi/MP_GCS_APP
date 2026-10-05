using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MissionPlanner.Controls;
using MissionPlanner.GCSViews;

namespace MissionPlanner.GCSViews.ConfigurationView
{
    partial class ConfigPlanner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigPlanner));
            this.label33 = new System.Windows.Forms.Label();
            this.CMB_ratesensors = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.CMB_videoresolutions = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.CHK_GDIPlus = new CdaCheckBox();
            this.label24 = new System.Windows.Forms.Label();
            this.CHK_loadwponconnect = new CdaCheckBox();
            this.label23 = new System.Windows.Forms.Label();
            this.NUM_tracklength = new System.Windows.Forms.NumericUpDown();
            this.CHK_speechaltwarning = new CdaCheckBox();
            this.label108 = new System.Windows.Forms.Label();
            this.CHK_resetapmonconnect = new CdaCheckBox();
            this.CHK_mavdebug = new CdaCheckBox();
            this.label107 = new System.Windows.Forms.Label();
            this.CMB_raterc = new System.Windows.Forms.ComboBox();
            this.label104 = new System.Windows.Forms.Label();
            this.label103 = new System.Windows.Forms.Label();
            this.label102 = new System.Windows.Forms.Label();
            this.label101 = new System.Windows.Forms.Label();
            this.CMB_ratestatus = new System.Windows.Forms.ComboBox();
            this.CMB_rateposition = new System.Windows.Forms.ComboBox();
            this.CMB_rateattitude = new System.Windows.Forms.ComboBox();
            this.label99 = new System.Windows.Forms.Label();
            this.label98 = new System.Windows.Forms.Label();
            this.label97 = new System.Windows.Forms.Label();
            this.CMB_speedunits = new System.Windows.Forms.ComboBox();
            this.CMB_distunits = new System.Windows.Forms.ComboBox();
            this.label96 = new System.Windows.Forms.Label();
            this.label95 = new System.Windows.Forms.Label();
            this.CHK_speechbattery = new CdaCheckBox();
            this.CHK_speechcustom = new CdaCheckBox();
            this.CHK_speechmode = new CdaCheckBox();
            this.CHK_speechwaypoint = new CdaCheckBox();
            this.label94 = new System.Windows.Forms.Label();
            this.CMB_osdcolor = new System.Windows.Forms.ComboBox();
            this.CMB_severity = new System.Windows.Forms.ComboBox();
            this.CMB_language = new System.Windows.Forms.ComboBox();
            this.label93 = new System.Windows.Forms.Label();
            this.CHK_enablespeech = new CdaCheckBox();
            this.CHK_hudshow = new CdaCheckBox();
            this.label92 = new System.Windows.Forms.Label();
            this.CMB_videosources = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CHK_maprotation = new CdaCheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CHK_disttohomeflightdata = new CdaCheckBox();
            this.BUT_Joystick = new MissionPlanner.Controls.MyButton();
            this.BUT_videostop = new MissionPlanner.Controls.MyButton();
            this.BUT_videostart = new MissionPlanner.Controls.MyButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_log_dir = new System.Windows.Forms.TextBox();
            this.BUT_logdirbrowse = new MissionPlanner.Controls.MyButton();
            this.label4 = new System.Windows.Forms.Label();
            this.CMB_theme = new System.Windows.Forms.ComboBox();
            this.BUT_themecustom = new MissionPlanner.Controls.MyButton();
            this.CHK_speecharmdisarm = new CdaCheckBox();
            this.BUT_Vario = new MissionPlanner.Controls.MyButton();
            this.chk_analytics = new CdaCheckBox();
            this.CHK_beta = new CdaCheckBox();
            this.CHK_Password = new CdaCheckBox();
            this.CHK_speechlowspeed = new CdaCheckBox();
            this.CHK_showairports = new CdaCheckBox();
            this.chk_ADSB = new CdaCheckBox();
            this.chk_tfr = new CdaCheckBox();
            this.chk_temp = new CdaCheckBox();
            this.chk_norcreceiver = new CdaCheckBox();
            this.CMB_Layout = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CHK_AutoParamCommit = new CdaCheckBox();
            this.chk_shownofly = new CdaCheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CMB_altunits = new System.Windows.Forms.ComboBox();
            this.num_gcsid = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.CHK_params_bg = new CdaCheckBox();
            this.chk_slowMachine = new CdaCheckBox();
            this.CHK_speechArmedOnly = new CdaCheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmb_secondarydisplaystyle = new System.Windows.Forms.ComboBox();
            this.chk_displaycog = new CdaCheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chk_displayheading = new CdaCheckBox();
            this.chk_displaynavbearing = new CdaCheckBox();
            this.chk_displayradius = new CdaCheckBox();
            this.chk_displaytarget = new CdaCheckBox();
            this.num_linelength = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.chk_displaytooltip = new CdaCheckBox();
            this.CMB_mapCache = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.BUT_mapCacheDir = new MissionPlanner.Controls.MyButton();
            this.CHK_rtsresetesp32 = new CdaCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUM_tracklength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_gcsid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_linelength)).BeginInit();
            this.SuspendLayout();
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // CMB_ratesensors
            // 
            this.CMB_ratesensors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_ratesensors.FormattingEnabled = true;
            this.CMB_ratesensors.Items.AddRange(new object[] {
            resources.GetString("CMB_ratesensors.Items"),
            resources.GetString("CMB_ratesensors.Items1"),
            resources.GetString("CMB_ratesensors.Items2"),
            resources.GetString("CMB_ratesensors.Items3"),
            resources.GetString("CMB_ratesensors.Items4"),
            resources.GetString("CMB_ratesensors.Items5"),
            resources.GetString("CMB_ratesensors.Items6"),
            resources.GetString("CMB_ratesensors.Items7"),
            resources.GetString("CMB_ratesensors.Items8"),
            resources.GetString("CMB_ratesensors.Items9"),
            resources.GetString("CMB_ratesensors.Items10"),
            resources.GetString("CMB_ratesensors.Items11"),
            resources.GetString("CMB_ratesensors.Items12"),
            resources.GetString("CMB_ratesensors.Items13"),
            resources.GetString("CMB_ratesensors.Items14")});
            resources.ApplyResources(this.CMB_ratesensors, "CMB_ratesensors");
            this.CMB_ratesensors.Name = "CMB_ratesensors";
            this.CMB_ratesensors.SelectedIndexChanged += new System.EventHandler(this.CMB_ratesensors_SelectedIndexChanged);
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // CMB_videoresolutions
            // 
            this.CMB_videoresolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_videoresolutions.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_videoresolutions, "CMB_videoresolutions");
            this.CMB_videoresolutions.Name = "CMB_videoresolutions";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // CHK_GDIPlus
            // 
            resources.ApplyResources(this.CHK_GDIPlus, "CHK_GDIPlus");
            this.CHK_GDIPlus.Name = "CHK_GDIPlus";
            this.CHK_GDIPlus.UseVisualStyleBackColor = true;
            this.CHK_GDIPlus.CheckedChanged += new System.EventHandler(this.CHK_GDIPlus_CheckedChanged);
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // CHK_loadwponconnect
            // 
            resources.ApplyResources(this.CHK_loadwponconnect, "CHK_loadwponconnect");
            this.CHK_loadwponconnect.Name = "CHK_loadwponconnect";
            this.CHK_loadwponconnect.UseVisualStyleBackColor = true;
            this.CHK_loadwponconnect.CheckedChanged += new System.EventHandler(this.CHK_loadwponconnect_CheckedChanged);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // NUM_tracklength
            // 
            this.NUM_tracklength.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            resources.ApplyResources(this.NUM_tracklength, "NUM_tracklength");
            this.NUM_tracklength.Maximum = new decimal(new int[] {
            200000,
            0,
            0,
            0});
            this.NUM_tracklength.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.NUM_tracklength.Name = "NUM_tracklength";
            this.NUM_tracklength.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.NUM_tracklength.ValueChanged += new System.EventHandler(this.NUM_tracklength_ValueChanged);
            // 
            // CHK_speechaltwarning
            // 
            resources.ApplyResources(this.CHK_speechaltwarning, "CHK_speechaltwarning");
            this.CHK_speechaltwarning.Name = "CHK_speechaltwarning";
            this.CHK_speechaltwarning.UseVisualStyleBackColor = true;
            this.CHK_speechaltwarning.CheckedChanged += new System.EventHandler(this.CHK_speechaltwarning_CheckedChanged);
            // 
            // label108
            // 
            resources.ApplyResources(this.label108, "label108");
            this.label108.Name = "label108";
            // 
            // CHK_resetapmonconnect
            // 
            resources.ApplyResources(this.CHK_resetapmonconnect, "CHK_resetapmonconnect");
            this.CHK_resetapmonconnect.Name = "CHK_resetapmonconnect";
            this.CHK_resetapmonconnect.UseVisualStyleBackColor = true;
            this.CHK_resetapmonconnect.CheckedChanged += new System.EventHandler(this.CHK_resetapmonconnect_CheckedChanged);
            // 
            // CHK_mavdebug
            // 
            resources.ApplyResources(this.CHK_mavdebug, "CHK_mavdebug");
            this.CHK_mavdebug.Name = "CHK_mavdebug";
            this.CHK_mavdebug.UseVisualStyleBackColor = true;
            this.CHK_mavdebug.CheckedChanged += new System.EventHandler(this.CHK_mavdebug_CheckedChanged);
            // 
            // label107
            // 
            resources.ApplyResources(this.label107, "label107");
            this.label107.Name = "label107";
            // 
            // CMB_raterc
            // 
            this.CMB_raterc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_raterc.FormattingEnabled = true;
            this.CMB_raterc.Items.AddRange(new object[] {
            resources.GetString("CMB_raterc.Items"),
            resources.GetString("CMB_raterc.Items1"),
            resources.GetString("CMB_raterc.Items2"),
            resources.GetString("CMB_raterc.Items3"),
            resources.GetString("CMB_raterc.Items4"),
            resources.GetString("CMB_raterc.Items5"),
            resources.GetString("CMB_raterc.Items6"),
            resources.GetString("CMB_raterc.Items7"),
            resources.GetString("CMB_raterc.Items8"),
            resources.GetString("CMB_raterc.Items9"),
            resources.GetString("CMB_raterc.Items10"),
            resources.GetString("CMB_raterc.Items11"),
            resources.GetString("CMB_raterc.Items12"),
            resources.GetString("CMB_raterc.Items13"),
            resources.GetString("CMB_raterc.Items14")});
            resources.ApplyResources(this.CMB_raterc, "CMB_raterc");
            this.CMB_raterc.Name = "CMB_raterc";
            this.CMB_raterc.SelectedIndexChanged += new System.EventHandler(this.CMB_raterc_SelectedIndexChanged);
            // 
            // label104
            // 
            resources.ApplyResources(this.label104, "label104");
            this.label104.Name = "label104";
            // 
            // label103
            // 
            resources.ApplyResources(this.label103, "label103");
            this.label103.Name = "label103";
            // 
            // label102
            // 
            resources.ApplyResources(this.label102, "label102");
            this.label102.Name = "label102";
            // 
            // label101
            // 
            resources.ApplyResources(this.label101, "label101");
            this.label101.Name = "label101";
            // 
            // CMB_ratestatus
            // 
            this.CMB_ratestatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_ratestatus.FormattingEnabled = true;
            this.CMB_ratestatus.Items.AddRange(new object[] {
            resources.GetString("CMB_ratestatus.Items"),
            resources.GetString("CMB_ratestatus.Items1"),
            resources.GetString("CMB_ratestatus.Items2"),
            resources.GetString("CMB_ratestatus.Items3"),
            resources.GetString("CMB_ratestatus.Items4"),
            resources.GetString("CMB_ratestatus.Items5"),
            resources.GetString("CMB_ratestatus.Items6"),
            resources.GetString("CMB_ratestatus.Items7"),
            resources.GetString("CMB_ratestatus.Items8"),
            resources.GetString("CMB_ratestatus.Items9"),
            resources.GetString("CMB_ratestatus.Items10"),
            resources.GetString("CMB_ratestatus.Items11"),
            resources.GetString("CMB_ratestatus.Items12"),
            resources.GetString("CMB_ratestatus.Items13"),
            resources.GetString("CMB_ratestatus.Items14")});
            resources.ApplyResources(this.CMB_ratestatus, "CMB_ratestatus");
            this.CMB_ratestatus.Name = "CMB_ratestatus";
            this.CMB_ratestatus.SelectedIndexChanged += new System.EventHandler(this.CMB_ratestatus_SelectedIndexChanged);
            // 
            // CMB_rateposition
            // 
            this.CMB_rateposition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_rateposition.FormattingEnabled = true;
            this.CMB_rateposition.Items.AddRange(new object[] {
            resources.GetString("CMB_rateposition.Items"),
            resources.GetString("CMB_rateposition.Items1"),
            resources.GetString("CMB_rateposition.Items2"),
            resources.GetString("CMB_rateposition.Items3"),
            resources.GetString("CMB_rateposition.Items4"),
            resources.GetString("CMB_rateposition.Items5"),
            resources.GetString("CMB_rateposition.Items6"),
            resources.GetString("CMB_rateposition.Items7"),
            resources.GetString("CMB_rateposition.Items8"),
            resources.GetString("CMB_rateposition.Items9"),
            resources.GetString("CMB_rateposition.Items10"),
            resources.GetString("CMB_rateposition.Items11"),
            resources.GetString("CMB_rateposition.Items12"),
            resources.GetString("CMB_rateposition.Items13"),
            resources.GetString("CMB_rateposition.Items14")});
            resources.ApplyResources(this.CMB_rateposition, "CMB_rateposition");
            this.CMB_rateposition.Name = "CMB_rateposition";
            this.CMB_rateposition.SelectedIndexChanged += new System.EventHandler(this.CMB_rateposition_SelectedIndexChanged);
            // 
            // CMB_rateattitude
            // 
            this.CMB_rateattitude.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_rateattitude.FormattingEnabled = true;
            this.CMB_rateattitude.Items.AddRange(new object[] {
            resources.GetString("CMB_rateattitude.Items"),
            resources.GetString("CMB_rateattitude.Items1"),
            resources.GetString("CMB_rateattitude.Items2"),
            resources.GetString("CMB_rateattitude.Items3"),
            resources.GetString("CMB_rateattitude.Items4"),
            resources.GetString("CMB_rateattitude.Items5"),
            resources.GetString("CMB_rateattitude.Items6"),
            resources.GetString("CMB_rateattitude.Items7"),
            resources.GetString("CMB_rateattitude.Items8"),
            resources.GetString("CMB_rateattitude.Items9"),
            resources.GetString("CMB_rateattitude.Items10"),
            resources.GetString("CMB_rateattitude.Items11"),
            resources.GetString("CMB_rateattitude.Items12"),
            resources.GetString("CMB_rateattitude.Items13")});
            resources.ApplyResources(this.CMB_rateattitude, "CMB_rateattitude");
            this.CMB_rateattitude.Name = "CMB_rateattitude";
            this.CMB_rateattitude.SelectedIndexChanged += new System.EventHandler(this.CMB_rateattitude_SelectedIndexChanged);
            // 
            // label99
            // 
            resources.ApplyResources(this.label99, "label99");
            this.label99.Name = "label99";
            // 
            // label98
            // 
            resources.ApplyResources(this.label98, "label98");
            this.label98.Name = "label98";
            // 
            // label97
            // 
            resources.ApplyResources(this.label97, "label97");
            this.label97.Name = "label97";
            // 
            // CMB_speedunits
            // 
            this.CMB_speedunits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_speedunits.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_speedunits, "CMB_speedunits");
            this.CMB_speedunits.Name = "CMB_speedunits";
            this.CMB_speedunits.SelectedIndexChanged += new System.EventHandler(this.CMB_speedunits_SelectedIndexChanged);
            // 
            // CMB_distunits
            // 
            this.CMB_distunits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_distunits.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_distunits, "CMB_distunits");
            this.CMB_distunits.Name = "CMB_distunits";
            this.CMB_distunits.SelectedIndexChanged += new System.EventHandler(this.CMB_distunits_SelectedIndexChanged);
            // 
            // label96
            // 
            resources.ApplyResources(this.label96, "label96");
            this.label96.Name = "label96";
            // 
            // label95
            // 
            resources.ApplyResources(this.label95, "label95");
            this.label95.Name = "label95";
            // 
            // CHK_speechbattery
            // 
            resources.ApplyResources(this.CHK_speechbattery, "CHK_speechbattery");
            this.CHK_speechbattery.Name = "CHK_speechbattery";
            this.CHK_speechbattery.UseVisualStyleBackColor = true;
            this.CHK_speechbattery.CheckedChanged += new System.EventHandler(this.CHK_speechbattery_CheckedChanged);
            // 
            // CHK_speechcustom
            // 
            resources.ApplyResources(this.CHK_speechcustom, "CHK_speechcustom");
            this.CHK_speechcustom.Name = "CHK_speechcustom";
            this.CHK_speechcustom.UseVisualStyleBackColor = true;
            this.CHK_speechcustom.CheckedChanged += new System.EventHandler(this.CHK_speechcustom_CheckedChanged);
            // 
            // CHK_speechmode
            // 
            resources.ApplyResources(this.CHK_speechmode, "CHK_speechmode");
            this.CHK_speechmode.Name = "CHK_speechmode";
            this.CHK_speechmode.UseVisualStyleBackColor = true;
            this.CHK_speechmode.CheckedChanged += new System.EventHandler(this.CHK_speechmode_CheckedChanged);
            // 
            // CHK_speechwaypoint
            // 
            resources.ApplyResources(this.CHK_speechwaypoint, "CHK_speechwaypoint");
            this.CHK_speechwaypoint.Name = "CHK_speechwaypoint";
            this.CHK_speechwaypoint.UseVisualStyleBackColor = true;
            this.CHK_speechwaypoint.CheckedChanged += new System.EventHandler(this.CHK_speechwaypoint_CheckedChanged);
            // 
            // label94
            // 
            resources.ApplyResources(this.label94, "label94");
            this.label94.Name = "label94";
            // 
            // CMB_osdcolor
            // 
            this.CMB_osdcolor.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CMB_osdcolor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_osdcolor.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_osdcolor, "CMB_osdcolor");
            this.CMB_osdcolor.Name = "CMB_osdcolor";
            this.CMB_osdcolor.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.CMB_osdcolor_DrawItem);
            this.CMB_osdcolor.SelectedIndexChanged += new System.EventHandler(this.CMB_osdcolor_SelectedIndexChanged);
            // 
            // CMB_severity
            // 
            this.CMB_severity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_severity.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_severity, "CMB_severity");
            this.CMB_severity.Name = "CMB_severity";
            this.CMB_severity.SelectedIndexChanged += new System.EventHandler(this.CMB_severity_SelectedIndexChanged);
            // 
            // CMB_language
            // 
            this.CMB_language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_language.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_language, "CMB_language");
            this.CMB_language.Name = "CMB_language";
            this.CMB_language.SelectedIndexChanged += new System.EventHandler(this.CMB_language_SelectedIndexChanged);
            // 
            // label93
            // 
            resources.ApplyResources(this.label93, "label93");
            this.label93.Name = "label93";
            // 
            // CHK_enablespeech
            // 
            resources.ApplyResources(this.CHK_enablespeech, "CHK_enablespeech");
            this.CHK_enablespeech.Name = "CHK_enablespeech";
            this.CHK_enablespeech.UseVisualStyleBackColor = true;
            this.CHK_enablespeech.CheckedChanged += new System.EventHandler(this.CHK_enablespeech_CheckedChanged);
            // 
            // CHK_hudshow
            // 
            this.CHK_hudshow.Checked = true;
            this.CHK_hudshow.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.CHK_hudshow, "CHK_hudshow");
            this.CHK_hudshow.Name = "CHK_hudshow";
            this.CHK_hudshow.UseVisualStyleBackColor = true;
            this.CHK_hudshow.CheckedChanged += new System.EventHandler(this.CHK_hudshow_CheckedChanged);
            // 
            // label92
            // 
            resources.ApplyResources(this.label92, "label92");
            this.label92.Name = "label92";
            // 
            // CMB_videosources
            // 
            this.CMB_videosources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_videosources.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_videosources, "CMB_videosources");
            this.CMB_videosources.Name = "CMB_videosources";
            this.CMB_videosources.SelectedIndexChanged += new System.EventHandler(this.CMB_videosources_SelectedIndexChanged);
            this.CMB_videosources.Click += new System.EventHandler(this.CMB_videosources_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CHK_maprotation
            // 
            resources.ApplyResources(this.CHK_maprotation, "CHK_maprotation");
            this.CHK_maprotation.Name = "CHK_maprotation";
            this.CHK_maprotation.UseVisualStyleBackColor = true;
            this.CHK_maprotation.CheckedChanged += new System.EventHandler(this.CHK_maprotation_CheckedChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // CHK_disttohomeflightdata
            // 
            this.CHK_disttohomeflightdata.Checked = true;
            this.CHK_disttohomeflightdata.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.CHK_disttohomeflightdata, "CHK_disttohomeflightdata");
            this.CHK_disttohomeflightdata.Name = "CHK_disttohomeflightdata";
            this.CHK_disttohomeflightdata.UseVisualStyleBackColor = true;
            this.CHK_disttohomeflightdata.CheckedChanged += new System.EventHandler(this.CHK_disttohomeflightdata_CheckedChanged);
            // 
            // BUT_Joystick
            // 
            resources.ApplyResources(this.BUT_Joystick, "BUT_Joystick");
            this.BUT_Joystick.Name = "BUT_Joystick";
            this.BUT_Joystick.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_Joystick.UseVisualStyleBackColor = true;
            this.BUT_Joystick.Click += new System.EventHandler(this.BUT_Joystick_Click);
            // 
            // BUT_videostop
            // 
            resources.ApplyResources(this.BUT_videostop, "BUT_videostop");
            this.BUT_videostop.Name = "BUT_videostop";
            this.BUT_videostop.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_videostop.UseVisualStyleBackColor = true;
            this.BUT_videostop.Click += new System.EventHandler(this.BUT_videostop_Click);
            // 
            // BUT_videostart
            // 
            resources.ApplyResources(this.BUT_videostart, "BUT_videostart");
            this.BUT_videostart.Name = "BUT_videostart";
            this.BUT_videostart.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_videostart.UseVisualStyleBackColor = true;
            this.BUT_videostart.Click += new System.EventHandler(this.BUT_videostart_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txt_log_dir
            // 
            resources.ApplyResources(this.txt_log_dir, "txt_log_dir");
            this.txt_log_dir.Name = "txt_log_dir";
            // 
            // BUT_logdirbrowse
            // 
            resources.ApplyResources(this.BUT_logdirbrowse, "BUT_logdirbrowse");
            this.BUT_logdirbrowse.Name = "BUT_logdirbrowse";
            this.BUT_logdirbrowse.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_logdirbrowse.UseVisualStyleBackColor = true;
            this.BUT_logdirbrowse.Click += new System.EventHandler(this.BUT_logdirbrowse_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // CMB_theme
            // 
            this.CMB_theme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_theme.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_theme, "CMB_theme");
            this.CMB_theme.Name = "CMB_theme";
            this.CMB_theme.SelectedIndexChanged += new System.EventHandler(this.CMB_theme_SelectedIndexChanged);
            // 
            // BUT_themecustom
            // 
            resources.ApplyResources(this.BUT_themecustom, "BUT_themecustom");
            this.BUT_themecustom.Name = "BUT_themecustom";
            this.BUT_themecustom.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_themecustom.UseVisualStyleBackColor = true;
            this.BUT_themecustom.Click += new System.EventHandler(this.BUT_themecustom_Click);
            // 
            // CHK_speecharmdisarm
            // 
            resources.ApplyResources(this.CHK_speecharmdisarm, "CHK_speecharmdisarm");
            this.CHK_speecharmdisarm.Name = "CHK_speecharmdisarm";
            this.CHK_speecharmdisarm.UseVisualStyleBackColor = true;
            this.CHK_speecharmdisarm.CheckedChanged += new System.EventHandler(this.CHK_speecharmdisarm_CheckedChanged);
            // 
            // BUT_Vario
            // 
            resources.ApplyResources(this.BUT_Vario, "BUT_Vario");
            this.BUT_Vario.Name = "BUT_Vario";
            this.BUT_Vario.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_Vario.UseVisualStyleBackColor = true;
            this.BUT_Vario.Click += new System.EventHandler(this.BUT_Vario_Click);
            // 
            // chk_analytics
            // 
            resources.ApplyResources(this.chk_analytics, "chk_analytics");
            this.chk_analytics.Name = "chk_analytics";
            this.chk_analytics.UseVisualStyleBackColor = true;
            this.chk_analytics.CheckedChanged += new System.EventHandler(this.chk_analytics_CheckedChanged);
            // 
            // CHK_beta
            // 
            resources.ApplyResources(this.CHK_beta, "CHK_beta");
            this.CHK_beta.Name = "CHK_beta";
            this.CHK_beta.UseVisualStyleBackColor = true;
            this.CHK_beta.CheckedChanged += new System.EventHandler(this.CHK_beta_CheckedChanged);
            // 
            // CHK_Password
            // 
            resources.ApplyResources(this.CHK_Password, "CHK_Password");
            this.CHK_Password.Name = "CHK_Password";
            this.CHK_Password.UseVisualStyleBackColor = true;
            this.CHK_Password.CheckedChanged += new System.EventHandler(this.CHK_Password_CheckedChanged);
            // 
            // CHK_speechlowspeed
            // 
            resources.ApplyResources(this.CHK_speechlowspeed, "CHK_speechlowspeed");
            this.CHK_speechlowspeed.Name = "CHK_speechlowspeed";
            this.CHK_speechlowspeed.UseVisualStyleBackColor = true;
            this.CHK_speechlowspeed.CheckedChanged += new System.EventHandler(this.CHK_speechlowspeed_CheckedChanged);
            // 
            // CHK_showairports
            // 
            resources.ApplyResources(this.CHK_showairports, "CHK_showairports");
            this.CHK_showairports.Checked = true;
            this.CHK_showairports.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_showairports.Name = "CHK_showairports";
            this.CHK_showairports.UseVisualStyleBackColor = true;
            this.CHK_showairports.CheckedChanged += new System.EventHandler(this.CHK_showairports_CheckedChanged);
            // 
            // chk_ADSB
            // 
            resources.ApplyResources(this.chk_ADSB, "chk_ADSB");
            this.chk_ADSB.Name = "chk_ADSB";
            this.chk_ADSB.UseVisualStyleBackColor = true;
            this.chk_ADSB.CheckedChanged += new System.EventHandler(this.chk_ADSB_CheckedChanged);
            // 
            // chk_tfr
            // 
            resources.ApplyResources(this.chk_tfr, "chk_tfr");
            this.chk_tfr.Checked = true;
            this.chk_tfr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_tfr.Name = "chk_tfr";
            this.chk_tfr.UseVisualStyleBackColor = true;
            this.chk_tfr.CheckedChanged += new System.EventHandler(this.chk_tfr_CheckedChanged);
            // 
            // chk_temp
            // 
            resources.ApplyResources(this.chk_temp, "chk_temp");
            this.chk_temp.Name = "chk_temp";
            this.chk_temp.UseVisualStyleBackColor = true;
            this.chk_temp.CheckedChanged += new System.EventHandler(this.chk_temp_CheckedChanged);
            // 
            // chk_norcreceiver
            // 
            resources.ApplyResources(this.chk_norcreceiver, "chk_norcreceiver");
            this.chk_norcreceiver.Name = "chk_norcreceiver";
            this.chk_norcreceiver.UseVisualStyleBackColor = true;
            this.chk_norcreceiver.CheckedChanged += new System.EventHandler(this.chk_norcreceiver_CheckedChanged);
            // 
            // CMB_Layout
            // 
            this.CMB_Layout.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_Layout.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_Layout, "CMB_Layout");
            this.CMB_Layout.Name = "CMB_Layout";
            this.CMB_Layout.SelectedIndexChanged += new System.EventHandler(this.CMB_Layout_SelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // CHK_AutoParamCommit
            // 
            resources.ApplyResources(this.CHK_AutoParamCommit, "CHK_AutoParamCommit");
            this.CHK_AutoParamCommit.Checked = true;
            this.CHK_AutoParamCommit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CHK_AutoParamCommit.Name = "CHK_AutoParamCommit";
            this.CHK_AutoParamCommit.UseVisualStyleBackColor = true;
            this.CHK_AutoParamCommit.CheckedChanged += new System.EventHandler(this.CHK_AutoParamCommit_CheckedChanged);
            // 
            // chk_shownofly
            // 
            resources.ApplyResources(this.chk_shownofly, "chk_shownofly");
            this.chk_shownofly.Checked = true;
            this.chk_shownofly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_shownofly.Name = "chk_shownofly";
            this.chk_shownofly.UseVisualStyleBackColor = true;
            this.chk_shownofly.CheckedChanged += new System.EventHandler(this.chk_shownofly_CheckedChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // CMB_altunits
            // 
            this.CMB_altunits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_altunits.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_altunits, "CMB_altunits");
            this.CMB_altunits.Name = "CMB_altunits";
            this.CMB_altunits.SelectedIndexChanged += new System.EventHandler(this.CMB_altunits_SelectedIndexChanged);
            // 
            // num_gcsid
            // 
            resources.ApplyResources(this.num_gcsid, "num_gcsid");
            this.num_gcsid.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.num_gcsid.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.num_gcsid.Name = "num_gcsid";
            this.num_gcsid.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.num_gcsid.ValueChanged += new System.EventHandler(this.num_gcsid_ValueChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // CHK_params_bg
            // 
            resources.ApplyResources(this.CHK_params_bg, "CHK_params_bg");
            this.CHK_params_bg.Name = "CHK_params_bg";
            this.CHK_params_bg.UseVisualStyleBackColor = true;
            this.CHK_params_bg.CheckedChanged += new System.EventHandler(this.CHK_params_bg_CheckedChanged);
            // 
            // chk_slowMachine
            // 
            resources.ApplyResources(this.chk_slowMachine, "chk_slowMachine");
            this.chk_slowMachine.Name = "chk_slowMachine";
            this.chk_slowMachine.UseVisualStyleBackColor = true;
            this.chk_slowMachine.CheckedChanged += new System.EventHandler(this.chk_slowMachine_CheckedChanged);
            // 
            // CHK_speechArmedOnly
            // 
            resources.ApplyResources(this.CHK_speechArmedOnly, "CHK_speechArmedOnly");
            this.CHK_speechArmedOnly.Name = "CHK_speechArmedOnly";
            this.CHK_speechArmedOnly.UseVisualStyleBackColor = true;
            this.CHK_speechArmedOnly.CheckedChanged += new System.EventHandler(this.CHK_speechArmedOnly_CheckedChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // cmb_secondarydisplaystyle
            // 
            this.cmb_secondarydisplaystyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_secondarydisplaystyle.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_secondarydisplaystyle, "cmb_secondarydisplaystyle");
            this.cmb_secondarydisplaystyle.Name = "cmb_secondarydisplaystyle";
            this.cmb_secondarydisplaystyle.SelectedIndexChanged += new System.EventHandler(this.cmb_secondarydisplaystyle_SelectedIndexChanged);
            // 
            // chk_displaycog
            // 
            resources.ApplyResources(this.chk_displaycog, "chk_displaycog");
            this.chk_displaycog.Name = "chk_displaycog";
            this.chk_displaycog.UseVisualStyleBackColor = true;
            this.chk_displaycog.CheckedChanged += new System.EventHandler(this.chk_displaycog_CheckedChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // chk_displayheading
            // 
            resources.ApplyResources(this.chk_displayheading, "chk_displayheading");
            this.chk_displayheading.Name = "chk_displayheading";
            this.chk_displayheading.UseVisualStyleBackColor = true;
            this.chk_displayheading.CheckedChanged += new System.EventHandler(this.chk_displayheading_CheckedChanged);
            // 
            // chk_displaynavbearing
            // 
            resources.ApplyResources(this.chk_displaynavbearing, "chk_displaynavbearing");
            this.chk_displaynavbearing.Name = "chk_displaynavbearing";
            this.chk_displaynavbearing.UseVisualStyleBackColor = true;
            this.chk_displaynavbearing.CheckedChanged += new System.EventHandler(this.chk_displaynavbearing_CheckedChanged);
            // 
            // chk_displayradius
            // 
            resources.ApplyResources(this.chk_displayradius, "chk_displayradius");
            this.chk_displayradius.Name = "chk_displayradius";
            this.chk_displayradius.UseVisualStyleBackColor = true;
            this.chk_displayradius.CheckedChanged += new System.EventHandler(this.chk_displayradius_CheckedChanged);
            // 
            // chk_displaytarget
            // 
            resources.ApplyResources(this.chk_displaytarget, "chk_displaytarget");
            this.chk_displaytarget.Name = "chk_displaytarget";
            this.chk_displaytarget.UseVisualStyleBackColor = true;
            this.chk_displaytarget.CheckedChanged += new System.EventHandler(this.chk_displaytarget_CheckedChanged);
            // 
            // num_linelength
            // 
            this.num_linelength.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            resources.ApplyResources(this.num_linelength, "num_linelength");
            this.num_linelength.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.num_linelength.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.num_linelength.Name = "num_linelength";
            this.num_linelength.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.num_linelength.ValueChanged += new System.EventHandler(this.num_linelength_ValueChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // chk_displaytooltip
            // 
            resources.ApplyResources(this.chk_displaytooltip, "chk_displaytooltip");
            this.chk_displaytooltip.Name = "chk_displaytooltip";
            this.chk_displaytooltip.UseVisualStyleBackColor = true;
            this.chk_displaytooltip.CheckedChanged += new System.EventHandler(this.chk_displaytooltip_CheckedChanged);
            // 
            // CMB_mapCache
            // 
            this.CMB_mapCache.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CMB_mapCache.FormattingEnabled = true;
            resources.ApplyResources(this.CMB_mapCache, "CMB_mapCache");
            this.CMB_mapCache.Name = "CMB_mapCache";
            this.CMB_mapCache.SelectedIndexChanged += new System.EventHandler(this.CMB_mapCache_SelectedIndexChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // BUT_mapCacheDir
            // 
            resources.ApplyResources(this.BUT_mapCacheDir, "BUT_mapCacheDir");
            this.BUT_mapCacheDir.Name = "BUT_mapCacheDir";
            this.BUT_mapCacheDir.TextColorNotEnabled = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(87)))), ((int)(((byte)(4)))));
            this.BUT_mapCacheDir.UseVisualStyleBackColor = true;
            this.BUT_mapCacheDir.Click += new System.EventHandler(this.BUT_mapCacheDir_Click);
            // 
            // CHK_rtsresetesp32
            // 
            resources.ApplyResources(this.CHK_rtsresetesp32, "CHK_rtsresetesp32");
            this.CHK_rtsresetesp32.Name = "CHK_rtsresetesp32";
            this.CHK_rtsresetesp32.UseVisualStyleBackColor = true;
            this.CHK_rtsresetesp32.CheckedChanged += new System.EventHandler(this.CHK_rtsresetesp32_CheckedChanged);
            // 
            // ConfigPlanner
            // 
            resources.ApplyResources(this, "$this");
            this.Name = "ConfigPlanner";
            this.DoubleBuffered = true;
            this.BuildCdaLayout();
            this.Load += new System.EventHandler(this.ConfigPlanner_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NUM_tracklength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_gcsid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_linelength)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.ComboBox CMB_ratesensors;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.ComboBox CMB_videoresolutions;
        private System.Windows.Forms.Label label12;
        private CdaCheckBox CHK_GDIPlus;
        private System.Windows.Forms.Label label24;
        private CdaCheckBox CHK_loadwponconnect;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown NUM_tracklength;
        private CdaCheckBox CHK_speechaltwarning;
        private System.Windows.Forms.Label label108;
        private CdaCheckBox CHK_resetapmonconnect;
        private CdaCheckBox CHK_mavdebug;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.ComboBox CMB_raterc;
        private System.Windows.Forms.Label label104;
        private System.Windows.Forms.Label label103;
        private System.Windows.Forms.Label label102;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.ComboBox CMB_ratestatus;
        private System.Windows.Forms.ComboBox CMB_rateposition;
        private System.Windows.Forms.ComboBox CMB_rateattitude;
        private System.Windows.Forms.Label label99;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.Label label97;
        private System.Windows.Forms.ComboBox CMB_speedunits;
        private System.Windows.Forms.ComboBox CMB_distunits;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Label label95;
        private CdaCheckBox CHK_speechbattery;
        private CdaCheckBox CHK_speechcustom;
        private CdaCheckBox CHK_speechmode;
        private CdaCheckBox CHK_speechwaypoint;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.ComboBox CMB_osdcolor;
        private System.Windows.Forms.ComboBox CMB_severity;
        private System.Windows.Forms.ComboBox CMB_language;
        private System.Windows.Forms.Label label93;
        private CdaCheckBox CHK_enablespeech;
        private CdaCheckBox CHK_hudshow;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.ComboBox CMB_videosources;
        private Controls.MyButton BUT_Joystick;
        private Controls.MyButton BUT_videostop;
        private Controls.MyButton BUT_videostart;
        private System.Windows.Forms.Label label1;
        private CdaCheckBox CHK_maprotation;
        private System.Windows.Forms.Label label2;
        private CdaCheckBox CHK_disttohomeflightdata;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_log_dir;
        private Controls.MyButton BUT_logdirbrowse;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CMB_theme;
        private Controls.MyButton BUT_themecustom;
        private CdaCheckBox CHK_speecharmdisarm;
        private Controls.MyButton BUT_Vario;
        private CdaCheckBox chk_analytics;
        private CdaCheckBox CHK_beta;
        private CdaCheckBox CHK_Password;
        private CdaCheckBox CHK_speechlowspeed;
        private CdaCheckBox CHK_showairports;
        private CdaCheckBox chk_ADSB;
        private CdaCheckBox chk_tfr;
        private CdaCheckBox chk_temp;
        private CdaCheckBox chk_norcreceiver;
        public System.Windows.Forms.ComboBox CMB_Layout;
        private System.Windows.Forms.Label label5;
        private CdaCheckBox CHK_AutoParamCommit;
        private CdaCheckBox chk_shownofly;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CMB_altunits;
        private System.Windows.Forms.NumericUpDown num_gcsid;
        private System.Windows.Forms.Label label7;
        private CdaCheckBox CHK_params_bg;
        private CdaCheckBox chk_slowMachine;
        private CdaCheckBox CHK_speechArmedOnly;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.ComboBox cmb_secondarydisplaystyle;
        private CdaCheckBox chk_displaycog;
        private System.Windows.Forms.Label label10;
        private CdaCheckBox chk_displayheading;
        private CdaCheckBox chk_displaynavbearing;
        private CdaCheckBox chk_displayradius;
        private CdaCheckBox chk_displaytarget;
        private System.Windows.Forms.NumericUpDown num_linelength;
        private System.Windows.Forms.Label label11;
        private CdaCheckBox chk_displaytooltip;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.ComboBox CMB_mapCache;
        private Controls.MyButton BUT_mapCacheDir;
        private CdaCheckBox CHK_rtsresetesp32;

        // =====================================================================
        //  Chennai Drone Academy layout – banner + rounded section cards
        // =====================================================================
        #region CDA layout

        private const int RowH = 42;      // label + input rows
        private const int CheckH = 32;    // one check-box line
        private const int GridPad = 4;

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (!Program.MONO)
                    cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED: flicker/ghost-free child painting
                return cp;
            }
        }

        private readonly List<Control> cdaCards = new List<Control>();
        private CardBuilder cdaSpeech;
        private Control cdaSpeechGrid;
        private CdaBanner cdaBanner;
        private bool cdaPositioning;

        private sealed class CardBuilder
        {
            public readonly CdaPanelCard Card;
            private readonly List<Control> rows = new List<Control>();
            private int height = 52 + 14;

            public CardBuilder(string title)
            {
                Card = new CdaPanelCard { Title = title, Padding = new Padding(18, 52, 18, 14), Radius = 14 };
            }

            public void Add(Control row, int h)
            {
                row.Dock = DockStyle.Top;
                row.Height = h;
                rows.Add(row);
                height += h;
            }

            public void Recalc()
            {
                int h = 52 + 14;
                foreach (var r in rows) if (r.Visible) h += r.Height;
                Card.Height = h;
            }

            public CdaPanelCard Done()
            {
                for (int i = rows.Count - 1; i >= 0; i--)   // last added docks first = top
                    Card.Controls.Add(rows[i]);
                Card.Height = height;
                return Card;
            }
        }

        private static int TextW(Control c)
        {
            return Math.Max(76, TextRenderer.MeasureText(c.Text, CdaTheme.Ui(8.75f, FontStyle.Bold)).Width + 36);
        }

        private static Control Field(Control inner)
        {
            inner.Font = CdaTheme.Ui(9f);
            if (inner is TextBox) ((TextBox)inner).BorderStyle = BorderStyle.None;
            if (inner is NumericUpDown) ((NumericUpDown)inner).BorderStyle = BorderStyle.None;
            int ih = inner is TextBox ? 17 : 23;
            var f = new CdaField(inner) { Margin = new Padding(0, 4, 0, 4) };
            f.Padding = new Padding(10, Math.Max(2, (34 - ih) / 2), 6, 0);
            // when the page hides the inner control (e.g. Layout), hide its rounded holder too
            inner.VisibleChanged += (s, e) => { if (!inner.Visible && f.Visible) f.Visible = false; };
            return f;
        }

        private static void PrepLabel(Label l, string tag = null)
        {
            l.AutoSize = false;
            l.Dock = DockStyle.Fill;
            l.Margin = new Padding(0);
            l.TextAlign = ContentAlignment.MiddleLeft;
            l.AutoEllipsis = true;
            if (tag != null) l.Tag = tag;
        }

        private static void PrepCheck(Control c)
        {
            c.AutoSize = false;
            c.Dock = DockStyle.Fill;
            c.Margin = new Padding(0, 2, 6, 2);
        }

        /// <summary>[label][main (fixed width or fill)][trailing buttons...]</summary>
        private static Control MakeRow(Control label, Control main, int mainWidth, params Control[] trailing)
        {
            int cols = 2 + (mainWidth > 0 ? 1 : 0) + trailing.Length;
            var t = new CdaRowTable { ColumnCount = cols, RowCount = 1 };
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 132f));

            int col = 0;
            if (label != null)
            {
                if (label is Label) PrepLabel((Label)label);
                t.Controls.Add(label, col, 0);
            }
            col++;

            if (main is Label) PrepLabel((Label)main);
            else if (main is CdaCheckBox) PrepCheck(main);
            else main.Dock = DockStyle.Fill;

            if (mainWidth > 0)
            {
                t.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, mainWidth));
                t.Controls.Add(main, col++, 0);
                t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                col++;
            }
            else
            {
                t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
                t.Controls.Add(main, col++, 0);
            }

            if (main is Button) main.Margin = new Padding(0, 4, 0, 4);

            foreach (var tr in trailing)
            {
                int w = tr is Button ? TextW(tr) + 10 : 120;
                t.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, w));
                tr.Dock = DockStyle.Fill;
                tr.Margin = new Padding(10, 4, 0, 4);
                t.Controls.Add(tr, col++, 0);
            }
            return t;
        }

        /// <summary>Grid of check boxes (cols columns).</summary>
        private static Control MakeGrid(int cols, params Control[] items)
        {
            int rows = (items.Length + cols - 1) / cols;
            var t = new CdaRowTable { ColumnCount = cols, RowCount = rows };
            for (int c = 0; c < cols; c++) t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));
            for (int r = 0; r < rows; r++) t.RowStyles.Add(new RowStyle(SizeType.Absolute, CheckH));
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null) continue;
                PrepCheck(items[i]);
                t.Controls.Add(items[i], i % cols, i / cols);
            }
            t.Tag = rows * CheckH;
            return t;
        }

        private static Control MakeTelemetry(params Control[][] pairs)  // {caption label, combo}
        {
            var t = new CdaRowTable { ColumnCount = pairs.Length, RowCount = 2 };
            for (int c = 0; c < pairs.Length; c++) t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / pairs.Length));
            t.RowStyles.Add(new RowStyle(SizeType.Absolute, 22f));
            t.RowStyles.Add(new RowStyle(SizeType.Absolute, 44f));
            for (int i = 0; i < pairs.Length; i++)
            {
                var cap = (Label)pairs[i][0];
                PrepLabel(cap, "caption");
                var fld = Field(pairs[i][1]);
                fld.Dock = DockStyle.Fill;
                fld.Margin = new Padding(0, 0, i == pairs.Length - 1 ? 0 : 10, 4);
                t.Controls.Add(cap, i, 0);
                t.Controls.Add(fld, i, 1);
            }
            return t;
        }

        private void BuildCdaLayout()
        {
            SuspendLayout();
            AutoScroll = true;

            cdaBanner = new CdaBanner
            {
                Title = "Planner Settings",
                Subtitle = "Video, speech, units, telemetry and display preferences",
                Tagline = "Fly Higher With Us",
                Height = 92
            };

            // ---- Video & HUD -------------------------------------------------
            var video = new CardBuilder("Video & HUD");
            video.Add(MakeRow(label92, Field(CMB_videosources), 0, BUT_videostart, BUT_videostop), RowH);
            video.Add(MakeRow(label26, Field(CMB_videoresolutions), 0), RowH);
            video.Add(MakeRow(label94, Field(CMB_osdcolor), 0), RowH);
            video.Add(MakeRow(label12, CHK_hudshow, 0), CheckH + 4);
            video.Add(MakeRow(null, CHK_GDIPlus, 0), CheckH + 4);

            // ---- Speech ------------------------------------------------------
            var speech = new CardBuilder("Speech Alerts");
            speech.Add(MakeRow(label95, CHK_enablespeech, 0), CheckH + 4);
            speech.Add(MakeRow(null, Field(CMB_severity), 0), RowH);
            speech.Add(MakeRow(null, label8, 0), 26);
            label8.Tag = "note";
            var speechGrid = MakeGrid(2, CHK_speechArmedOnly, CHK_speechwaypoint, CHK_speechaltwarning, CHK_speechbattery,
                CHK_speechcustom, CHK_speechmode, CHK_speecharmdisarm, CHK_speechlowspeed);
            speech.Add(speechGrid, (int)speechGrid.Tag + GridPad);
            cdaSpeech = speech;
            cdaSpeechGrid = speechGrid;

            // ---- Units & Language -------------------------------------------
            var units = new CardBuilder("Units & Language");
            units.Add(MakeRow(label93, Field(CMB_language), 0), RowH);
            units.Add(MakeRow(label97, Field(CMB_distunits), 0), RowH);
            units.Add(MakeRow(label6, Field(CMB_altunits), 0), RowH);
            units.Add(MakeRow(label98, Field(CMB_speedunits), 0), RowH);
            label99.Tag = "note";
            label99.Text = label99.Text.Trim();
            units.Add(MakeRow(null, label99, 0), 46);
            units.Add(MakeRow(label96, BUT_Joystick, TextW(BUT_Joystick) + 20), RowH);

            // ---- Telemetry rates --------------------------------------------
            var telem = new CardBuilder("Telemetry Rates");
            telem.Add(MakeTelemetry(
                new Control[] { label102, CMB_rateattitude },
                new Control[] { label103, CMB_rateposition },
                new Control[] { label104, CMB_ratestatus },
                new Control[] { label107, CMB_raterc },
                new Control[] { label33, CMB_ratesensors }), 70);

            // ---- Connection & parameters ------------------------------------
            var conn = new CardBuilder("Connection & Parameters");
            conn.Add(MakeRow(label108, CHK_resetapmonconnect, 0), CheckH + 4);
            conn.Add(MakeRow(null, CHK_rtsresetesp32, 0), CheckH + 4);
            conn.Add(MakeRow(label7, Field(num_gcsid), 130), RowH);
            var connGrid = MakeGrid(2, CHK_params_bg, CHK_AutoParamCommit, chk_norcreceiver, chk_slowMachine,
                CHK_Password, chk_temp, CHK_mavdebug);
            conn.Add(connGrid, (int)connGrid.Tag + GridPad);

            // ---- Flight data & map ------------------------------------------
            var map = new CardBuilder("Flight Data & Map");
            map.Add(MakeRow(label23, Field(NUM_tracklength), 130), RowH);
            map.Add(MakeRow(label2, CHK_disttohomeflightdata, 0), CheckH + 4);
            map.Add(MakeRow(label24, CHK_loadwponconnect, 0), CheckH + 4);
            map.Add(MakeRow(label1, CHK_maprotation, 0), CheckH + 4);
            map.Add(MakeRow(label13, Field(CMB_mapCache), 0, BUT_mapCacheDir), RowH);
            var mapGrid = MakeGrid(2, CHK_showairports, chk_ADSB, chk_tfr, chk_shownofly);
            map.Add(mapGrid, (int)mapGrid.Tag + GridPad);

            // ---- Aircraft icon ----------------------------------------------
            var plane = new CardBuilder("Aircraft Icon");
            var planeGrid = MakeGrid(2, chk_displaycog, chk_displayheading, chk_displaynavbearing,
                chk_displayradius, chk_displaytarget, chk_displaytooltip);
            plane.Add(planeGrid, (int)planeGrid.Tag + GridPad);
            plane.Add(MakeRow(label11, Field(num_linelength), 130), RowH);
            plane.Add(MakeRow(label9, Field(cmb_secondarydisplaystyle), 0), RowH);

            // ---- Appearance & files -----------------------------------------
            var look = new CardBuilder("Appearance & Files");
            look.Add(MakeRow(label3, Field(txt_log_dir), 0, BUT_logdirbrowse), RowH);
            look.Add(MakeRow(label4, Field(CMB_theme), 0, BUT_themecustom), RowH);
            look.Add(MakeRow(label5, Field(CMB_Layout), 0), RowH);
            look.Add(MakeRow(null, BUT_Vario, TextW(BUT_Vario) + 20), RowH);
            var lookGrid = MakeGrid(2, chk_analytics, CHK_beta);
            look.Add(lookGrid, (int)lookGrid.Tag + GridPad);

            // masonry order (cards go into the shorter column)
            cdaCards.Clear();
            foreach (var cb in new[] { video, conn, speech, map, units, plane, telem, look })
                cdaCards.Add(cb.Done());

            this.Controls.Add(cdaBanner);
            foreach (var c in cdaCards) this.Controls.Add(c);

            SizeChanged += (s, e) => PositionCdaCards();
            ResumeLayout(false);
            PositionCdaCards();
        }

        /// <summary>Show/hide the speech options block and shrink the card when speech is off.</summary>
        private void RefreshCdaSpeech()
        {
            if (cdaSpeech == null) return;
            cdaSpeechGrid.Visible = CHK_speechArmedOnly.Visible;
            cdaSpeech.Recalc();
            PositionCdaCards();
        }

        /// <summary>Two-column masonry on wide windows, single column on narrow ones.</summary>
        private void PositionCdaCards()
        {
            if (cdaPositioning || cdaBanner == null) return;
            cdaPositioning = true;
            try
            {
                SuspendLayout();
                const int margin = 18, gap = 16;
                int w = ClientSize.Width;
                bool two = w >= 900;
                int colW = two ? (w - margin * 2 - gap) / 2 : Math.Max(300, w - margin * 2);
                var off = AutoScrollPosition;     // negative when scrolled

                cdaBanner.SetBounds(margin + off.X, margin + off.Y, Math.Max(300, w - margin * 2), cdaBanner.Height);

                int[] y = { margin + cdaBanner.Height + gap, margin + cdaBanner.Height + gap };
                foreach (var card in cdaCards)
                {
                    int c = (!two || y[0] <= y[1]) ? 0 : 1;
                    card.SetBounds(margin + c * (colW + gap) + off.X, y[c] + off.Y, colW, card.Height);
                    y[c] += card.Height + gap;
                }
                AutoScrollMinSize = new Size(0, Math.Max(y[0], y[1]) - gap + margin);
            }
            finally
            {
                ResumeLayout(false);
                cdaPositioning = false;
            }
        }

        #endregion
    }
}