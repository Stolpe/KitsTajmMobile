using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KitsTajmMobile.Service;
using KitsTajmMobile.ViewModels;

namespace KitsTajmMobile.Helpers
{
    public class WeekModelRepository
    {
        private readonly LinkedList<WeekViewModel> _models;
        private readonly IKitsTajmService _service;
        private readonly TimeViewModel _timemodel;

        public WeekModelRepository(IKitsTajmService service, TimeViewModel timemodel)
        {
            this._models = new LinkedList<WeekViewModel>();
            this._service = service;
            this._timemodel = timemodel;
        }

        public WeekViewModel GetModel(DateTime date)
        {
            var monday = GetMonday(date);

            var model = this._models
                .FirstOrDefault(m => m.Monday.Date.Equals(monday));
            LinkedListNode<WeekViewModel> modelnode;

            if (model == null)
            {
                model = new WeekViewModel(monday, this._timemodel);
                modelnode = Add(model, this._models);
            }
            else
            {
                modelnode = this._models.Find(model);
            }

            PadModel(modelnode, this._timemodel);

            return model;
        }

        private static DateTime GetMonday(DateTime date)
        {
            var delta = DayOfWeek.Monday - date.DayOfWeek;
            if (delta > 0)
            {
                delta -= 7;
            }

            var monday = date.AddDays(delta);

            return monday.Date;
        }

        private static LinkedListNode<WeekViewModel> Add(
            WeekViewModel model,
            LinkedList<WeekViewModel> models)
        {
            LinkedListNode<WeekViewModel> node;

            if (models.Any() == false)
            {
                node = models.AddFirst(model);
            }
            else if (model.Monday.Date < models.First.Value.Monday.Date)
            {
                node = models.AddFirst(model);
            }
            else// if (model.Monday.Date > models.Last.Value.Monday.Date)
            {
                node = models.AddLast(model);
            }

            return node;
        }

        private static void PadModel(
            LinkedListNode<WeekViewModel> currentnode,
            TimeViewModel timemodel)
        {
            var models = currentnode.List;

            var currentmodel = currentnode.Value;
            var previousnode = currentnode.Previous;
            var nextnode = currentnode.Next;

            //pad adjacent models
            if (previousnode == null)
            {
                previousnode = models.AddBefore(
                    currentnode,
                    GetPreviousWeek(currentmodel, timemodel));
            }

            if (nextnode == null)
            {
                nextnode = models.AddAfter(
                    currentnode,
                    GetNextWeek(currentmodel, timemodel));
            }

            //pad rest of month(s)
            LinkedListNode<WeekViewModel> newpreviousnode = previousnode;
            LinkedListNode<WeekViewModel> newnextnode = nextnode;

            while (newpreviousnode.Previous == null
                && newpreviousnode.Value.Monday.Date.Month == previousnode.Value.Monday.Date.Month
                && newpreviousnode.Value.Monday.Date.Year == previousnode.Value.Monday.Date.Year)
            {
                newpreviousnode = models.AddBefore(
                    newpreviousnode,
                    GetPreviousWeek(
                        newpreviousnode.Value,
                        timemodel));
            }

            while (newnextnode.Next == null
                && newnextnode.Value.Sunday.Date.Month == nextnode.Value.Sunday.Date.Month
                && newnextnode.Value.Monday.Date.Year == nextnode.Value.Monday.Date.Year)
            {
                newnextnode = models.AddAfter(
                    newnextnode,
                    GetNextWeek(
                        newnextnode.Value,
                        timemodel));
            }
        }

        private static WeekViewModel GetPreviousWeek(
            WeekViewModel model,
            TimeViewModel timemodel)
        {
            return GetAnotherWeek(
                model,
                TimeSpan.FromDays(-7),
                timemodel);
        }

        private static WeekViewModel GetNextWeek(
            WeekViewModel model,
            TimeViewModel timemodel)
        {
            return GetAnotherWeek(
                model,
                TimeSpan.FromDays(7),
                timemodel);
        }

        private static WeekViewModel GetAnotherWeek(
            WeekViewModel model,
            TimeSpan span,
            TimeViewModel timemodel)
        {
            var newmodel = new WeekViewModel(
                model.Monday.Date.Add(span),
                timemodel);

            return newmodel;
        }

        public async Task Load()
        {
            await Task.WhenAll(
                this._models
                    .Where(model => model.TestAndSetIsLoaded() == false)
                    .Select(model => Load(model, this._service)));
        }

