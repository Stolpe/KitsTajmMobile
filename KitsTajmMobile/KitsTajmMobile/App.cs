using System.Threading.Tasks;
using KitsTajmMobile.Service;
using KitsTajmMobile.Views;
using Xamarin.Forms;

namespace KitsTajmMobile
{
    public class App : Application
    {
        IKitsTajmService _service;

        public App()
        {
            this._service = new KitsTajmService();

            this.MainPage = new LoginPage(this._service, this);
        }

        public async Task ShowMainPage()
        {
            var timepage = new TimePage(this._service);

            this.MainPage = timepage;
            await timepage.Load();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
