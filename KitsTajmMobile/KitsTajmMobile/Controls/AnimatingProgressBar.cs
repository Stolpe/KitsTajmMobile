using System;
using KitsTajmMobile.Controls.TypeConverters;
using Xamarin.Forms;

namespace KitsTajmMobile.Controls
{
    public class AnimatingProgressBar : ProgressBar
    {
        public static readonly BindableProperty AnimatedProgressProperty =
            BindableProperty.Create<AnimatingProgressBar, double>(p => p.AnimatedProgress, default(double), propertyChanged: OnAnimatedProgressChanged);
        public static readonly BindableProperty EasingProperty =
            BindableProperty.Create<AnimatingProgressBar, Easing>(p => p.Easing, default(Easing));
        public static readonly BindableProperty LengthProperty =
            BindableProperty.Create<AnimatingProgressBar, TimeSpan>(p => p.Length, default(TimeSpan));

        public double AnimatedProgress
        {
            get
            {
                return (double)GetValue(AnimatedProgressProperty);
            }
            set
            {
                SetValue(AnimatedProgressProperty, value);
            }
        }

        [TypeConverter(typeof(EasingTypeConverter))]
        public Easing Easing
        {
            get
            {
                return (Easing)GetValue(EasingProperty);
            }
            set
            {
                SetValue(EasingProperty, value);
            }
        }

        public TimeSpan Length
        {
            get
            {
                return (TimeSpan)GetValue(LengthProperty);
            }
            set
            {
                SetValue(LengthProperty, value);
            }
        }

        private static async void OnAnimatedProgressChanged(BindableObject bindable, double oldValue, double newValue)
        {
            var progressbar = (AnimatingProgressBar)(bindable);

            await progressbar.ProgressTo(
                newValue,
                (uint)progressbar.Length.TotalMilliseconds,
                progressbar.Easing);
        }
    }
}
