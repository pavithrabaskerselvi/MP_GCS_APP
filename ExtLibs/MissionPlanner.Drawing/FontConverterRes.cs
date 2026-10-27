using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Drawing
{
    public class ResourceFontConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext c, Type t)
        { return t == typeof(string) || base.CanConvertFrom(c, t); }

        public override object ConvertFrom(ITypeDescriptorContext c, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null) return base.ConvertFrom(c, culture, value);

            // e.g. "Microsoft Sans Serif, 8.25pt" or "Segoe UI, 9pt, style=Bold, Italic"
            var parts = s.Split(',');
            string name = parts[0].Trim();
            float size = 8.25f;
            var style = FontStyle.Regular;

            if (parts.Length > 1)
            {
                string sz = parts[1].Trim().ToLowerInvariant();
                foreach (var suffix in new[] { "pt", "px", "in", "mm" })
                    if (sz.EndsWith(suffix)) { sz = sz.Substring(0, sz.Length - suffix.Length); break; }
                float.TryParse(sz, NumberStyles.Float, CultureInfo.InvariantCulture, out size);
            }
            for (int i = 2; i < parts.Length; i++)
            {
                string st = parts[i].Trim();
                if (st.StartsWith("style=", StringComparison.OrdinalIgnoreCase)) st = st.Substring(6);
                FontStyle fs;
                if (Enum.TryParse(st, true, out fs)) style |= fs;
            }
            return new Font(name, size, style, GraphicsUnit.Point);
        }
    }

    public static class ResourceFontRegistration
    {
        private static bool done;
        public static void Register()
        {
            if (done) return;
            done = true;
            TypeDescriptor.AddAttributes(typeof(Font), new TypeConverterAttribute(typeof(ResourceFontConverter)));
        }
    }
}