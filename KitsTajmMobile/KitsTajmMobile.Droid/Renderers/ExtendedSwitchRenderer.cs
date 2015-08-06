using System.ComponentModel;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using KitsTajmMobile.Controls;
using KitsTajmMobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedSwitch), typeof(ExtendedSwitchRenderer))]

namespace KitsTajmMobile.Droid.Renderers
{
    public class ExtendedSwitchRenderer : SwitchRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                var entry = (ExtendedSwitch)this.Element;
                var control = this.Control;

                SetColors(entry, control);
                SetOffText(entry, control);
                SetOnText(entry, control);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var entry = (ExtendedSwitch)this.Element;
            var control = this.Control;

            switch (e.PropertyName)
            {
                case nameof(ExtendedSwitch.DisabledColor):
                case nameof(ExtendedSwitch.OffColor):
                case nameof(ExtendedSwitch.OnColor):
                    SetColors(entry, control);
                    break;
                case nameof(ExtendedSwitch.OffText):
                    SetOffText(entry, control);
                    break;
                case nameof(ExtendedSwitch.OnText):
                    SetOnText(entry, control);
                    break;
            }
        }

        private static void SetColors(ExtendedSwitch entry, Android.Widget.Switch control)
        {
            var track = (StateListDrawable)control.TrackDrawable;

            track.SetColorFilter(entry.TrackColor.ToAndroid(), PorterDuff.Mode.Multiply);

            //lollipop
            if (control.ThumbDrawable is AnimatedStateListDrawable)
            {
                var thumb = (AnimatedStateListDrawable)control.ThumbDrawable;
            }
            else
            {
                var thumb = new StateListDrawable();
                thumb.AddState(
                    new[] { Android.Resource.Attribute.StateChecked },
                    new ColorDrawable(entry.OnColor.ToAndroid()));
                thumb.AddState(
                    new[] { -Android.Resource.Attribute.StateEnabled },
                    new ColorDrawable(entry.DisabledColor.ToAndroid()));
                thumb.AddState(
                    new int[0],
                    new ColorDrawable(entry.OffColor.ToAndroid()));

                control.ThumbDrawable = thumb;
            }
        }

        private static void SetOffText(ExtendedSwitch entry, Android.Widget.Switch control)
        {
            control.TextOff = entry.OffText;
        }

        private static void SetOnText(ExtendedSwitch entry, Android.Widget.Switch control)
        {
            control.TextOn = entry.OnText;
        }
    }
}