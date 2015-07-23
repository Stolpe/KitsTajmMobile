using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using KitsTajmMobile.Helpers;
using KitsTajmMobile.Service;

namespace KitsTajmMobile.ViewModels
{
    public class WeekViewModel : INotifyPropertyChanged
    {
        private TimeViewModel _parenttime;
        private IList<ProjectResponse> _projects;
        private bool _loaded = false;

        public DateViewModel Monday { get; private set; }
        public DateViewModel Tuesday { get; private set; }
        public DateViewModel Wednesday { get; private set; }
        public DateViewModel Thursday { get; private set; }
        public DateViewModel Friday { get; private set; }
        public DateViewModel Saturday { get; private set; }
        public DateViewModel Sunday { get; private set; }
        public IList<ProjectResponse> Projects
        {
            get
            {
                return this._projects;
            }
            set
            {
                if (this._projects != value)
                {
                    this._projects = value;

                    NotifyPropertyChanged();
                    AddEmptyRow();
                }
            }
        }
        public ObservableCollection<RowViewModel> Rows { get; private set; }

        public IEnumerable<DateViewModel> Dates
        {
            get
            {
                yield return this.Monday;
                yield return this.Tuesday;
                yield return this.Wednesday;
                yield return this.Thursday;
                yield return this.Friday;
                yield return this.Saturday;
                yield return this.Sunday;
            }
        }
        public int WeekNumber
        {
            get
            {
                //http://blogs.msdn.com/b/shawnste/archive/2006/01/24/iso-8601-week-of-year-format-in-microsoft-net.aspx
                return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(this.Thursday.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
        }
        public int WeekWorkedHours => this.Rows.Sum(row => row.WorkedHours);
        public int WeekWorkingHours => this.Dates.Count(date => date.Type == DateViewModel.DateType.WorkDay) * 8;
        public float WeekProgress => (float)this.WeekWorkedHours / this.WeekWorkingHours;
        public string WeekProgressText => $"{this.WeekWorkedHours.ToString()}/{this.WeekWorkingHours.ToString()}";

        private bool _HasEmptyRow => this.Rows.Any(row => row.IsEmpty);

        public IEnumerable<TimeViewModel.MonthViewModel> Months
        {
            get
            {
                var keys = this.Dates
                    .Select(date => new
                    {
                        Year = date.Date.Year,
                        Month = date.Date.Month
                    })
                    .Distinct();

                foreach (var key in keys)
                {
                    yield return this._parenttime.Months.FirstOrDefault(m => m.Year == key.Year && m.Month == key.Month);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public WeekViewModel(DateTime monday, TimeViewModel timemodel)
        {
            this.Monday = new DateViewModel(monday.AddDays(0));
            this.Tuesday = new DateViewModel(monday.AddDays(1));
            this.Wednesday = new DateViewModel(monday.AddDays(2));
            this.Thursday = new DateViewModel(monday.AddDays(3));
            this.Friday = new DateViewModel(monday.AddDays(4));
            this.Saturday = new DateViewModel(monday.AddDays(5));
            this.Sunday = new DateViewModel(monday.AddDays(6));

            var rows = new ObservableCollection<RowViewModel>();
            rows.CollectionChanged += Rows_CollectionChanged;
            this.Rows = rows;

            this._parenttime = timemodel;
            this._parenttime.Weeks.Add(this);
        }

        public bool TestAndSetIsLoaded()
        {
            if (this._loaded == true)
            {
                return true;
            }
            else
            {
                this._loaded = true;

                return false;
            }
        }

        private void AddEmptyRow()
        {
            if (this._HasEmptyRow == false)
            {
                var row = new WeekViewModel.RowViewModel(
                new WeekViewModel.RowViewModel.ProjectViewModel.ProjectViewModelParameters
                {
                    SelectedItem = null,
                },
                new WeekViewModel.RowViewModel.ActivityViewModel.ActivityViewModelParameters
                {
                    SelectedItem = null
                },
                new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                {
                    Date = this.Monday.Date
                },
                new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                {
                    Date = this.Tuesday.Date
                },
                new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                {
                    Date = this.Wednesday.Date
                },
                new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                {
                    Date = this.Thursday.Date
                },
                new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                {
                    Date = this.Friday.Date
                },
                new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                {
                    Date = this.Saturday.Date
                },
                new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                {
                    Date = this.Sunday.Date
                },
                this);

                this.Rows.Add(row);
            }
        }

        private void Rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var row in e.NewItems.OfType<RowViewModel>())
                    {
                        row.PropertyChanged += Row_PropertyChanged;
                    }

                    NotifyPropertyChanged(nameof(WeekProgress));
                    NotifyPropertyChanged(nameof(WeekProgressText));
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

        private void Row_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(RowViewModel.IsEmpty):
                    if (this._HasEmptyRow == false)
                    {
                        AddEmptyRow();
                    }
                    break;
                case nameof(RowViewModel.WorkedHours):
                    NotifyPropertyChanged(nameof(WeekWorkedHours));
                    NotifyPropertyChanged(nameof(WeekProgress));
                    NotifyPropertyChanged(nameof(WeekProgressText));
                    break;
            }
        }

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public class DateViewModel
        {
            public DateTime Date { get; private set; }
            public DateType Type { get; private set; }

            public DateViewModel(DateTime date)
            {
                this.Date = date.Date;

                if (HolidayHelper.IsHelgdag(this.Date))
                {
                    this.Type = DateType.Helgdag;
                }
                else if (HolidayHelper.IsHelgdagsafton(this.Date))
                {
                    this.Type = DateType.Helgdagsafton;
                }
                else
                {
                    this.Type = DateType.WorkDay;
                }
            }

            public enum DateType
            {
                WorkDay,
                Helgdagsafton,
                Helgdag
            };
        }

        public class RowViewModel : INotifyPropertyChanged
        {
            private int _dayssaving = 0;
            private readonly WeekViewModel _weekmodel;
            private int? _rownumber = null;

            public ProjectViewModel Project { get; private set; }
            public ActivityViewModel Activity { get; private set; }
            public DayViewModel Monday { get; private set; }
            public DayViewModel Tuesday { get; private set; }
            public DayViewModel Wednesday { get; private set; }
            public DayViewModel Thursday { get; private set; }
            public DayViewModel Friday { get; private set; }
            public DayViewModel Saturday { get; private set; }
            public DayViewModel Sunday { get; private set; }
            public int RowNumber
            {
                get
                {
                    return this._rownumber ?? this._weekmodel.Rows.Count - 1;
                }
                set
                {
                    if (this._rownumber != value)
                    {
                        this._rownumber = value;

                        NotifyPropertyChanged();
                    }
                }
            }

            public IEnumerable<DayViewModel> Days
            {
                get
                {
                    yield return this.Monday;
                    yield return this.Tuesday;
                    yield return this.Wednesday;
                    yield return this.Thursday;
                    yield return this.Friday;
                    yield return this.Saturday;
                    yield return this.Sunday;
                }
            }

            public bool DaySaving
            {
                get
                {
                    return this._dayssaving > 0;
                }
            }
            public bool IsEmpty
            {
                get
                {
                    return this.Project.SelectedItem == null;
                }
            }
            public int WorkedHours
            {
                get
                {
                    return this.Days.Sum(day => day.Hours) ?? 0;
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public RowViewModel(
                ProjectViewModel.ProjectViewModelParameters project,
                ActivityViewModel.ActivityViewModelParameters activity,
                DayViewModel.DayViewModelParameters monday,
                DayViewModel.DayViewModelParameters tuesday,
                DayViewModel.DayViewModelParameters wednesday,
                DayViewModel.DayViewModelParameters thursday,
                DayViewModel.DayViewModelParameters friday,
                DayViewModel.DayViewModelParameters saturday,
                DayViewModel.DayViewModelParameters sunday,
                WeekViewModel parentWeek,
                int? rownumber = null)
            {
                this._weekmodel = parentWeek;
                this._rownumber = rownumber;

                this.Project = new ProjectViewModel(project, this);
                this.Activity = new ActivityViewModel(activity, this);
                this.Monday = new DayViewModel(monday, this);
                this.Tuesday = new DayViewModel(tuesday, this);
                this.Wednesday = new DayViewModel(wednesday, this);
                this.Thursday = new DayViewModel(thursday, this);
                this.Friday = new DayViewModel(friday, this);
                this.Saturday = new DayViewModel(saturday, this);
                this.Sunday = new DayViewModel(sunday, this);

                this.Project.PropertyChanged += Project_PropertyChanged;

                foreach (var day in this.Days)
                {
                    day.PropertyChanged += Day_PropertyChanged;
                }

                this._weekmodel.Rows.CollectionChanged += Rows_CollectionChanged;
            }

            public void Save()
            {
                this._dayssaving++;

                NotifyPropertyChanged(nameof(DaySaving));
            }

            public void Saved()
            {
                this._dayssaving--;

                NotifyPropertyChanged(nameof(DaySaving));
            }

            private void Project_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case nameof(ProjectViewModel.SelectedItem):
                        this.RowNumber = this._weekmodel.Rows.Count - 1;

                        NotifyPropertyChanged(nameof(IsEmpty));
                        break;
                }
            }

            private void Day_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case nameof(DayViewModel.Hours):
                        NotifyPropertyChanged(nameof(WorkedHours));
                        break;
                }
            }

            private void Rows_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        NotifyPropertyChanged(nameof(RowNumber));
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

            protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public class ProjectViewModel : SelectorViewModel<ProjectResponse>
            {
                public override IList<ProjectResponse> Items
                {
                    get
                    {
                        return this.ParentRow._weekmodel.Projects;
                    }
                }
                public override bool IsEnabled => this.ParentRow.DaySaving == false;

                public ProjectViewModel(ProjectViewModelParameters parameters, RowViewModel parentrow)
                    : base(parameters, parentrow)
                {
                    parentrow._weekmodel.PropertyChanged += Weekmodel_PropertyChanged;
                }

                private void Weekmodel_PropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    switch (e.PropertyName)
                    {
                        case nameof(WeekViewModel.Projects):
                            NotifyPropertyChanged(nameof(Items));
                            break;
                    }
                }

                public class ProjectViewModelParameters : SelectorViewModel<ProjectResponse>.SelectorViewModelParameters
                { }
            }

