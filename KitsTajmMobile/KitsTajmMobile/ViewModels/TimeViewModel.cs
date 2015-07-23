using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using KitsTajm.Helpers;
using KitsTajmMobile.Helpers;

namespace KitsTajmMobile.ViewModels
{
    public class TimeViewModel
    {
        private ConcurrentDictionary<YearMonthKey, MonthViewModel> _months;
        public IEnumerable<MonthViewModel> Months => this._months.Values;
        public ObservableCollection<WeekViewModel> Weeks { get; private set; }

        public TimeViewModel()
        {
            this._months = new ConcurrentDictionary<YearMonthKey, MonthViewModel>();

            var weeks = new ObservableCollection<WeekViewModel>();
            weeks.CollectionChanged += Weeks_CollectionChanged;

            this.Weeks = weeks;
        }

        private void Weeks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var week in e.NewItems.OfType<WeekViewModel>())
                    {
                        var yearmonths = week.Dates
                            .Select(date => new YearMonthKey
                            {
                                Year = date.Date.Year,
                                Month = date.Date.Month
                            })
                            .Distinct();

                        foreach (var yearmonth in yearmonths)
                        {
                            this._months.TryAdd(
                                yearmonth,
                                new MonthViewModel(yearmonth.Year, yearmonth.Month));
                        }

                        week.Rows.CollectionChanged += Rows_CollectionChanged;
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

        private void Rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    var daygroups = e.NewItems.OfType<WeekViewModel.RowViewModel>()
                        .SelectMany(row => row.Days)
                        .GroupBy(day => new
                        {
                            Year = day.Date.Year,
                            Month = day.Date.Month
                        });

                    foreach (var daygroup in daygroups)
                    {
                        var year = daygroup.Key.Year;
                        var month = daygroup.Key.Month;

                        this._months.AddOrUpdate(
                            new YearMonthKey
                            {
                                Year = year,
                                Month = month
                            },
                            key => //shouldn't happen
                            {
                                var value = new MonthViewModel(key.Year, key.Month);

                                foreach (var day in daygroup)
                                {
                                    value.Days.Add(day);
                                }

                                return value;
                            },
                            (key, oldvalue) =>
                            {
                                foreach (var day in daygroup)
                                {
                                    oldvalue.Days.Add(day);
                                }

                                return oldvalue;
                            });
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

        public class MonthViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<WeekViewModel.RowViewModel.DayViewModel> Days { get; private set; }
            public int Year { get; private set; }
            public int Month { get; private set; }
            public float Progress => (float)this.WorkedHours / this.WorkingHours;
            public string ProgressText => $"{this.WorkedHours.ToString()}/{this.WorkingHours.ToString()}";
            public int WorkingHours
            {
                get
                {
                    var from = new DateTime(this.Year, this.Month, 1);
                    var to = from.AddMonths(1);
                    var dates = Enumerable.Range(0, to.Subtract(from).Days)
                        .Select(offset => from.AddDays(offset));
                    var workingdays = dates.Count(date =>
                        HolidayHelper.IsHelgdag(date) == false
                     && HolidayHelper.IsHelgdagsafton(date) == false);

                    return workingdays * 8;
                }
            }
            public int WorkedHours => this.Days.Sum(day => day.Hours ?? 0);

            public event PropertyChangedEventHandler PropertyChanged;

            public MonthViewModel(int year, int monthnumber)
            {
                this.Year = year;
                this.Month = monthnumber;

                var days = new ObservableCollection<WeekViewModel.RowViewModel.DayViewModel>();
                days.CollectionChanged += Days_CollectionChanged;
                this.Days = days;
            }

            private void Days_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var day in e.NewItems.OfType<WeekViewModel.RowViewModel.DayViewModel>())
                        {
                            day.PropertyChanged += Day_PropertyChanged;
                        }

                        NotifyPropertyChanged(nameof(WorkedHours));
                        NotifyPropertyChanged(nameof(Progress));
                        NotifyPropertyChanged(nameof(ProgressText));
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

            private void Day_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case nameof(WeekViewModel.RowViewModel.DayViewModel.Hours):
                        NotifyPropertyChanged(nameof(WorkedHours));
                        NotifyPropertyChanged(nameof(Progress));
                        NotifyPropertyChanged(nameof(ProgressText));
                        break;
                }
            }

            private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private class YearMonthKey : IEquatable<YearMonthKey>
        {
            public int Year { get; set; }
            public int Month { get; set; }

            public bool Equals(YearMonthKey other)
            {
                if (other != null)
                {
                    if (this.Year.Equals(other.Year) && this.Month.Equals(other.Month))
                    {
                        return true;
                    }
                }

                return false;
            }

            public override bool Equals(object obj)
            {
                return this.Equals(obj as YearMonthKey);
            }

            public override int GetHashCode()
            {
                return this.Year ^ this.Month;
            }
        }
    }
}
