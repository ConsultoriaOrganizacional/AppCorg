
namespace QRManager.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;
    using GalaSoft.MvvmLight.Views;
    using QRManager.Views;
    using RestSharp;
    using Newtonsoft.Json;
    using Acr.UserDialogs;
    using System.Threading.Tasks;
    public class RecoveryPasswordViewModel : ViewModelBase
    {
        #region Atributos
        private string email;
        #endregion
        #region Propiedades
        public string Email
        {
            get { return this.email; }
            set
            {
                SetValue(ref this.email, value);
            }
        }
        #endregion
    }
}
