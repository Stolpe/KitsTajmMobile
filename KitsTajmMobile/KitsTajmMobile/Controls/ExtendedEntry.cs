using Xamarin.Forms;

namespace KitsTajmMobile.Controls
{
    public class ExtendedEntry : Entry
    {
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create<ExtendedEntry, Color>(
            e => e.BorderColor,
            default(Color));
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create<ExtendedEntry, int>(
            e => e.BorderWidth,
            default(int));

        public Color BorderColor
        {
            get
            {
                return (Color)GetValue(BorderColorProperty);
            }
            set
            {
                SetValue(BorderColorProperty, value);
            }
        }

        public int BorderWidth
        {
            get
            {
                return (int)GetValue(BorderWidthProperty);
            }
            set
            {
                SetValue(BorderWidthProperty, value);
            }
        }
    }
}
