using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;

namespace KitsTajmMobile.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string _storedPasswordKey = "stored_password";
        private const string _storedPasswordDefault = null;
        private const string _storePasswordKey = "store_password";
        private const bool _storePasswordDefault = false;
        private const string _storedUserNameKey = "stored_username";
        private const string _storedUserNameDefault = null;
        private const string _storeUserNameKey = "store_username";
        private const bool _storeUserNameDefault = false;

        #endregion

        public static string StoredPassword
        {
            get
            {
                return AppSettings.GetValueOrDefault(Settings._storedPasswordKey, Settings._storedPasswordDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Settings._storedPasswordKey, value);
            }
        }

        public static bool StorePassword
        {
            get
            {
                return AppSettings.GetValueOrDefault(Settings._storePasswordKey, Settings._storePasswordDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Settings._storePasswordKey, value);

                if (value == false)
                {
                    AppSettings.Remove(Settings._storedPasswordKey);
                }
            }
        }

        public static string StoredUserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(Settings._storedUserNameKey, Settings._storedUserNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Settings._storedUserNameKey, value);
            }
        }

        public static bool StoreUserName
        {
            get
            {
                return AppSettings.GetValueOrDefault(Settings._storeUserNameKey, Settings._storeUserNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Settings._storeUserNameKey, value);

                if (value == false)
                {
                    AppSettings.Remove(Settings._storedUserNameKey);
                }
            }
        }
    }
}