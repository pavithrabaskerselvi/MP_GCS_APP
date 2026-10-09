using System;
using System.Drawing;
using System.Windows.Forms;
using MissionPlanner.Utilities;

namespace MissionPlanner.GCSViews
{
    // Layout only. No HUD / tab / map logic is changed here.
    //
    // After:   MainH (top / bottom)
    //            top    = tableMap (map full width + coords strip)
    //            bottom = SubMainLeft (hud1 left | tabControlactions right)  <- "drawer"
    //
    // A small button on the strip under the map hides / shows the bottom drawer.
    // Hidden = the map gets the whole screen. The choice is remembered.
    public partial class FlightData
    {
        private bool _cdaStructureDone;
        private bool _cdaDistancesApplied;
        private Button _cdaDrawerBtn;

        // fraction of the Flight Data height used by the bottom drawer (HUD + tabs)
        private const double CdaBottomFraction = 0.36;

        // Call this ONE time in the FlightData constructor, right after InitializeComponent();
        private void InitCdaLayout()
        {
            if (_cdaStructureDone) return;
            _cdaStructureDone = true;

            // --- neutralise old-layout code in FlightData.cs (so no edits needed there) ---
            hud1.Resize -= hud1_Resize;                       // old code forced splitter = hud height
            Settings.Instance.Remove("FlightSplitter");       // old saved splitter belongs to old layout
            Settings.Instance["HudSwap"] = "false";           // swap would break this layout
            swapWithMapToolStripMenuItem.Visible = false;

            SuspendLayout();
            MainH.SuspendLayout();
            SubMainLeft.SuspendLayout();
            try
            {
                MainH.Panel1.Controls.Remove(SubMainLeft);
                MainH.Panel2.Controls.Remove(tableMap);

                MainH.Orientation = Orientation.Horizontal;        // top / bottom
                MainH.FixedPanel = FixedPanel.Panel2;              // drawer keeps height, map grows
                SubMainLeft.Orientation = Orientation.Vertical;    // HUD left | tabs right
                SubMainLeft.FixedPanel = FixedPanel.None;

                try { MainH.Panel1MinSize = 100; MainH.Panel2MinSize = 120; } catch { }
                try { SubMainLeft.Panel1MinSize = 120; SubMainLeft.Panel2MinSize = 250; } catch { }

                tableMap.Dock = DockStyle.Fill;
                SubMainLeft.Dock = DockStyle.Fill;
                MainH.Panel1.Controls.Add(tableMap);       // map on top
                MainH.Panel2.Controls.Add(SubMainLeft);    // HUD + tabs at bottom
            }
            finally
            {
                SubMainLeft.ResumeLayout(false);
                MainH.ResumeLayout(false);
                ResumeLayout(false);
            }

            CreateCdaDrawerButton();

            // Splitter distances need real sizes, so do it at runtime.
            Load += (s, e) => BeginInvoke((Action)(() =>
            {
                ApplyCdaDistances(true);
                SetCdaDrawer(Settings.Instance.GetBoolean("CdaDrawerHidden", false), false);
            }));
            MainH.SizeChanged += (s, e) => ApplyCdaDistances(false);
            // keep the HUD fully visible whenever the drawer changes size
            SubMainLeft.SizeChanged += (s, e) => FitCdaHud();
        }

        // small button on the strip under the map (the one with GEO / Tuning / Auto Pan)
        private void CreateCdaDrawerButton()
        {
            _cdaDrawerBtn = new Button
            {
                Text = "\u25BC Hide panels",
                Size = new Size(120, 22),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(107, 78, 255),   // same purple as the existing CDA theme
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                TabStop = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            _cdaDrawerBtn.FlatAppearance.BorderSize = 0;
            _cdaDrawerBtn.Click += (s, e) => SetCdaDrawer(!MainH.Panel2Collapsed, true);

            panel1.Controls.Add(_cdaDrawerBtn);
            _cdaDrawerBtn.Location = new Point(Math.Max(0, panel1.Width - _cdaDrawerBtn.Width - 20), 0);
            _cdaDrawerBtn.BringToFront();
        }

        // hidden = true  -> map takes the full screen
        // hidden = false -> HUD + tabs drawer is shown
        private void SetCdaDrawer(bool hidden, bool save)
        {
            try
            {
                MainH.Panel2Collapsed = hidden;
                _cdaDrawerBtn.Text = hidden ? "\u25B2 Show panels" : "\u25BC Hide panels";

                if (save)
                    Settings.Instance["CdaDrawerHidden"] = hidden.ToString();

                if (!hidden)
                    BeginInvoke((Action)(() => ApplyCdaDistances(true)));
            }
            catch
            {
            }
        }

        private void ApplyCdaDistances(bool force)
        {
            if (_cdaDistancesApplied && !force) return;
            if (MainH.Height < 300 || SubMainLeft.Width < 400) return;

            try
            {
                int bottomH = (int)(MainH.Height * CdaBottomFraction);
                int top = MainH.Height - bottomH - MainH.SplitterWidth;
                top = Math.Max(MainH.Panel1MinSize, Math.Min(top, MainH.Height - MainH.Panel2MinSize - MainH.SplitterWidth));
                MainH.SplitterDistance = top;

                FitCdaHud();

                _cdaDistancesApplied = true;
            }
            catch
            {
                // size not ready yet, SizeChanged will retry
            }
        }

        // HUD width = drawer height * aspect ratio, so the full HUD fits in the drawer.
        private void FitCdaHud()
        {
            if (SubMainLeft.Height < 100 || SubMainLeft.Width < 400) return;

            try
            {
                double aspect = hud1.SixteenXNine ? 16.0 / 9.0 : 4.0 / 3.0;
                int hudW = (int)((SubMainLeft.Height - 4) * aspect);

                // always leave room for the tabs
                int max = SubMainLeft.Width - SubMainLeft.Panel2MinSize - SubMainLeft.SplitterWidth;
                hudW = Math.Min(hudW, max);
                hudW = Math.Max(SubMainLeft.Panel1MinSize, hudW);

                if (Math.Abs(SubMainLeft.SplitterDistance - hudW) > 2)
                    SubMainLeft.SplitterDistance = hudW;
            }
            catch
            {
            }
        }
    }
}