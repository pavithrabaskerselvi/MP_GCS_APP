using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace System.Drawing
{
    public class ResourceBitmapConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(byte[]) || sourceType == typeof(Stream) ||
                   base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                if (value is byte[] bytes) return new Bitmap(new MemoryStream(bytes));
                if (value is Stream stream) return new Bitmap(stream);
            }
            catch
            {
                return null;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

    public class ResourceIconConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(byte[]) || sourceType == typeof(Stream) ||
                   base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            try
            {
                if (value is byte[] bytes) return new Icon(new MemoryStream(bytes));
                if (value is Stream stream) return new Icon(stream);
            }
            catch
            {
                return null;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
