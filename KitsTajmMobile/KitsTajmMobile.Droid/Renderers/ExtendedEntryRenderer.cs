using System.ComponentModel;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using KitsTajmMobile.Controls;
using KitsTajmMobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]

namespace KitsTajmMobile.Droid.Renderers
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (this.Control != null)
            {
                var entry = (ExtendedEntry)this.Element;
                var control = this.Control;

                var shape = new ShapeDrawable(new RectShape());
                shape.Paint.SetStyle(Paint.Style.Stroke);
                control.Background = shape;

                SetBorderColor(entry, control);
                SetBorderWidth(entry, control);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var entry = (ExtendedEntry)this.Element;
            var control = this.Control;

            switch (e.PropertyName)
            {
                case nameof(ExtendedEntry.BorderColor):
                    SetBorderColor(entry, control);
                    break;
                case nameof(ExtendedEntry.BorderWidth):
                    SetBorderWidth(entry, control);
                    break;
            }
        }

        private static void SetBorderColor(ExtendedEntry entry, EntryEditText control)
        {
            var shape = (ShapeDrawable)control.Background;

            shape.Paint.Color = entry.BorderColor.ToAndroid();
        }

        private static void SetBorderWidth(ExtendedEntry entry, EntryEditText control)
        {
            var shape = (ShapeDrawable)control.Background;

            shape.Paint.StrokeWidth = entry.BorderWidth;
        }
    }
}