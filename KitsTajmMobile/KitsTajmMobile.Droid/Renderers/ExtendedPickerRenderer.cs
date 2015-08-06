using System.ComponentModel;
using Android.Widget;
using KitsTajmMobile.Controls;
using KitsTajmMobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedPicker), typeof(ExtendedPickerRenderer))]

namespace KitsTajmMobile.Droid.Renderers
{
    public class ExtendedPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                var entry = (ExtendedPicker)this.Element;
                var control = this.Control;

                SetTextColor(entry, control);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var entry = (ExtendedPicker)this.Element;
            var control = this.Control;

            switch (e.PropertyName)
            {
                case nameof(ExtendedPicker.TextColor):
                    SetTextColor(entry, control);
                    break;
            }
        }

        private static void SetTextColor(ExtendedPicker entry, EditText control)
        {
            control.SetTextColor(entry.TextColor.ToAndroid());
        }
    }
}