using System;
using System.Threading.Tasks;
using KitsTajmMobile.Helpers;
using KitsTajmMobile.Service;
using KitsTajmMobile.ViewModels;
using Xamarin.Forms;

namespace KitsTajmMobile.Views
{
    public partial class TimePage : CarouselPage
    {
        private bool _initialized = false;

        private readonly IKitsTajmService _service;
        private readonly TimeViewModel _model;
        private readonly WeekModelRepository _weekmodels;

        private WeekPage CurrentWeekPage => (WeekPage)this.CurrentPage;

        public TimePage(IKitsTajmService service)
        {
            InitializeComponent();

            this._service = service;
            this._model = new TimeViewModel();
            this._weekmodels = new WeekModelRepository(this._service, this._model);

            var currentmodel = this._weekmodels.GetModel(DateTime.Now);
            var currentpage = new WeekPage(this._service, currentmodel);

            var previouspage = GetPreviousWeek(
                currentmodel,
                this._weekmodels,
                this._service);
            var nextpage = GetNextWeek(
                currentmodel,
                this._weekmodels,
                this._service);

            this.Children.Add(currentpage);
            this.Children.Insert(0, previouspage);
            this.Children.Add(nextpage);

            this._initialized = true;
        }

        public async Task Load()
        {
            await this._weekmodels.Load();
        }

        protected override async void OnCurrentPageChanged()
        {
            if (this._initialized == true)
            {
                var currentindex = this.Children.IndexOf(this.CurrentPage);

                //the first
                if (currentindex <= 1)
                {
                    var newpage = GetPreviousWeek(
                        this.CurrentWeekPage.Model,
                        this._weekmodels,
                        this._service);

                    this.Children.Insert(0, newpage);

                    await this._weekmodels.Load();
                }
                //...or last page was selected
                else if (currentindex >= this.Children.Count - 2)
                {
                    var newpage = GetNextWeek(
                        this.CurrentWeekPage.Model,
                        this._weekmodels,
                        this._service);

                    this.Children.Add(newpage);

                    await this._weekmodels.Load();
                }
            }                
        }

        private static WeekPage GetPreviousWeek(
            WeekViewModel currentmodel,
            WeekModelRepository weekmodels,
            IKitsTajmService service)
        {
            return GetAnotherWeek(
                currentmodel,
                weekmodels,
                service,
                TimeSpan.FromDays(-7));
        }

        private static WeekPage GetNextWeek(
            WeekViewModel currentmodel,
            WeekModelRepository weekmodels,
            IKitsTajmService service)
        {
            return GetAnotherWeek(
                currentmodel,
                weekmodels,
                service,
                TimeSpan.FromDays(7));
        }

        private static WeekPage GetAnotherWeek(
            WeekViewModel currentmodel,
            WeekModelRepository weekmodels,
            IKitsTajmService service,
            TimeSpan span)
        {
            var newpage = new WeekPage(
                service,
                weekmodels.GetModel(currentmodel.Monday.Date.Add(span)));

            return newpage;
        }
    }
}
