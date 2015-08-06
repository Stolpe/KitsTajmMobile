using Xamarin.Forms;

namespace KitsTajmMobile.Controls
{
    public class ExtendedSwitch : Switch
    {
        public static readonly BindableProperty DisabledColorProperty = BindableProperty.Create<ExtendedSwitch, Color>(
            e => e.DisabledColor,
            default(Color));
        public static readonly BindableProperty OffColorProperty = BindableProperty.Create<ExtendedSwitch, Color>(
            e => e.OffColor,
            default(Color));
        public static readonly BindableProperty OffTextProperty = BindableProperty.Create<ExtendedSwitch, string>(
            e => e.OffText,
            string.Empty);
        public static readonly BindableProperty OnColorProperty = BindableProperty.Create<ExtendedSwitch, Color>(
            e => e.OnColor,
            default(Color));
        public static readonly BindableProperty OnTextProperty = BindableProperty.Create<ExtendedSwitch, string>(
            e => e.OnText,
            string.Empty);
        public static readonly BindableProperty TrackColorProperty = BindableProperty.Create<ExtendedSwitch, Color>(
            e => e.TrackColor,
            default(Color));

        public Color DisabledColor
        {
            get
            {
                return (Color)GetValue(DisabledColorProperty);
            }
            set
            {
                SetValue(DisabledColorProperty, value);
            }
        }

        public Color OffColor
        {
            get
            {
                return (Color)GetValue(OffColorProperty);
            }
            set
            {
                SetValue(OffColorProperty, value);
            }
        }

        public string OffText
        {
            get
            {
                return (string)GetValue(OffTextProperty);
            }
            set
            {
                SetValue(OffTextProperty, value);
            }
        }

        public Color OnColor
        {
            get
            {
                return (Color)GetValue(OnColorProperty);
            }
            set
            {
                SetValue(OnColorProperty, value);
            }
        }

        public string OnText
        {
            get
            {
                return (string)GetValue(OnTextProperty);
            }
            set
            {
                SetValue(OnTextProperty, value);
            }
        }

        public Color TrackColor
        {
            get
            {
                return (Color)GetValue(TrackColorProperty);
            }
            set
            {
                SetValue(TrackColorProperty, value);
            }
        }
    }
}
