using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using KitsTajmMobile.Controls;
using KitsTajmMobile.Converters;
using KitsTajmMobile.Service;
using KitsTajmMobile.ViewModels;
using Xamarin.Forms;

namespace KitsTajmMobile.Views
{
    public partial class WeekPage : ContentPage
    {
        private readonly IKitsTajmService _service;

        public WeekViewModel Model
        {
            get
            {
                return (WeekViewModel)this.Resources["Model"];
            }
            private set
            {
                this.Resources["Model"] = value;
            }
        }

        private Style _DayEntryStyle => (Style)this.Resources["DayEntryStyle"];
        private Style _ProjectAndActivityPickerStyle => (Style)this.Resources["ProjectAndActivityPickerStyle"];
        private Style _MonthProgressMonthLabelStyle => (Style)this.Resources["MonthProgressMonthLabelStyle"];
        private Style _MonthProgressbarStyle => (Style)this.Resources["MonthProgressbarStyle"];
        private Style _MonthProgressbarTextStyle => (Style)this.Resources["MonthProgressbarTextStyle"];
        private int _RowFirstProject => (int)this.Resources["RowFirstProject"];
        private int _ColumnProject => (int)this.Resources["ColumnProject"];
        private int _ColumnActivity => (int)this.Resources["ColumnActivity"];
        private int _ColumnMonday => (int)this.Resources["ColumnMonday"];
        private int _ColumnTuesday => (int)this.Resources["ColumnTuesday"];
        private int _ColumnWednesday => (int)this.Resources["ColumnWednesday"];
        private int _ColumnThursday => (int)this.Resources["ColumnThursday"];
        private int _ColumnFriday => (int)this.Resources["ColumnFriday"];
        private int _ColumnSaturday => (int)this.Resources["ColumnSaturday"];
        private int _ColumnSunday => (int)this.Resources["ColumnSunday"];

        public WeekPage(IKitsTajmService service, WeekViewModel model)
        {
            this._service = service;

            InitializeComponent();

            this.Model = model;
            this.Model.Rows.CollectionChanged += this.Rows_CollectionChanged;

            foreach (var rowmodel in this.Model.Rows)
            {
                DrawRow(rowmodel);
            }

            foreach (var month in this.Model.Months.Select((m, i) => new { Month = m, Row = i }))
            {
                DrawMonthProgress(month.Month, month.Row);
            }
        }

        private void DrawMonthProgress(TimeViewModel.MonthViewModel month, int row)
        {
            string monthname = null;

            switch (month.Month)
            {
                case 1:
                    monthname = "januari";
                    break;
                case 2:
                    monthname = "februari";
                    break;
                case 3:
                    monthname = "mars";
                    break;
                case 4:
                    monthname = "april";
                    break;
                case 5:
                    monthname = "maj";
                    break;
                case 6:
                    monthname = "juni";
                    break;
                case 7:
                    monthname = "juli";
                    break;
                case 8:
                    monthname = "augusti";
                    break;
                case 9:
                    monthname = "september";
                    break;
                case 10:
                    monthname = "oktober";
                    break;
                case 11:
                    monthname = "november";
                    break;
                case 12:
                    monthname = "december";
                    break;
            }

            var monthlabel = new Label
            {
                BindingContext = month,
                Style = this._MonthProgressMonthLabelStyle,
                Text = monthname
            };
            var progressbar = new AnimatingProgressBar
            {
                BindingContext = month,
                Style = this._MonthProgressbarStyle
            };
            var progresslabel = new Label
            {
                BindingContext = month,
                Style = this._MonthProgressbarTextStyle
            };

            progressbar.SetBinding<TimeViewModel.MonthViewModel>(
                AnimatingProgressBar.AnimatedProgressProperty,
                m => m.Progress);
            progresslabel.SetBinding<TimeViewModel.MonthViewModel>(
                Label.TextProperty,
                m => m.ProgressText);

            this.MonthsGrid.Children.Add(monthlabel, 0, row);
            this.MonthsGrid.Children.Add(progressbar, 1, row);
            this.MonthsGrid.Children.Add(progresslabel, 1, row);
        }

