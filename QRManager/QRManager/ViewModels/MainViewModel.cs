namespace QRManager.ViewModels
{
    using System.Collections.Generic;
    public class MainViewModel
    {
        #region  ViewModels
        public LoginPageViewModel Login
        {
            get; set;

        }
        public QRGeneratorViewModel QRGenerator
        {
            get; set;
        }

        public RecoveryPasswordViewModel RecoPassword
        {
            get; set;
        }
        public ChangePasswordViewModel ChangePassword
        {
            get; set;
        }
        #endregion
        #region Constructors
        public MainViewModel()
        {
            this.Login = new LoginPageViewModel();
        }
        #endregion
        #region Singleton

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }
        #endregion
    }
}

