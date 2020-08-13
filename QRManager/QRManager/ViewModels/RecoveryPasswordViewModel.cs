
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
    using System.Security.Cryptography.X509Certificates;
    using GalaSoft.MvvmLight.Helpers;

    public class RecoveryPasswordViewModel : ViewModelBase
    {
        #region Atributos
        private string email;
        private bool isRunning;
        #endregion
        #region Propiedades
        public class User
        {
            public string email { get; set; }
            
        }
       
        public string Email
        {
            get { return this.email; }
            set
            {
                SetValue(ref this.email, value);
            }
        }
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
            set
            {
                SetValue(ref this.isRunning, value);
            }
        }
        #endregion
        #region Contructors
        public RecoveryPasswordViewModel()
        {
            Email = this.email;
        }

        #endregion
        #region Commands

        public ICommand GeneratePassword => new Command(Recovery);
        public ICommand BackLogin => new Command(Back);


        public  void RecoveryPassowrd(string email)
        {
            var client = new RestClient("https://corgqr.herokuapp.com");
            var request = new RestRequest("/forgot", Method.POST);
            var yourobject = new User
            {
                email = email
         
        };

        var json = JsonConvert.SerializeObject(yourobject);
        request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            try
            {
                this.IsRunning = true;
                IRestResponse response = client.Execute(request);
            }
            catch
            {

                Application.Current.MainPage.DisplayAlert("Ok","","");
            }
        }

        private async void Recovery()
        {

            if (String.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El Correo es necesario",
                    "Accept");
                return;
            }

            this.isRunning = true;
            if (this.email.Contains("@consultoriaorganizacional.com"))
            {

                RecoveryPassowrd(this.Email);
            }
            else
            {
                RecoveryPassowrd(this.Email + "@consultoriaorganizacional.com");
            }

            this.IsRunning = false;
        }

        private async void Back()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }



        #endregion
    }
}
