using Xamarin.Forms;

namespace KitsTajmMobile.Controls
{
    public class ExtendedProgressBar : ProgressBar
    {
        public static readonly BindableProperty ProgressColorProperty = BindableProperty.Create<ExtendedProgressBar, Color>(
            e => e.ProgressColor,
            default(Color));

        public Color ProgressColor
        {
            get
            {
                return (Color)GetValue(ProgressColorProperty);
            }
            set
            {
                SetValue(ProgressColorProperty, value);
            }
        }
    }
}