            public class ActivityViewModel : SelectorViewModel<ProjectResponse.Activity>
            {
                public override IList<ProjectResponse.Activity> Items
                {
                    get
                    {
                        return this.ParentRow.Project.SelectedItem?.Activities
                            .OrderBy(a => a.Name)
                            .ToList();
                    }
                }
                public override bool IsEnabled
                {
                    get
                    {
                        return this.ParentRow.DaySaving == false && this.ParentRow.Project.SelectedItem != null;
                    }
                }

                public ActivityViewModel(ActivityViewModelParameters parameters, RowViewModel parentrow)
                    : base(parameters, parentrow)
                {
                    parentrow.Project.PropertyChanged += Project_PropertyChanged;
                }

                private void Project_PropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    switch (e.PropertyName)
                    {
                        case nameof(ProjectViewModel.SelectedItem):
                            NotifyPropertyChanged(nameof(Items));
                            NotifyPropertyChanged(nameof(IsEnabled));
                            break;
                    }
                }

                public class ActivityViewModelParameters : SelectorViewModel<ProjectResponse.Activity>.SelectorViewModelParameters
                { }
            }

            public abstract class SelectorViewModel<T> : INotifyPropertyChanged
                where T : class
            {
                private T _selecteditem;

                public abstract IList<T> Items { get; }
                public T SelectedItem
                {
                    get
                    {
                        return this._selecteditem;
                    }
                    set
                    {
                        if (this._selecteditem != value)
                        {
                            this._selecteditem = value;

                            NotifyPropertyChanged();
                        }
                    }
                }
                public abstract bool IsEnabled { get; }
                public RowViewModel ParentRow { get; private set; }
                public int Row => this.ParentRow.RowNumber;

                public event PropertyChangedEventHandler PropertyChanged;

                protected SelectorViewModel(SelectorViewModelParameters parameters, RowViewModel parentrow)
                {
                    this.SelectedItem = parameters.SelectedItem;

                    parentrow.PropertyChanged += Parentrow_PropertyChanged;
                    this.ParentRow = parentrow;
                }

                private void Parentrow_PropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    switch (e.PropertyName)
                    {
                        case nameof(RowViewModel.DaySaving):
                            NotifyPropertyChanged(nameof(IsEnabled));
                            break;
                        case nameof(RowViewModel.RowNumber):
                            NotifyPropertyChanged(nameof(Row));
                            break;
                    }
                }