        private static async Task Load(WeekViewModel weekmodel, IKitsTajmService service)
        {
            var projects = await service.GetProjects();
            var trs = await service.GetTimeRecords(weekmodel.Monday.Date, weekmodel.Sunday.Date);
            var rows = trs
                .GroupBy(
                    tr => tr.ProjectId,
                    (pid, trs2) => new
                    {
                        Project = projects.FirstOrDefault(p => p.Id == pid),
                        Activities = trs2.GroupBy(
                            tr => tr.ActivityId,
                            (aid, trs3) => new
                            {
                                Activity = projects
                                    .Where(p => p.Id == pid)
                                    .SelectMany(p => p.Activities)
                                    .FirstOrDefault(a => a.Id == aid),
                                TimeRecords = trs3
                            })
                    })
                .OrderBy(pa => pa.Project.Name)
                .SelectMany(
                    pa => pa.Activities
                        .OrderBy(at => at.Activity.Name),
                    (pa, at) => new
                    {
                        Project = pa.Project,
                        Activity = at.Activity,
                        TimeRecords = at.TimeRecords
                    })
                    .Select((pat, rownumber) => new WeekViewModel.RowViewModel(
                        new WeekViewModel.RowViewModel.ProjectViewModel.ProjectViewModelParameters
                        {
                            SelectedItem = pat.Project
                        },
                        new WeekViewModel.RowViewModel.ActivityViewModel.ActivityViewModelParameters
                        {
                            SelectedItem = pat.Activity
                        },
                        pat.TimeRecords
                            .Where(tr => tr.ReportedDate.Date.Equals(weekmodel.Monday.Date))
                            .Select(tr => new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = tr.ReportedDate,
                                RecordId = tr.RecordId,
                                Text = tr.Time.ToString()
                            })
                            .FirstOrDefault()
                            ?? new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = weekmodel.Monday.Date
                            },
                        pat.TimeRecords
                            .Where(tr => tr.ReportedDate.Date.Equals(weekmodel.Tuesday.Date))
                            .Select(tr => new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = tr.ReportedDate,
                                RecordId = tr.RecordId,
                                Text = tr.Time.ToString()
                            })
                            .FirstOrDefault()
                            ?? new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = weekmodel.Tuesday.Date
                            },
                        pat.TimeRecords
                            .Where(tr => tr.ReportedDate.Date.Equals(weekmodel.Wednesday.Date))
                            .Select(tr => new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = tr.ReportedDate,
                                RecordId = tr.RecordId,
                                Text = tr.Time.ToString()
                            })
                            .FirstOrDefault()
                            ?? new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = weekmodel.Wednesday.Date
                            },
                        pat.TimeRecords
                            .Where(tr => tr.ReportedDate.Date.Equals(weekmodel.Thursday.Date))
                            .Select(tr => new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = tr.ReportedDate,
                                RecordId = tr.RecordId,
                                Text = tr.Time.ToString()
                            })
                            .FirstOrDefault()
                            ?? new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = weekmodel.Thursday.Date
                            },
                        pat.TimeRecords
                            .Where(tr => tr.ReportedDate.Date.Equals(weekmodel.Friday.Date))
                            .Select(tr => new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = tr.ReportedDate,
                                RecordId = tr.RecordId,
                                Text = tr.Time.ToString()
                            })
                            .FirstOrDefault()
                            ?? new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = weekmodel.Friday.Date
                            },
                        pat.TimeRecords
                            .Where(tr => tr.ReportedDate.Date.Equals(weekmodel.Saturday.Date))
                            .Select(tr => new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = tr.ReportedDate,
                                RecordId = tr.RecordId,
                                Text = tr.Time.ToString()
                            })
                            .FirstOrDefault()
                            ?? new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = weekmodel.Saturday.Date
                            },
                        pat.TimeRecords
                            .Where(tr => tr.ReportedDate.Date.Equals(weekmodel.Sunday.Date))
                            .Select(tr => new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = tr.ReportedDate,
                                RecordId = tr.RecordId,
                                Text = tr.Time.ToString()
                            })
                            .FirstOrDefault()
                            ?? new WeekViewModel.RowViewModel.DayViewModel.DayViewModelParameters
                            {
                                Date = weekmodel.Sunday.Date
                            },
                        weekmodel,
                        rownumber));

            weekmodel.Projects = projects
                .OrderBy(p => p.Name)
                .ToList();

            foreach (var row in rows)
            {
                weekmodel.Rows.Add(row);
            }
        }
    }
}
