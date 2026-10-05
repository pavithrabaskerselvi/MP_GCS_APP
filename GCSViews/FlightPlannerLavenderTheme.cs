using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MissionPlanner.Controls;

namespace MissionPlanner.GCSViews
{
    /// <summary>
    /// Lavender (Skylynk) theme for the Plan page.
    /// Call LavenderTheme.Attach(this) from FlightPlanner_Load and Activate().
    /// It keeps re-applying itself, so ThemeManager can no longer turn the page dark again.
    /// </summary>
    public static class LavenderTheme
    {
        // ---------- Palette ----------
        public static readonly Color PageBg      = Color.FromArgb(0xEE, 0xE9, 0xFC);
        public static readonly Color PanelBg     = Color.FromArgb(0xF6, 0xF3, 0xFF);
        public static readonly Color PanelBg2    = Color.FromArgb(0xE4, 0xDD, 0xFA); // gradient end
        public static readonly Color PanelBorder = Color.FromArgb(0xD6, 0xCC, 0xF7);
        public static readonly Color Accent      = Color.FromArgb(0x7C, 0x5C, 0xE6);
        public static readonly Color AccentDark  = Color.FromArgb(0x5B, 0x3F, 0xC9);
        public static readonly Color AccentLight = Color.FromArgb(0x9B, 0x82, 0xF2);
        public static readonly Color InputBg     = Color.White;
        public static readonly Color TextDark    = Color.FromArgb(0x3B, 0x2F, 0x6B);
        public static readonly Color TextMuted   = Color.FromArgb(0x6E, 0x62, 0xA3);
        public static readonly Color RowAlt      = Color.FromArgb(0xF3, 0xEF, 0xFF);
        public static readonly Color Selection   = Color.FromArgb(0xD9, 0xCE, 0xFA);
        public static readonly Color GridLine    = Color.FromArgb(0xE6, 0xDF, 0xFA);

        private const string FontName = "Segoe UI";

        private static readonly HashSet<Control> rounded = new HashSet<Control>();
        private static readonly HashSet<FlightPlanner> attached = new HashSet<FlightPlanner>();

        // =====================================================================
        //  Attach: apply now + keep applying (fixes "theme only works first time")
        // =====================================================================
        public static void Attach(FlightPlanner fp)
        {
            if (fp == null) return;

            Apply(fp);

            // run again after ThemeManager has finished repainting
            if (fp.IsHandleCreated)
                fp.BeginInvoke((Action)(() => Apply(fp)));

            if (attached.Contains(fp)) return;
            attached.Add(fp);

            fp.VisibleChanged += (s, e) => { if (fp.Visible) Apply(fp); };
            fp.ParentChanged += (s, e) => Apply(fp);

            // gradient + accent edge on the two side panels
            fp.panelAction.Paint += (s, e) => PaintCard(e.Graphics, fp.panelAction, true);
            fp.panelWaypoints.Paint += (s, e) => PaintCard(e.Graphics, fp.panelWaypoints, false);

            // guard timer: if something turned the page dark again, restore it
            var guard = new System.Windows.Forms.Timer { Interval = 300 };
            guard.Tick += (s, e) =>
            {
                if (fp.IsDisposed) { guard.Stop(); guard.Dispose(); return; }
                if (!fp.Visible) return;
                if (IsDrifted(fp)) Apply(fp);
            };
            guard.Start();
            fp.Disposed += (s, e) => { guard.Stop(); guard.Dispose(); attached.Remove(fp); };
        }

        private static bool IsDrifted(FlightPlanner fp)
        {
            return fp.BackColor != PageBg ||
                   fp.panelAction.BackColor != PanelBg ||
                   fp.panelWaypoints.BackColor != PanelBg ||
                   fp.Commands.BackgroundColor != PanelBg;
        }

