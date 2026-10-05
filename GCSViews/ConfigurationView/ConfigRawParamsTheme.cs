using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using MissionPlanner.Controls;

namespace MissionPlanner.GCSViews
{
    // =====================================================================
    //  Chennai Drone Academy - "Full Parameter List" page theme
    //  Uses the shared CdaTheme palette (SITLTheme.cs) and CdaCheckBox /
    //  CdaRowTable (ConfigPlannerTheme.cs).
    // =====================================================================

    /// <summary>Rounded purple action button (ignores ThemeManager colours).</summary>
    internal class CdaActionButton : Button
    {
        private bool _hover;
        private bool _down;

        /// <summary>Soft lavender variant (used for the small collapse tab).</summary>
        public bool Subtle { get; set; }

        public CdaActionButton()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; _down = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnMouseDown(MouseEventArgs e) { _down = true; Invalidate(); base.OnMouseDown(e); }
        protected override void OnMouseUp(MouseEventArgs e) { _down = false; Invalidate(); base.OnMouseUp(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
        protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

        // colour of whatever the button sits on, so the rounded corners blend in
        private Color Backdrop()
        {
            for (Control p = Parent; p != null; p = p.Parent)
            {
                if (p.BackColor.A == 255)
                    return p.BackColor;
            }
            return CdaTheme.Page;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(Backdrop());
            if (Width < 6 || Height < 6) return;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            Color fg;
            using (var path = CdaTheme.Round(rect, Math.Max(4, Math.Min(10, Height / 3))))
            {
                if (!Enabled)
                {
                    using (var br = new SolidBrush(Color.FromArgb(230, 226, 248))) g.FillPath(br, path);
                    using (var pen = new Pen(Color.FromArgb(212, 205, 243))) g.DrawPath(pen, path);
                    fg = Color.FromArgb(160, 150, 205);
                }
                else if (Subtle)
                {
                    using (var br = new SolidBrush(_down ? Color.FromArgb(214, 206, 248) : _hover ? Color.FromArgb(226, 220, 252) : Color.FromArgb(236, 232, 253)))
                        g.FillPath(br, path);
                    using (var pen = new Pen(CdaTheme.CardBorder)) g.DrawPath(pen, path);
                    fg = CdaTheme.Primary;
                }
                else
                {
                    Color top = _down ? CdaTheme.PrimaryDark : _hover ? CdaTheme.Primary : CdaTheme.PrimaryLight;
                    Color bot = _down ? CdaTheme.PrimaryDark : _hover ? CdaTheme.PrimaryDark : CdaTheme.Primary;
                    using (var br = new LinearGradientBrush(rect, top, bot, 90f)) g.FillPath(br, path);
                    using (var pen = new Pen(CdaTheme.PrimaryDark)) g.DrawPath(pen, path);
                    fg = Color.White;
                }
            }

            if (Focused && ShowFocusCues)
            {
                var ring = new Rectangle(2, 2, Width - 5, Height - 5);
                using (var path = CdaTheme.Round(ring, Math.Max(3, Math.Min(8, Height / 3))))
                using (var pen = new Pen(Color.FromArgb(120, 255, 255, 255), 1f))
                    g.DrawPath(pen, path);
            }

            var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                        TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis;
            TextRenderer.DrawText(g, Text, Font, ClientRectangle, fg, flags);
        }
    }
}

namespace MissionPlanner.GCSViews.ConfigurationView
{
    public partial class ConfigRawParams
    {
        // cell highlights (queued change / rejected value) that stay readable on the light grid
        private static readonly Color CdaEditedColor = Color.FromArgb(205, 238, 216);
        private static readonly Color CdaInvalidColor = Color.FromArgb(255, 205, 212);

