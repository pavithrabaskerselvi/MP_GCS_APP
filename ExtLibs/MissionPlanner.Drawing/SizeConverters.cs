using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Drawing
{
    public class ResourceSizeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext c, Type t)
        { return t == typeof(string) || base.CanConvertFrom(c, t); }

        public override object ConvertFrom(ITypeDescriptorContext c, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null) return base.ConvertFrom(c, culture, value);
            var p = s.Split(',');
            return new Size(int.Parse(p[0].Trim(), CultureInfo.InvariantCulture),
                            int.Parse(p[1].Trim(), CultureInfo.InvariantCulture));
        }
    }

    public class ResourcePointConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext c, Type t)
        { return t == typeof(string) || base.CanConvertFrom(c, t); }

        public override object ConvertFrom(ITypeDescriptorContext c, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null) return base.ConvertFrom(c, culture, value);
            var p = s.Split(',');
            return new Point(int.Parse(p[0].Trim(), CultureInfo.InvariantCulture),
                             int.Parse(p[1].Trim(), CultureInfo.InvariantCulture));
        }
    }

    public class ResourceSizeFConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext c, Type t)
        { return t == typeof(string) || base.CanConvertFrom(c, t); }

        public override object ConvertFrom(ITypeDescriptorContext c, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s == null) return base.ConvertFrom(c, culture, value);
            var p = s.Split(',');
            return new SizeF(float.Parse(p[0].Trim(), CultureInfo.InvariantCulture),
                             float.Parse(p[1].Trim(), CultureInfo.InvariantCulture));
        }
    }

    public static class ResourceConverterRegistration
    {
        private static bool done;

        public static void Register()
        {
            if (done) return;
            done = true;
            TypeDescriptor.AddAttributes(typeof(Size), new TypeConverterAttribute(typeof(ResourceSizeConverter)));
            TypeDescriptor.AddAttributes(typeof(Point), new TypeConverterAttribute(typeof(ResourcePointConverter)));
            TypeDescriptor.AddAttributes(typeof(SizeF), new TypeConverterAttribute(typeof(ResourceSizeFConverter)));
        }
    }
}