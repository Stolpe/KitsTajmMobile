using System;
using KitsTajmMobile.Helpers;
using KitsTajmMobile.Service;
using KitsTajmMobile.ViewModels;
using Xamarin.Forms;

namespace KitsTajmMobile.Views
{
    public partial class LoginPage : ContentPage
    {
        private App _app;
        private IKitsTajmService _service;

        private LoginViewModel _Model => (LoginViewModel)this.Resources["model"];

        public LoginPage(IKitsTajmService service, App app)
        {
            this._app = app;
            this._service = service;

            InitializeComponent();
        }

        protected async void LoginButtonClicked(object sender, EventArgs args)
        {
            LoginResponse response;

            try
            {
                response = await this._service.Login(this._Model.UserName, this._Model.Password);
            }
            catch (System.Net.WebException e)
            {
                response = new LoginResponse
                {
                    Code = LoginResponse.LoginResponseCode.UnableToLogin,
                    Message = e.Status.ToString()
                };
            }

            var loggedin = response.Code == LoginResponse.LoginResponseCode.LoggedInOk;

            if (loggedin)
            {
                if (this._Model.SaveUserName == true)
                {
                    Settings.StoreUserName = true;
                    Settings.StoredUserName = this._Model.UserName;
                }
                else
                {
                    Settings.StoreUserName = false;
                }

                if (this._Model.SavePassword)
                {
                    Settings.StorePassword = true;
                    Settings.StoredPassword = this._Model.Password;
                }
                else
                {
                    Settings.StorePassword = false;
                }

                await this._app.ShowMainPage();
            }
            else
            {
                this._Model.ErrorMessage = response.Message;
            }
        }
    }
}