        private static readonly Font CdaFontBase = new Font("Segoe UI", 8.5f);
        private static readonly Font CdaFontBold = new Font("Segoe UI Semibold", 8.5f);
        private static readonly Font CdaFontNote = new Font("Segoe UI", 8f);
        private static readonly Font CdaFontInput = new Font("Segoe UI", 9f);
        private static readonly Font CdaFontTree = new Font("Segoe UI", 9f);

        private readonly HashSet<Control> _cdaGuarded = new HashSet<Control>();
        private bool _cdaHooked;

        /// <summary>
        /// Applies the lavender theme to the Full Parameter List page. Safe to call repeatedly;
        /// call it again after ThemeManager has touched the page.
        /// </summary>
        internal void ApplyCdaTheme()
        {
            if (!_cdaHooked)
            {
                _cdaHooked = true;
                VisibleChanged += (s, e) => { if (Visible) ApplyCdaTheme(); };
                treeView1.DrawNode += CdaTree_DrawNode;
            }

            CdaGuard(this, CdaTheme.Page, CdaTheme.Text);

            // ---- shell -------------------------------------------------
            CdaGuard(splitContainer1, CdaTheme.Page, CdaTheme.Text);
            CdaGuard(splitContainer1.Panel1, CdaTheme.Page, CdaTheme.Text);
            CdaGuard(splitContainer1.Panel2, CdaTheme.Page, CdaTheme.Text);
            splitContainer1.SplitterWidth = 6;
            splitContainer1.Panel1.Padding = new Padding(8, 8, 2, 8);
            splitContainer1.Panel2.Padding = new Padding(2, 8, 8, 8);

            but_collapse.Subtle = true;
            but_collapse.Font = CdaFontBold;

            // ---- parameter tree ---------------------------------------
            treeView1.BorderStyle = BorderStyle.None;
            treeView1.Font = CdaFontTree;
            if (treeView1.ItemHeight != 24) treeView1.ItemHeight = 24;
            treeView1.ShowLines = true;
            treeView1.HideSelection = false;
            treeView1.LineColor = CdaTheme.CardBorder;
            if (treeView1.DrawMode != TreeViewDrawMode.OwnerDrawText)
                treeView1.DrawMode = TreeViewDrawMode.OwnerDrawText;
            CdaGuard(treeView1, CdaTheme.Card, CdaTheme.Text);

            // ---- parameter grid ---------------------------------------
            Params.BorderStyle = BorderStyle.None;
            Params.BackgroundColor = CdaTheme.Card;
            Params.EnableHeadersVisualStyles = false;
            Params.GridColor = Color.FromArgb(228, 222, 248);
            Params.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            Params.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            Params.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            Params.ColumnHeadersHeight = 30;

            DataGridViewCellStyle head = Params.ColumnHeadersDefaultCellStyle;
            head.BackColor = CdaTheme.Primary;
            head.ForeColor = Color.White;
            head.Font = CdaFontBold;
            head.SelectionBackColor = CdaTheme.Primary;
            head.SelectionForeColor = Color.White;
            head.Alignment = DataGridViewContentAlignment.MiddleLeft;
            head.Padding = new Padding(6, 0, 0, 0);

            DataGridViewCellStyle body = Params.DefaultCellStyle;
            body.BackColor = Color.White;
            body.ForeColor = CdaTheme.Text;
            body.Font = CdaFontBase;
            body.SelectionBackColor = Color.FromArgb(226, 220, 250);
            body.SelectionForeColor = CdaTheme.Text;

            DataGridViewCellStyle alt = Params.AlternatingRowsDefaultCellStyle;
            alt.BackColor = Color.FromArgb(248, 246, 254);
            alt.ForeColor = CdaTheme.Text;
            alt.SelectionBackColor = Color.FromArgb(226, 220, 250);
            alt.SelectionForeColor = CdaTheme.Text;

            // ---- right-hand action column -----------------------------
            tableLayoutPanel1.Padding = new Padding(10, 8, 10, 8);
            foreach (Control c in tableLayoutPanel1.Controls)
                CdaStyleSide(c);

            Params.Invalidate();
            treeView1.Invalidate();
        }

