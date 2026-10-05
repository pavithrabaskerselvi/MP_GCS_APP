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
    //  Chennai Drone Academy – "Planner" configuration page theme
    //  Uses the shared CdaTheme palette / CdaCard / CdaField / CdaTable /
    //  CdaIcons that already live in SITLTheme.cs.
    // =====================================================================

    /// <summary>Rounded purple check box (ignores ThemeManager colours).</summary>
    internal class CdaCheckBox : CheckBox
    {
        private bool _hover;

        public CdaCheckBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
            AutoSize = false;
        }

        // opaque on purpose: transparent owner-drawn controls leave ghost images when the page scrolls
        public override Color BackColor { get { return CdaTheme.Card; } set { } }

        protected override void OnPaintBackground(PaintEventArgs e) { e.Graphics.Clear(CdaTheme.Card); }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnCheckedChanged(EventArgs e) { Invalidate(); base.OnCheckedChanged(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
        protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
        protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }
        protected override void OnTextChanged(EventArgs e) { Invalidate(); base.OnTextChanged(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(CdaTheme.Card);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            const int s = 18;
            var box = new Rectangle(1, (Height - s) / 2, s, s);
            int alpha = Enabled ? 255 : 120;

            using (var path = CdaTheme.Round(box, 5))
            {
                if (Checked)
                {
                    using (var br = new LinearGradientBrush(box,
                        Color.FromArgb(alpha, CdaTheme.PrimaryLight), Color.FromArgb(alpha, CdaTheme.Primary), 90f))
                        g.FillPath(br, path);
                    using (var pen = new Pen(Color.FromArgb(alpha, CdaTheme.PrimaryDark), 1f))
                        g.DrawPath(pen, path);
                    using (var tick = new Pen(Color.White, 2.2f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
                        g.DrawLines(tick, new[]
                        {
                            new Point(box.X + 4, box.Y + 9),
                            new Point(box.X + 8, box.Y + 13),
                            new Point(box.X + 14, box.Y + 5)
                        });
                }
                else
                {
                    using (var br = new SolidBrush(Color.FromArgb(alpha, _hover ? Color.FromArgb(244, 241, 255) : Color.White)))
                        g.FillPath(br, path);
                    using (var pen = new Pen(_hover ? CdaTheme.PrimaryLight : Color.FromArgb(188, 178, 238), 1.5f))
                        g.DrawPath(pen, path);
                }
            }

            if (Focused && ShowFocusCues)
            {
                var ring = new Rectangle(box.X - 2, box.Y - 2, s + 4, s + 4);
                using (var path = CdaTheme.Round(ring, 7))
                using (var pen = new Pen(Color.FromArgb(120, CdaTheme.PrimaryLight), 1.2f))
                    g.DrawPath(pen, path);
            }

            var flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis;
            TextRenderer.DrawText(g, Text, Font, new Rectangle(s + 10, 0, Math.Max(0, Width - s - 10), Height),
                Enabled ? CdaTheme.Text : CdaTheme.TextMuted, flags);
        }
    }

    /// <summary>Row/grid container that sits on a card: opaque card colour, ignores ThemeManager.</summary>
    internal class CdaRowTable : TableLayoutPanel
    {
        public CdaRowTable()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
        public override Color BackColor { get { return CdaTheme.Card; } set { } }
        protected override void OnPaintBackground(PaintEventArgs e) { e.Graphics.Clear(CdaTheme.Card); }
    }

    /// <summary>Section card: rounded lavender card with a gradient accent bar + title + divider.</summary>
    internal class CdaPanelCard : CdaCard
    {
        public string Title { get; set; } = "";

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var bar = new Rectangle(18, 16, 5, 22);
            using (var path = CdaTheme.Round(bar, 2))
            using (var br = new LinearGradientBrush(bar, CdaTheme.PrimaryLight, CdaTheme.PrimaryDark, 90f))
                g.FillPath(br, path);

            using (var f = new Font("Segoe UI Semibold", 11f))
                TextRenderer.DrawText(g, Title, f, new Rectangle(32, 12, Width - 50, 30), CdaTheme.Text,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);

            using (var pen = new Pen(Color.FromArgb(120, CdaTheme.CardBorder)))
                g.DrawLine(pen, 18, 46, Width - 19, 46);
        }
    }

    /// <summary>Gradient header banner with drone icon, title, subtitle and tagline.</summary>
    internal class CdaBanner : Control
    {
        public string Title { get; set; } = "";
        public string Subtitle { get; set; } = "";
        public string Tagline { get; set; } = "";
        private Bitmap _icon;

        public CdaBanner()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
            _icon = CdaIcons.Make(CdaIcons.Kind.Quad, 64, Color.White);
        }

        public override Color BackColor { get { return Color.Transparent; } set { } }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _icon != null) { _icon.Dispose(); _icon = null; }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = CdaTheme.Round(rect, 16))
            {
                using (var br = new LinearGradientBrush(rect, CdaTheme.PrimaryDark, CdaTheme.PrimaryLight, 20f))
                    g.FillPath(br, path);

                var state = g.Save();
                g.SetClip(path, CombineMode.Intersect);

                // soft decorative circles
                using (var br = new SolidBrush(Color.FromArgb(28, 255, 255, 255)))
                {
                    g.FillEllipse(br, Width - 210, -70, 260, 260);
                    g.FillEllipse(br, Width - 330, 20, 150, 150);
                }
                // hills along the bottom-right
                using (var br = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
                {
                    int bx = Math.Max(0, Width - 420), by = Height;
                    g.FillPolygon(br, new[]
                    {
                        new Point(bx, by), new Point(bx + 90, by - 34), new Point(bx + 150, by - 18),
                        new Point(bx + 250, by - 50), new Point(bx + 340, by - 20), new Point(Width, by - 40), new Point(Width, by)
                    });
                }
                g.Restore(state);
            }

            // icon badge
            var badge = new Rectangle(20, (Height - 54) / 2, 54, 54);
            using (var path = CdaTheme.Round(badge, 14))
            {
                using (var br = new SolidBrush(Color.FromArgb(50, 255, 255, 255))) g.FillPath(br, path);
                using (var pen = new Pen(Color.FromArgb(110, 255, 255, 255))) g.DrawPath(pen, path);
            }
            if (_icon != null)
                g.DrawImage(_icon, new Rectangle(badge.X + 9, badge.Y + 9, 36, 36));

            int tx = badge.Right + 16;
            using (var f = new Font("Segoe UI Semibold", 17f))
                TextRenderer.DrawText(g, Title, f, new Rectangle(tx, 14, Width - tx - 260, 32), Color.White,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            using (var f = new Font("Segoe UI", 9f))
                TextRenderer.DrawText(g, Subtitle, f, new Rectangle(tx, 48, Width - tx - 260, 22), Color.FromArgb(230, 224, 255),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);

            using (var f = new Font("Segoe UI", 11f, FontStyle.Italic))
                TextRenderer.DrawText(g, Tagline, f, new Rectangle(Width - 250, 0, 232, Height), Color.White,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }
    }

    /// <summary>
    /// Applies the Chennai Drone Academy lavender theme to the Planner configuration page.
    /// Colours are guarded, so a later ThemeManager.ApplyThemeTo(...) cannot undo them.
    /// </summary>
    internal static class CdaPageTheme
    {
        private static readonly HashSet<Control> guarded = new HashSet<Control>();

        private static readonly Font FLabel = new Font("Segoe UI Semibold", 9f);
        private static readonly Font FNote = new Font("Segoe UI", 8f);
        private static readonly Font FCaption = new Font("Segoe UI Semibold", 8f);
        private static readonly Font FInput = new Font("Segoe UI", 9f);
        private static readonly Font FCheck = new Font("Segoe UI", 8.75f);
        private static readonly Font FButton = new Font("Segoe UI Semibold", 8.75f);

        public static void Apply(UserControl page)
        {
            page.AutoScroll = true;
            GuardOnce(page, CdaTheme.Page, CdaTheme.Text);
            Walk(page);
        }

        private static void Walk(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                Style(c);
                if (c.HasChildren && !(c is NumericUpDown) && !(c is ComboBox))
                    Walk(c);
            }
        }

        private static void Style(Control c)
        {
            if (c is CdaCheckBox)
            {
                c.Font = FCheck;
            }
            else if (c is MyButton b)
            {
                // MyButton paints itself from these properties
                b.Font = FButton;
                b.BGGradTop = CdaTheme.PrimaryLight;
                b.BGGradBot = CdaTheme.Primary;
                b.Outline = CdaTheme.PrimaryDark;
                b.TextColor = Color.White;
                b.TextColorNotEnabled = Color.FromArgb(200, 190, 245);
                b.ColorMouseOver = CdaTheme.PrimaryDark;
                b.ColorMouseDown = CdaTheme.PrimaryDark;
                b.Invalidate();
            }
            else if (c is Label l)
            {
                string tag = c.Tag as string;
                Font f = tag == "note" ? FNote : tag == "caption" ? FCaption : FLabel;
                Color fore = (tag == "note" || tag == "caption") ? CdaTheme.TextMuted : CdaTheme.Text;
                l.Font = f;
                GuardOnce(c, Color.Transparent, fore);
            }
            else if (c is CheckBox || c is RadioButton)
            {
                GuardOnce(c, Color.Transparent, CdaTheme.Text);
                ((ButtonBase)c).UseVisualStyleBackColor = false;
            }
            else if (c is NumericUpDown n)
            {
                n.Font = FInput;
                n.BorderStyle = n.Parent is CdaField ? BorderStyle.None : BorderStyle.FixedSingle;
                GuardDeepOnce(n, CdaTheme.Field, CdaTheme.Text);
            }
            else if (c is ComboBox cb)
            {
                cb.Font = FInput;
                cb.FlatStyle = FlatStyle.Flat;
                GuardOnce(cb, CdaTheme.Field, CdaTheme.Text);
            }
            else if (c is TextBox t)
            {
                t.Font = FInput;
                t.BorderStyle = t.Parent is CdaField ? BorderStyle.None : BorderStyle.FixedSingle;
                GuardOnce(t, CdaTheme.Field, CdaTheme.Text);
            }
            else if (c is Panel || c is GroupBox)
            {
                GuardOnce(c, Color.Transparent, CdaTheme.Text);
            }
        }

        private static void GuardOnce(Control c, Color back, Color fore)
        {
            if (guarded.Add(c))
            {
                c.Disposed += (s, e) => guarded.Remove(c);
                CdaTheme.Guard(c, back, fore);
            }
            else
            {
                c.BackColor = back;
                c.ForeColor = fore;
            }
        }

        private static void GuardDeepOnce(Control c, Color back, Color fore)
        {
            GuardOnce(c, back, fore);
            foreach (Control child in c.Controls)
                GuardDeepOnce(child, back, fore);
        }
    }
}