
namespace QRManager.ViewModels
{
    using Newtonsoft.Json;
    using QRManager.Views;
    using RestSharp;
    using System;
    using System.Net;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Acr.UserDialogs;
    using System.Threading.Tasks;
    using GalaSoft.MvvmLight;

    public class RecoveryPasswordViewModel : ViewModelBase
    {
        #region Atributos
        private string email;
        bool isBusy = false;
        private bool isEnabled;
        private bool isVisible;
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
        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        public bool IsVisible
        {
            get { return this.isVisible; }
            set { SetValue(ref this.isVisible, value); }
        }
        #endregion
        #region Contructors
        public RecoveryPasswordViewModel()
        {
            Email = this.email;
        }
        public bool IsBusy

        {
            get { return isBusy; }

            set { SetProperty(ref isBusy, value); }

        }
        #endregion
        #region Commands

        public ICommand GeneratePassword => new Command(Recovery);
        public ICommand BackLogin => new Command(Back);
         

        public  void RecoveryPassowrd(string email)
        {

         
            var client = new RestClient("https://corgqr.herokuapp.com");
            var request = new RestRequest("/forgot", Method.POST);
            this.isEnabled = false;
            this.isVisible = false;
            var yourobject = new User
            {
                email = email

            };

            var json = JsonConvert.SerializeObject(yourobject);
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
            try
            {
                IRestResponse response = client.Execute(request);
                HttpStatusCode statusCode = response.StatusCode;
                int numericStatusCode = (int)statusCode;

                if (numericStatusCode == 200)
                {
                    Application.Current.MainPage.DisplayAlert(
                    "OK",
                    "La solicitud fue enviada correctamente, por favor revise su bandeja de correo y siga las instrucciones.",
                    "Aceptar");
                }
                if (numericStatusCode == 400)
                {
                    Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El correo indicado no existe en la base de datos, por favor verifique que este correcto.",
                    "Aceptar");
                }
            }
            catch
            {
                    Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Se ha presentado un error al enviar la solicitud, por favor intente más tarde",
                    "Aceptar");
            }
        }
        private void Recovery()
        {

            if (String.IsNullOrEmpty(this.Email))
            {
                Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El Correo es necesario",
                    "Accept");
                return;
            }


            if (this.email.Contains("@consultoriaorganizacional.com"))
            {

                Login(this.Email);
            }
            else
            {
                Login(this.Email + "@consultoriaorganizacional.com");
            }

        }

        private void Login(string emailtt)
        {
            Task.Run(() =>
            {
                try
                {
                    Device.BeginInvokeOnMainThread(() => IsBusy = true);
                    
                    var client = new RestClient("https://corgqr.herokuapp.com");
                    var request = new RestRequest("/forgot", Method.POST);
                    var yourobject = new User
                    {
                        email = emailtt

                    };

                    var json = JsonConvert.SerializeObject(yourobject);
                    request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                   

                        IRestResponse response = client.Execute(request);
                        HttpStatusCode statusCode = response.StatusCode;
                        int numericStatusCode = (int)statusCode;

                        if (numericStatusCode == 200)
                        {
                        Device.BeginInvokeOnMainThread(() => App.Current.MainPage.DisplayAlert("OK",
                        "La solicitud fue enviada correctamente, por favor revise su bandeja de correo y siga las instrucciones.",
                        "Aceptar"));


                    }

                        if (numericStatusCode == 400)
                        {
                        Device.BeginInvokeOnMainThread(() => App.Current.MainPage.DisplayAlert("Error",
                        "El correo indicado no existe en la base de datos, por favor verifique que este correcto.",
                        "Aceptar"));

                    }
                   
                }
                catch (Exception ex)
                {
                    Device.BeginInvokeOnMainThread(() => App.Current.MainPage.DisplayAlert("Error",
                    "Se ha presentado un error al enviar la solicitud, por favor intente más tarde",
                    "Aceptar"));

                }
                finally
                {
                    Device.BeginInvokeOnMainThread(() => IsBusy = false);
                }
            });
        }

        private async void Back()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }



        #endregion
    }
}