                protected void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }

                public abstract class SelectorViewModelParameters
                {
                    public T SelectedItem { get; set; }
                }
            }

            public class DayViewModel : INotifyPropertyChanged
            {
                private bool _saving = false;
                private bool _isfocused = default(bool);
                private ProjectResponse.Activity _previousactivity;
                private int? _previoushours;
                private string _text = string.Empty;

                public DateTime Date { get; private set; }
                public bool HasChanged
                {
                    get
                    {
                        return this._previoushours.Equals(this.Hours) == false
                            || this._previousactivity != this.ParentRow.Activity.SelectedItem;
                    }
                }
                public int? Hours
                {
                    get
                    {
                        int result;

                        if (int.TryParse(this.Text, out result))
                        {
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
                public bool IsEnabled
                {
                    get
                    {
                        return this._saving == false && this.ParentRow.Activity.SelectedItem != null;
                    }
                }
                public bool IsFocused
                {
                    get
                    {
                        return this._isfocused;
                    }
                    set
                    {
                        if (this._isfocused != value)
                        {
                            this._isfocused = value;

                            NotifyPropertyChanged();
                        }
                    }
                }
                public int? RecordId { get; set; }
                public RowViewModel ParentRow { get; private set; }
                public int Row => this.ParentRow.RowNumber;
                public string Text
                {
                    get
                    {
                        return this._text;
                    }
                    set
                    {
                        if (this._text != (value ?? string.Empty))
                        {
                            this._text = value;

                            NotifyPropertyChanged();
                            NotifyPropertyChanged(nameof(Hours));
                            NotifyPropertyChanged(nameof(IsValid));
                        }
                    }
                }
                public bool IsValid
                {
                    get
                    {
                        if (string.IsNullOrWhiteSpace(this.Text) || this.Hours >= 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                public event PropertyChangedEventHandler PropertyChanged;

                public DayViewModel(DayViewModelParameters parameters, RowViewModel parentrow)
                {
                    this.Date = parameters.Date;
                    this.RecordId = parameters.RecordId;
                    this.Text = parameters.Text;

                    parentrow.PropertyChanged += Parentrow_PropertyChanged;

                    parentrow.Activity.PropertyChanged += Activity_PropertyChanged;
                    this.ParentRow = parentrow;

                    this._previousactivity = this.ParentRow.Activity.SelectedItem;
                    this._previoushours = this.Hours;
                }

                public void Save()
                {
                    this._previousactivity = this.ParentRow.Activity.SelectedItem;
                    this._previoushours = this.Hours;
                    this._saving = true;

                    NotifyPropertyChanged(nameof(IsEnabled));

                    this.ParentRow.Save();
                }

                public void Saved()
                {
                    this._saving = false;

                    NotifyPropertyChanged(nameof(IsEnabled));

                    this.ParentRow.Saved();
                }

                private void Parentrow_PropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    switch (e.PropertyName)
                    {
                        case nameof(RowViewModel.RowNumber):
                            NotifyPropertyChanged(nameof(Row));
                            break;
                    }
                }

                private void Activity_PropertyChanged(object sender, PropertyChangedEventArgs e)
                {
                    switch (e.PropertyName)
                    {
                        case nameof(ActivityViewModel.SelectedItem):
                            NotifyPropertyChanged(nameof(IsEnabled));
                            break;
                    }
                }

                private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }

                public class DayViewModelParameters
                {
                    public DateTime Date { get; set; }
                    public int? RecordId { get; set; }
                    public string Text { get; set; }
                }
            }
        }
    }
}