        // =====================================================================
        //  Apply
        // =====================================================================
        public static void Apply(FlightPlanner fp)
        {
            if (fp == null || fp.IsDisposed) return;

            fp.SuspendLayout();

            fp.BackColor = PageBg;
            fp.ForeColor = TextDark;

            Style(fp);
            StyleGrid(fp.Commands);

            fp.MainMap.EmptyTileColor = PanelBg2;
            fp.MainMap.BackColor = PanelBg2;

            StyleMenu(fp.contextMenuStrip1);
            StyleMenu(fp.contextMenuStripPoly);

            // floating info labels on top of the map: readable "chips"
            foreach (var l in new[] { fp.lbl_status, fp.lbl_distance, fp.lbl_prevdist, fp.lbl_homedist })
            {
                if (l == null) continue;
                l.ForeColor = AccentDark;
                l.BackColor = Color.FromArgb(0xF6, 0xF3, 0xFF);
            }

            fp.ResumeLayout(true);
            fp.Invalidate(true);
        }

        // ---------- Recursive control styling ----------
        private static void Style(Control root)
        {
            foreach (Control c in root.Controls)
            {
                SetFont(c);

                switch (c)
                {
                    case MyButton b:
                        StyleButton(b);
                        break;

                    case DataGridView _:
                        break; // StyleGrid

                    case TextBox t:
                        t.BackColor = InputBg;
                        t.ForeColor = TextDark;
                        t.BorderStyle = BorderStyle.FixedSingle;
                        break;

                    case NumericUpDown n:
                        n.BackColor = InputBg;
                        n.ForeColor = TextDark;
                        break;

                    case ComboBox cb:
                        cb.BackColor = InputBg;
                        cb.ForeColor = TextDark;
                        cb.FlatStyle = FlatStyle.Flat;
                        break;

                    case CheckBox ck:
                        ck.ForeColor = TextDark;
                        ck.BackColor = Color.Transparent;
                        break;

                    case LinkLabel ll:
                        ll.LinkColor = AccentDark;
                        ll.ActiveLinkColor = Accent;
                        ll.VisitedLinkColor = AccentLight;
                        ll.BackColor = Color.Transparent;
                        break;

                    case Label lb:
                        lb.ForeColor = TextDark;
                        lb.BackColor = Color.Transparent;
                        break;

                    case Splitter sp:
                        sp.BackColor = PanelBorder;
                        break;

                    case Panel _:
                        c.BackColor = PanelBg;
                        c.ForeColor = TextDark;
                        break;

                    default:
                        if (!(c is GMap.NET.WindowsForms.GMapControl))
                        {
                            c.BackColor = PanelBg;
                            c.ForeColor = TextDark;
                        }
                        break;
                }

                if (c.HasChildren && !(c is DataGridView))
                    Style(c);
            }
        }

        private static void SetFont(Control c)
        {
            if (c is GMap.NET.WindowsForms.GMapControl || c is DataGridView) return;
            if (c.Font != null && c.Font.Name != FontName)
            {
                try { c.Font = new Font(FontName, c.Font.Size, c.Font.Style); } catch { }
            }
        }

        // ---------- Buttons: purple gradient + rounded corners ----------
        private static void StyleButton(MyButton b)
        {
            b.BGGradTop = AccentLight;
            b.BGGradBot = Accent;
            b.Outline = AccentDark;
            b.TextColor = Color.White;
            b.ColorMouseOver = AccentDark;
            b.ColorMouseDown = Color.FromArgb(0x48, 0x32, 0xA8);
            b.ColorNotEnabled = Color.FromArgb(0xC9, 0xC1, 0xE6);
            b.BackColor = Color.Transparent;

            RoundCorners(b, 8);
            b.Invalidate();
        }

        private static void RoundCorners(Control c, int radius)
        {
            ApplyRegion(c, radius);
            if (rounded.Add(c))
            {
                c.SizeChanged += (s, e) => ApplyRegion(c, radius);
                c.Disposed += (s, e) => rounded.Remove(c);
            }
        }

