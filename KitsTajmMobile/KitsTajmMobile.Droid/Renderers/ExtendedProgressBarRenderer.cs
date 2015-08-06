using System.ComponentModel;
using Android.Graphics;
using Android.Graphics.Drawables;
using KitsTajmMobile.Controls;
using KitsTajmMobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedProgressBar), typeof(ExtendedProgressBarRenderer))]

namespace KitsTajmMobile.Droid.Renderers
{
    public class ExtendedProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                var entry = (ExtendedProgressBar)this.Element;
                var control = this.Control;

                SetProgressColor(entry, control);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var entry = (ExtendedProgressBar)this.Element;
            var control = this.Control;

            switch (e.PropertyName)
            {
                case nameof(ExtendedProgressBar.ProgressColor):
                    SetProgressColor(entry, control);
                    break;
            }
        }

        private static void SetProgressColor(ExtendedProgressBar entry, Android.Widget.ProgressBar control)
        {
            var layer = (LayerDrawable)control.ProgressDrawable;
            var unprogressed = (NinePatchDrawable)layer.GetDrawable(0);
            var progressed = (ScaleDrawable)layer.GetDrawable(2);

            unprogressed.SetColorFilter(entry.ProgressColor.ToAndroid(), PorterDuff.Mode.SrcIn);
            progressed.SetColorFilter(entry.ProgressColor.ToAndroid(), PorterDuff.Mode.SrcIn);
        }
    }
}