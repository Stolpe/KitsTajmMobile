using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace KitsTajmMobile.Controls
{
    public class BindablePicker<T> : ExtendedPicker
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindablePicker<T>, IList<T>>(p => p.ItemsSource, default(IList<T>), propertyChanged: OnItemsSourceChanged);
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create<BindablePicker<T>, T>(o => o.SelectedItem, default(T), propertyChanged: OnSelectedItemChanged);

        public IList<T> ItemsSource
        {
            get
            {
                return (IList<T>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public T SelectedItem
        {
            get
            {
                return (T)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public Func<T, string> SourceItemLabelConverter { get; set; }

        public BindablePicker()
            : base()
        {
            this.SelectedIndexChanged += BindablePicker_SelectedIndexChanged;
        }

        private void BindablePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndex < 0)
            {
                this.SelectedItem = default(T);
            }
            else
            {
                this.SelectedItem = this.ItemsSource[this.SelectedIndex];
            }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, IList<T> oldvalue, IList<T> newvalue)
        {
            var picker = (BindablePicker<T>)bindable;

            if (oldvalue == null
                || newvalue == null 
                || oldvalue.SequenceEqual(newvalue) == false)
            {
                picker.Items.Clear();

                if (newvalue != null)
                {
                    foreach (var item in newvalue)
                    {
                        picker.Items.Add(picker.SourceItemLabelConverter?.Invoke(item) ?? item.ToString());
                    }
                }
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, T oldValue, T newValue)
        {
            var picker = (BindablePicker<T>)(bindable);

            if (newValue != null)
            {
                picker.SelectedIndex = picker.ItemsSource.IndexOf(newValue);
            }
            else
            {
                picker.SelectedIndex = -1;
            }
        }
    }
}