        private static void ApplyRegion(Control c, int r)
        {
            if (c.Width < 4 || c.Height < 4) return;
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

        // ---------- Side-panel gradient + accent edge ----------
        private static void PaintCard(Graphics g, Panel p, bool vertical)
        {
            if (p.Width <= 0 || p.Height <= 0) return;
            var rect = p.ClientRectangle;

            using (var br = new LinearGradientBrush(rect, PanelBg, PanelBg2, LinearGradientMode.Vertical))
                g.FillRectangle(br, rect);

            using (var pen = new Pen(Accent, 3))
            {
                if (vertical) g.DrawLine(pen, 1, 0, 1, rect.Height);          // left edge of the sidebar
                else g.DrawLine(pen, 0, 1, rect.Width, 1);                    // top edge of the table
            }
        }

        // ---------- Waypoint grid ----------
        private static void StyleGrid(DataGridView g)
        {
            if (g == null) return;

            g.EnableHeadersVisualStyles = false;
            g.BackgroundColor = PanelBg;
            g.GridColor = GridLine;
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 32;

            var hdr = g.ColumnHeadersDefaultCellStyle;
            hdr.BackColor = Accent;
            hdr.ForeColor = Color.White;
            hdr.SelectionBackColor = Accent;
            hdr.SelectionForeColor = Color.White;
            hdr.Alignment = DataGridViewContentAlignment.MiddleCenter;
            hdr.Font = new Font(FontName, 9f, FontStyle.Bold);

            var rh = g.RowHeadersDefaultCellStyle;
            rh.BackColor = AccentLight;
            rh.ForeColor = Color.White;
            rh.SelectionBackColor = AccentDark;
            rh.SelectionForeColor = Color.White;

            foreach (var st in new[] { g.DefaultCellStyle, g.RowsDefaultCellStyle })
            {
                st.BackColor = InputBg;
                st.ForeColor = TextDark;
                st.SelectionBackColor = Selection;
                st.SelectionForeColor = AccentDark;
                st.Padding = new Padding(2, 2, 2, 2);
            }

            var alt = g.AlternatingRowsDefaultCellStyle;
            alt.BackColor = RowAlt;
            alt.ForeColor = TextDark;
            alt.SelectionBackColor = Selection;
            alt.SelectionForeColor = AccentDark;

            g.RowTemplate.Height = 26;

            foreach (DataGridViewColumn col in g.Columns)
                if (col is DataGridViewComboBoxColumn cc)
                    cc.FlatStyle = FlatStyle.Flat;
        }

        // ---------- Right-click menus ----------
        private static void StyleMenu(ContextMenuStrip m)
        {
            if (m == null) return;
            m.Renderer = new LavenderMenuRenderer();
            m.BackColor = PanelBg;
            m.ForeColor = TextDark;
            m.Font = new Font(FontName, 9f);
            foreach (ToolStripItem i in m.Items) StyleItem(i);
        }

        private static void StyleItem(ToolStripItem i)
        {
            i.BackColor = PanelBg;
            i.ForeColor = TextDark;
            if (i is ToolStripMenuItem mi)
                foreach (ToolStripItem sub in mi.DropDownItems) StyleItem(sub);
        }

        private class LavenderMenuRenderer : ToolStripProfessionalRenderer
        {
            public LavenderMenuRenderer() : base(new LavenderColors()) { }
        }

        private class LavenderColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected => Selection;
            public override Color MenuItemSelectedGradientBegin => Selection;
            public override Color MenuItemSelectedGradientEnd => Selection;
            public override Color MenuItemBorder => AccentLight;
            public override Color MenuBorder => PanelBorder;
            public override Color ToolStripDropDownBackground => PanelBg;
            public override Color ImageMarginGradientBegin => RowAlt;
            public override Color ImageMarginGradientMiddle => RowAlt;
            public override Color ImageMarginGradientEnd => RowAlt;
            public override Color SeparatorDark => PanelBorder;
            public override Color SeparatorLight => PanelBorder;
        }
    }
}