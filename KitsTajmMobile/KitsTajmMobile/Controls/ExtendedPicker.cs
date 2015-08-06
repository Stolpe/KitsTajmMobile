using Xamarin.Forms;

namespace KitsTajmMobile.Controls
{
    public class ExtendedPicker : Picker
    {
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create<ExtendedPicker, Color>(
            e => e.TextColor,
            default(Color));

        public Color TextColor
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }
            set
            {
                SetValue(TextColorProperty, value);
            }
        }
    }
}