        private void CdaStyleSide(Control c)
        {
            if (c is CdaActionButton b)
            {
                b.Font = CdaFontBold;
                b.MinimumSize = new Size(150, 30);
                b.Margin = new Padding(3, 3, 3, 3);
            }
            else if (c is CdaCheckBox cb)
            {
                cb.Font = CdaFontBase;
            }
            else if (c is Label l)
            {
                bool note = ReferenceEquals(l, label1);
                l.Font = note ? CdaFontNote : CdaFontBold;
                l.MinimumSize = note ? new Size(150, 0) : Size.Empty;
                CdaGuard(l, Color.Transparent, note ? CdaTheme.TextMuted : CdaTheme.Text);
            }
            else if (c is ComboBox combo)
            {
                combo.Font = CdaFontInput;
                combo.FlatStyle = FlatStyle.Flat;
                combo.MinimumSize = new Size(150, 0);
                CdaGuard(combo, CdaTheme.Field, CdaTheme.Text);
            }
            else if (c is TextBox t)
            {
                t.Font = CdaFontInput;
                t.BorderStyle = BorderStyle.FixedSingle;
                t.MinimumSize = new Size(150, 0);
                CdaGuard(t, CdaTheme.Field, CdaTheme.Text);
            }
        }

        /// <summary>Keeps a control on our colours even if ThemeManager recolours it later.</summary>
        private void CdaGuard(Control c, Color back, Color fore)
        {
            if (_cdaGuarded.Add(c))
            {
                c.Disposed += (s, e) => _cdaGuarded.Remove(c);
                CdaTheme.Guard(c, back, fore);
            }
            else
            {
                c.BackColor = back;
                c.ForeColor = fore;
            }
        }

        /// <summary>Styles the editors the grid creates on the fly (bitmask button, option list, numeric box).</summary>
        private static void CdaStyleInline(Control c)
        {
            if (c is MyButton b)
            {
                b.BGGradTop = CdaTheme.PrimaryLight;
                b.BGGradBot = CdaTheme.Primary;
                b.Outline = CdaTheme.PrimaryDark;
                b.TextColor = Color.White;
                b.TextColorNotEnabled = Color.FromArgb(200, 190, 245);
                b.ColorMouseOver = CdaTheme.PrimaryDark;
                b.ColorMouseDown = CdaTheme.PrimaryDark;
                b.Invalidate();
            }
            else if (c is ComboBox cb)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.BackColor = CdaTheme.Field;
                cb.ForeColor = CdaTheme.Text;
            }
            else if (c is NumericUpDown n)
            {
                n.BackColor = CdaTheme.Field;
                n.ForeColor = CdaTheme.Text;
                foreach (Control inner in n.Controls)
                {
                    inner.BackColor = CdaTheme.Field;
                    inner.ForeColor = CdaTheme.Text;
                }
            }
        }

        /// <summary>Owner-drawn tree node: purple pill for the selected node, plain lavender text otherwise.</summary>
        private void CdaTree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node == null || e.Bounds.Width <= 0 || e.Bounds.Height <= 0)
                return;

            var g = e.Graphics;
            bool selected = (e.State & TreeNodeStates.Selected) != 0;
            Rectangle text = e.Bounds;
            var pill = new Rectangle(text.X - 2, text.Y + 1, text.Width + 6, text.Height - 2);

            using (var br = new SolidBrush(treeView1.BackColor))
                g.FillRectangle(br, pill);

            if (selected)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var path = CdaTheme.Round(pill, 6))
                using (var br = new LinearGradientBrush(pill, CdaTheme.PrimaryLight, CdaTheme.Primary, 0f))
                    g.FillPath(br, path);
            }

            Font font = e.Node.NodeFont ?? treeView1.Font;
            TextRenderer.DrawText(g, e.Node.Text, font, text,
                selected ? Color.White : CdaTheme.Text,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
                TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }
    }
}