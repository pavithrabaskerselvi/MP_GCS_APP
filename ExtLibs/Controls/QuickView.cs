using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace MissionPlanner.Controls
{
    // CDA light lavender glass card: soft shadow, icon + caption on top,
    // big gradient number. The number colour comes from numberColor.
    public partial class QuickView : SkiaSharp.Views.Desktop.SKControl
    {
        [System.ComponentModel.Browsable(true)]
        public string desc
        {
            get { return _desc; } set { if (_desc == value) return; _desc = value; Invalidate(); }
        }

        double _number = -9999;

        [System.ComponentModel.Browsable(true)]
        public double number
        {
            get { return _number; }
            set
            {
                lock (this)
                {
                    if (_number.Equals(value))
                        return;
                    _number = value;
                    Invalidate();
                }
            }
        }

        string _numberformat = "0.00";
        private string _desc = "";
        private Color _numbercolor;

        [System.ComponentModel.Browsable(true)]
        public string numberformat
        {
            get
            {
                return _numberformat;
            }
            set
            {
                if (_numberformat.Equals(value))
                    return;
                _numberformat = value;
                this.Invalidate();
            }
        }

        [System.ComponentModel.Browsable(true)]
        public Color numberColor { get { return _numbercolor; } set { if (_numbercolor == value) return; _numbercolor = value; Invalidate(); } }

        //We use this property as a backup store for the numberColor, so it is possible to change numberColor temporary.
        public Color numberColorBackup { get; set; }

        public QuickView()
        {
            InitializeComponent();

            PaintSurface += OnPaintSurface;
        }

        // ------------------------------------------------------------------
        //  helpers
        // ------------------------------------------------------------------
        static readonly Dictionary<string, SKTypeface> typefaces = new Dictionary<string, SKTypeface>();

        static SKTypeface GetTypeface(string family, bool bold)
        {
            lock (typefaces)
            {
                string key = family + (bold ? "|b" : "|r");
                SKTypeface tf;
                if (!typefaces.TryGetValue(key, out tf))
                {
                    tf = SKTypeface.FromFamilyName(family, bold ? SKFontStyle.Bold : SKFontStyle.Normal);
                    typefaces[key] = tf;
                }
                return tf;
            }
        }

        static SKColor ToSk(Color c)
        {
            return new SKColor(c.R, c.G, c.B, c.A);
        }

        // second colour of the number gradient
        static SKColor EndColor(Color c)
        {
            if (c.R > 200 && c.G > 170 && c.B < 120)
                return new SKColor(255, 140, 40); // yellow -> orange

            // everything else drifts towards violet
            float k = 0.55f;
            return new SKColor(
                (byte)(c.R + (140 - c.R) * k),
                (byte)(c.G + (80 - c.G) * k),
                (byte)(c.B + (235 - c.B) * k));
        }

        // small line icon, picked from the caption text
        static void DrawIcon(SKCanvas canvas, string desc, float cx, float cy, float size, SKColor col)
        {
            string d = (desc ?? "").ToLowerInvariant();
            float r = size / 2f;

            using (var p = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = Math.Max(1.6f, size / 11f),
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                Color = col
            })
            using (var path = new SKPath())
            {
                Func<float, float, SKPoint> P = (x, y) => new SKPoint(cx + x * r, cy + y * r);

                if (d.Contains("vertical"))
                {
                    canvas.DrawLine(P(-0.4f, 0.8f).X, P(-0.4f, 0.8f).Y, P(-0.4f, -0.8f).X, P(-0.4f, -0.8f).Y, p);
                    path.MoveTo(P(-0.75f, -0.4f)); path.LineTo(P(-0.4f, -0.85f)); path.LineTo(P(-0.05f, -0.4f));
                    canvas.DrawLine(P(0.4f, -0.8f).X, P(0.4f, -0.8f).Y, P(0.4f, 0.8f).X, P(0.4f, 0.8f).Y, p);
                    path.MoveTo(P(0.05f, 0.4f)); path.LineTo(P(0.4f, 0.85f)); path.LineTo(P(0.75f, 0.4f));
                    canvas.DrawPath(path, p);
                }
                else if (d.Contains("speed") || d.Contains("ground"))
                {
                    canvas.DrawCircle(cx, cy, r * 0.9f, p);
                    canvas.DrawLine(P(0f, 0.1f).X, P(0f, 0.1f).Y, P(0.5f, -0.5f).X, P(0.5f, -0.5f).Y, p);
                    canvas.DrawCircle(cx, cy + r * 0.1f, r * 0.09f, p);
                }
                else if (d.Contains("alt"))
                {
                    path.MoveTo(P(-0.95f, 0.7f)); path.LineTo(P(-0.35f, -0.1f)); path.LineTo(P(0f, 0.4f));
                    path.LineTo(P(0.4f, -0.6f)); path.LineTo(P(0.95f, 0.7f)); path.Close();
                    canvas.DrawPath(path, p);
                }
                else if (d.Contains("wp"))
                {
                    path.MoveTo(P(-0.42f, -0.05f)); path.LineTo(P(0f, 0.95f)); path.LineTo(P(0.42f, -0.05f));
                    canvas.DrawPath(path, p);
                    canvas.DrawCircle(cx, cy - r * 0.25f, r * 0.45f, p);
                    canvas.DrawCircle(cx, cy - r * 0.25f, r * 0.14f, p);
                }
                else if (d.Contains("yaw") || d.Contains("head"))
                {
                    canvas.DrawCircle(cx, cy, r * 0.9f, p);
                    path.MoveTo(P(0f, -0.6f)); path.LineTo(P(0.25f, 0.2f)); path.LineTo(P(0f, 0.02f));
                    path.LineTo(P(-0.25f, 0.2f)); path.Close();
                    canvas.DrawPath(path, p);
                }
                else if (d.Contains("mav") || d.Contains("dist"))
                {
                    canvas.DrawLine(P(0f, 0.95f).X, P(0f, 0.95f).Y, P(0f, -0.05f).X, P(0f, -0.05f).Y, p);
                    canvas.DrawCircle(cx, cy - r * 0.2f, r * 0.12f, p);
                    canvas.DrawArc(new SKRect(cx - r * 0.5f, cy - r * 0.7f, cx + r * 0.5f, cy + r * 0.3f), 225, 90, false, p);
                    canvas.DrawArc(new SKRect(cx - r * 0.9f, cy - r * 1.1f, cx + r * 0.9f, cy + r * 0.7f), 225, 90, false, p);
                }
            }
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e2)
        {
            var canvas = e2.Surface.Canvas;
            int w = e2.Info.Width;
            int h = e2.Info.Height;

            canvas.Clear(ToSk(this.BackColor));

            if (w < 24 || h < 24)
                return;

            Color nc = _numbercolor.A == 0 ? Color.FromArgb(150, 90, 235) : _numbercolor;
            Color fc = this.ForeColor.A == 0 ? Color.FromArgb(70, 55, 110) : this.ForeColor;

            string family = this.Font.FontFamily.Name;
            SKTypeface tfReg = GetTypeface(family, false);
            SKTypeface tfBold = GetTypeface(family, true);

            // ---- card: soft shadow, glass fill, light border ----
            var rect = new SKRect(3f, 3f, w - 4f, h - 5f);
            float rad = Math.Max(10f, Math.Min(w, h) * 0.14f);

            using (var sh = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                Color = new SKColor(140, 110, 220, 70),
                MaskFilter = SKMaskFilter.CreateBlur(SKBlurStyle.Normal, 4f)
            })
                canvas.DrawRoundRect(new SKRect(rect.Left + 1, rect.Top + 3, rect.Right - 1, rect.Bottom + 2), rad, rad, sh);

            using (var shader = SKShader.CreateLinearGradient(
                new SKPoint(0, rect.Top), new SKPoint(0, rect.Bottom),
                new[] { new SKColor(255, 255, 255), new SKColor(246, 242, 255) },
                null, SKShaderTileMode.Clamp))
            using (var fill = new SKPaint { IsAntialias = true, Style = SKPaintStyle.Fill, Shader = shader })
                canvas.DrawRoundRect(rect, rad, rad, fill);

            using (var line = new SKPaint
            {
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1.4f,
                Color = new SKColor(222, 212, 248)
            })
                canvas.DrawRoundRect(rect, rad, rad, line);

            // ---- icon + caption ----
            float iconSize = Math.Min(h * 0.22f, 22f);
            float iconX = rect.Left + 14f + iconSize / 2f;
            float iconY = rect.Top + 10f + iconSize / 2f;
            DrawIcon(canvas, desc, iconX, iconY, iconSize, new SKColor(125, 100, 205));

            float capX = iconX + iconSize / 2f + 9f;
            float capAvail = rect.Right - 8f - capX;
            float capSize = Math.Max(9f, Math.Min(h * 0.14f, 16f));

            using (var cap = new SKPaint { IsAntialias = true, Typeface = tfBold, Color = ToSk(fc) })
            {
                cap.TextSize = capSize;
                string d = desc ?? "";
                while (cap.MeasureText(d) > capAvail && capSize > 7f)
                {
                    capSize -= 0.5f;
                    cap.TextSize = capSize;
                }

                var fm = cap.FontMetrics;
                canvas.DrawText(d, capX, iconY - (fm.Ascent + fm.Descent) / 2f, cap);
            }

            // ---- big gradient number ----
            string numb = number.ToString(numberformat);
            float areaTop = iconY + iconSize / 2f + 4f;
            float areaBottom = rect.Bottom - 6f;
            float areaH = areaBottom - areaTop;

            using (var np = new SKPaint { IsAntialias = true, Typeface = tfBold })
            {
                float sz = Math.Max(10f, areaH * 0.95f);
                np.TextSize = sz;
                float maxW = (rect.Right - rect.Left) * 0.8f;
                while (np.MeasureText(numb) > maxW && sz > 10f)
                {
                    sz -= 1f;
                    np.TextSize = sz;
                }

                var fm = np.FontMetrics;
                float tw = np.MeasureText(numb);
                float x = (w - tw) / 2f;
                float by = areaTop + areaH / 2f - (fm.Ascent + fm.Descent) / 2f;

                using (var gs = SKShader.CreateLinearGradient(
                    new SKPoint(x, 0), new SKPoint(x + Math.Max(tw, 1f), 0),
                    new[] { ToSk(nc), EndColor(nc) }, null, SKShaderTileMode.Clamp))
                {
                    np.Shader = gs;
                    canvas.DrawText(numb, x, by, np);
                }
            }
        }

        public override void Refresh()
        {
            if (this.Visible)
                base.Refresh();
        }

        protected override void WndProc(ref Message m) // seems to crash here on linux... so try ignore it
        {
            try
            {
                base.WndProc(ref m);
            }
            catch { }
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            if (this.Visible && this.ThisReallyVisible())
                base.OnInvalidated(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }
    }
}