        private void Rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var rowmodel in e.NewItems.OfType<WeekViewModel.RowViewModel>())
                    {
                        DrawRow(rowmodel);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        private void DrawRow(WeekViewModel.RowViewModel rowmodel)
        {
            var projectpicker = GetPicker<ProjectResponse, WeekViewModel.RowViewModel.ProjectViewModel>(
                rowmodel.Project,
                this._ProjectAndActivityPickerStyle,
                p => p.Name,
                this._ColumnProject,
                this._RowFirstProject);
            var activitypicker = GetPicker<ProjectResponse.Activity, WeekViewModel.RowViewModel.ActivityViewModel>(
                rowmodel.Activity,
                this._ProjectAndActivityPickerStyle,
                a => a.Name,
                this._ColumnActivity,
                this._RowFirstProject);

            var mondayentry = GetDayEntry(
                rowmodel.Monday,
                this._DayEntryStyle,
                this._ColumnMonday,
                this._RowFirstProject);
            var tuesdayentry = GetDayEntry(
                rowmodel.Tuesday,
                this._DayEntryStyle,
                this._ColumnTuesday,
                this._RowFirstProject);
            var wednesdayentry = GetDayEntry(
                rowmodel.Wednesday,
                this._DayEntryStyle,
                this._ColumnWednesday,
                this._RowFirstProject);
            var thursdayentry = GetDayEntry(
                rowmodel.Thursday,
                this._DayEntryStyle,
                this._ColumnThursday,
                this._RowFirstProject);
            var fridayentry = GetDayEntry(
                rowmodel.Friday,
                this._DayEntryStyle,
                this._ColumnFriday,
                this._RowFirstProject);
            var saturdayentry = GetDayEntry(
                rowmodel.Saturday,
                this._DayEntryStyle,
                this._ColumnSaturday,
                this._RowFirstProject);
            var sundayentry = GetDayEntry(
                rowmodel.Sunday,
                this._DayEntryStyle,
                this._ColumnSunday,
                this._RowFirstProject);

            this.WeekGrid.Children.Add(projectpicker);
            this.WeekGrid.Children.Add(activitypicker);
            this.WeekGrid.Children.Add(mondayentry);
            this.WeekGrid.Children.Add(tuesdayentry);
            this.WeekGrid.Children.Add(wednesdayentry);
            this.WeekGrid.Children.Add(thursdayentry);
            this.WeekGrid.Children.Add(fridayentry);
            this.WeekGrid.Children.Add(saturdayentry);
            this.WeekGrid.Children.Add(sundayentry);

            rowmodel.Activity.PropertyChanged += Activity_PropertyChanged;

            foreach (var day in rowmodel.Days)
            {
                day.PropertyChanged += DayModel_PropertyChanged;
            }
        }

        private static BindablePicker<TPicker> GetPicker<TPicker, TModel>(
            TModel model,
            Style style,
            Func<TPicker, string> labelConverter,
            int column,
            int rowoffset)
            where TPicker : class
            where TModel : WeekViewModel.RowViewModel.SelectorViewModel<TPicker>
        {
            var picker = new BindablePicker<TPicker>
            {
                BindingContext = model,
                SourceItemLabelConverter = labelConverter,
                Style = style
            };

            picker.SetBinding<TModel>(
                BindablePicker<TPicker>.IsEnabledProperty,
                m => m.IsEnabled);
            picker.SetBinding<TModel>(
                BindablePicker<TPicker>.ItemsSourceProperty,
                m => m.Items);
            picker.SetBinding<TModel>(
                BindablePicker<TPicker>.SelectedItemProperty,
                m => m.SelectedItem,
                BindingMode.TwoWay);
            picker.SetValue(Grid.ColumnProperty, column);
            picker.SetBinding<TModel>(
                Grid.RowProperty,
                m => m.Row,
                converter: new RowNumberToGridNumberConverter
                {
                    Offset = rowoffset
                });

            return picker;
        }

        private static Entry GetDayEntry(
            WeekViewModel.RowViewModel.DayViewModel model,
            Style style,
            int column,
            int rowoffset)
        {
            var dayentry = new Entry
            {
                BindingContext = model,
                Style = style
            };

            dayentry.SetBinding<WeekViewModel.RowViewModel.DayViewModel>(
                Entry.IsEnabledProperty,
                m => m.IsEnabled);
            dayentry.SetBinding<WeekViewModel.RowViewModel.DayViewModel>(
                Entry.IsFocusedProperty,
                m => m.IsFocused);
            dayentry.SetBinding<WeekViewModel.RowViewModel.DayViewModel>(
                Entry.TextProperty,
                m => m.Text,
                BindingMode.TwoWay);
            dayentry.SetValue(
                Grid.ColumnProperty,
                column);
            dayentry.SetBinding<WeekViewModel.RowViewModel.DayViewModel>(
                Grid.RowProperty,
                m => m.Row,
                converter: new RowNumberToGridNumberConverter
                {
                    Offset = rowoffset
                });

            return dayentry;
        }

        private async void Activity_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var model = (WeekViewModel.RowViewModel.ActivityViewModel)sender;

            switch (e.PropertyName)
            {
                case nameof(WeekViewModel.RowViewModel.ActivityViewModel.SelectedItem):
                    await Task.WhenAll(
                        model.ParentRow.Days
                            .Select(day => SaveDay(day, this._service)));
                    break;
            }
        }

        private async void DayModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var model = (WeekViewModel.RowViewModel.DayViewModel)sender;

            switch (e.PropertyName)
            {
                case nameof(WeekViewModel.RowViewModel.DayViewModel.IsFocused):
                    await SaveDay(model, this._service);
                    break;
            }
        }

        private static async Task SaveDay(WeekViewModel.RowViewModel.DayViewModel model, IKitsTajmService service)
        {
            if (model.IsFocused == false && model.HasChanged)
            {
                if (model.Hours > 0 && model.ParentRow.Activity.SelectedItem != null)
                {
                    model.Save();

                    if (model.RecordId != null)
                    {
                        var response = await service.PutTimeRecord(
                            model.Date,
                            model.Hours.Value,
                            model.ParentRow.Project.SelectedItem,
                            model.ParentRow.Activity.SelectedItem,
                            model.RecordId.Value);

                        if (response.IsSuccess == false)
                        {
                            throw new Exception("this is fancy error handling");
                        }
                    }
                    else
                    {
                        var response = await service.PostNewTimeRecord(
                            model.Date,
                            model.Hours.Value,
                            model.ParentRow.Project.SelectedItem,
                            model.ParentRow.Activity.SelectedItem);

                        if (response.IsSuccess)
                        {
                            model.RecordId = response.RecordId;
                        }
                        else
                        {
                            throw new Exception("Unexpected result from PostNewTimeRecord");
                        }
                    }

                    model.Saved();
                }
                else if (model.RecordId != null)
                {
                    model.Save();

                    var response = await service.DeleteTimeRecord(model.Date, model.RecordId.Value);

                    if (response.IsSuccess)
                    {
                        model.RecordId = null;
                    }
                    else
                    {
                        throw new Exception("this is fancy error handling");
                    }

                    model.Saved();
                }
            }
        }
    }
}
