using System.ComponentModel;
using System.Runtime.CompilerServices;
using KitsTajmMobile.Helpers;

namespace KitsTajmMobile.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _errormessage;
        private string _password;
        private bool _savepassword;
        private bool _saveusername;
        private string _username;

        public LoginViewModel()
        {
            this._savepassword = Settings.StorePassword;
            this._saveusername = Settings.StoreUserName;

            if (this._savepassword == true)
            {
                this._password = Settings.StoredPassword;
            }

            if (this._saveusername == true)
            {
                this.UserName = Settings.StoredUserName;
            }
        }

        public bool CanLogin
        {
            get
            {
                return string.IsNullOrEmpty(this.UserName) == false
                    && string.IsNullOrEmpty(this.Password) == false;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return this._errormessage;
            }
            set
            {
                if (value != this._errormessage)
                {
                    this._errormessage = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                if (value != this._password)
                {
                    this._password = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(CanLogin));
                }
            }
        }

        public bool SavePassword
        {
            get
            {
                return this.SaveUserName && this._savepassword;
            }
            set
            {
                if (value != this._savepassword)
                {
                    this._savepassword = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool SaveUserName
        {
            get
            {
                return this._saveusername;
            }
            set
            {
                if (value != this._saveusername)
                {
                    this._saveusername = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(SavePassword));
                }
            }
        }

        public string UserName
        {
            get
            {
                return this._username;
            }
            set
            {
                if (value != this._username)
                {
                    this._username = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged(nameof(CanLogin));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
