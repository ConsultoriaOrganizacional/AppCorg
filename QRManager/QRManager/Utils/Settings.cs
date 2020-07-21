using Plugin.Settings.Abstractions;
using Plugin.Settings;

namespace QRManager.Utils
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
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

        private const string LastEmailSettingsKey = "last_email_key";
        private const string LastPaswordSettingsKey = "last_password_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string LastUsedEmail
        {
            get
            {
                return AppSettings.GetValueOrDefault(LastEmailSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LastEmailSettingsKey, value);
            }
        }
        public static string LastPasword
        {
            get
            {
                return AppSettings.GetValueOrDefault(LastPaswordSettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LastPaswordSettingsKey, value);
            }
        }
    }
}

