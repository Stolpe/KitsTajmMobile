using System;
using System.Globalization;
using Xamarin.Forms;

namespace KitsTajmMobile.Controls.TypeConverters
{
    public class EasingTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(CultureInfo culture, object value)
        {
            //http://developer.xamarin.com/api/type/Xamarin.Forms.Easing/#memberlist
            switch ((string)value)
            {
                case "BounceIn":
                    return Easing.BounceIn;
                case "BounceOut":
                    return Easing.BounceOut;
                case "CubicIn":
                    return Easing.CubicIn;
                case "CubicInOut":
                    return Easing.CubicInOut;
                case "CubicOut":
                    return Easing.CubicOut;
                case "Linear":
                    return Easing.Linear;
                case "SinIn":
                    return Easing.SinIn;
                case "SinInOut":
                    return Easing.SinInOut;
                case "SinOut":
                    return Easing.SinOut;
                case "SpringIn":
                    return Easing.SpringIn;
                case "SpringOut":
                    return Easing.SpringOut;
                default:
                    return default(Easing);
            }
        }
    }
}
