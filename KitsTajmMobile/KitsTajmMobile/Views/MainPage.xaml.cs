using System.Threading.Tasks;
using KitsTajmMobile.Service;
using Xamarin.Forms;

namespace KitsTajmMobile.Views
{
    public partial class MainPage : TabbedPage
    {
        private readonly TimePage _timepage;
        private readonly ReportsPage _reportspage;

        public MainPage(IKitsTajmService service)
        {
            var timepage = new TimePage(service)
            {
                IsVisible = true
            };
            var reportspage = new ReportsPage(service);

            this._timepage = timepage;
            this._reportspage = reportspage;

            this.Children.Add(timepage);
            this.Children.Add(reportspage);
        }

        public async Task Load()
        {
            await this._timepage.Load();
        }
    }
}
