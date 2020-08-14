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
using System.Net;

namespace QRManager.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Attributes

        private string email;
        private string password;
        bool isBusy = false;
        private bool isEnabled;
        private bool isVisible;
        public class LoginToken
        {
            public DataUser DataUser { get; set; }
            public string token { get; set; }

        }
        public class User
        { 
            public string email { get; set; }
            public string password { get; set; }
        }
        public class DataUser
        {
            public string name { get; set; }
            public string compañia { get; set; }
            public string cargo { get; set; }
            public string telefono { get; set; }
            public string telefonocorp { get; set; }
            public string direccion { get; set; }
            public string direccion2 { get; set; }
            public string email { get; set; }
            public string nota { get; set; }

        }
        #endregion 
        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value);
            QRManager.Utils.Settings.LastUsedEmail = value;
            }
        }
        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value);
            QRManager.Utils.Settings.LastPasword = value;
            }
        }

        public bool IsRemembered
        {
            get; set;
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
        #region Constructor
        public LoginPageViewModel()
        {
            Email = QRManager.Utils.Settings.LastUsedEmail;
            Password = QRManager.Utils.Settings.LastPasword;
        }
        public bool IsBusy

        {
            get { return isBusy; }

            set { SetProperty(ref isBusy, value); }

        }
        #endregion
        #region Commands
        public ICommand ClickCommand => new Command<string>((url) =>
        {
           Device.OpenUri(new System.Uri(url));
        }
        );

        public ICommand LoginCommand 
        {
        get
        {
                IsRemembered = true;
                this.IsEnabled = true;
                this.IsVisible = true;
                return new RelayCommand(Login);
        }
            
        }
        public ICommand RecoveryPasswordCommand
        {
            get
            {
               
                return new RelayCommand(Recovery);
            }
            set
            {

            }
        }
        public void Login1(string username, string password)
        {
            Task.Run(() =>

            {
            try
            {
                var client = new RestClient("https://corgqr.herokuapp.com");
                    this.IsEnabled = false;
                    this.isVisible = false;
                    var request = new RestRequest("/users/login", Method.POST);
                var yourobject = new User
                {
                    email = username,
                    password = password
                };
                var json = JsonConvert.SerializeObject(yourobject);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                
                    Device.BeginInvokeOnMainThread(() => IsBusy = true);
                    IRestResponse response = client.Execute(request);
                    HttpStatusCode statusCode = response.StatusCode;
                    LoginToken token = JsonConvert.DeserializeObject<LoginToken>(response.Content);
                    if (token.token != null)
                    {
                        Device.BeginInvokeOnMainThread(() => Application.Current.Properties["token"] = token.token);
                        GetUserData(token.token);

                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(() => App.Current.MainPage.DisplayAlert
                        ("Oh No!",
                        "Las credenciales ingresadas no son correctas, por favor validalos nuevamente.",
                        "Aceptar"));
                        this.IsEnabled = true;
                        this.isVisible = true;
                        return;
                    };
                }
                catch
                {

                    Device.BeginInvokeOnMainThread(() => App.Current.MainPage.DisplayAlert(
                        "Oh No!",
                        "Las credenciales ingresadas no son correctas, por favor validalos nuevamente.",
                        "Aceptar"));
                        this.IsEnabled = true;
                        this.isVisible = true;
                        return;

                }
                finally
                {
                    Device.BeginInvokeOnMainThread(() => IsBusy = false);
                }

                // Using the Newtonsoft.Json library we deserialaize the string into an object,
                // we have created a LoginToken class that will capture the keys we need


                // We check to see if we received an `id_token` and if we did make a secondary call
                // to get the user data. If we did not receive an `id_token` we can safely assume
                // that the authentication failed so we display an error message telling the user
                // to try again.

            });

        }

        public  void GetUserData(string token)
        {    

           
            var client = new RestClient("https://corgqr.herokuapp.com");
            var request = new RestRequest("/users/me", Method.GET);
            request.AddHeader("Authorization", "Bearer "+token);


            IRestResponse response = client.Execute(request);

            DataUser dataUser = JsonConvert.DeserializeObject<DataUser>(response.Content);

            // Finally, we navigate the user the the Orders page
            
            MainViewModel.GetInstance().QRGenerator = new QRGeneratorViewModel();
           
            this.IsEnabled = true;
            this.isVisible = true;

            Device.BeginInvokeOnMainThread(() => App.Current.MainPage.Navigation.PushAsync(new QRGeneratorPage(dataUser)));
        
        }

        private async void Login()
        {
            
            if (String.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El Correo es necesario",
                    "Accept");
                return;
            }
            if (String.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "La contraseña es necesaria",
                    "Accept");
                return;
            }
           
            this.IsEnabled = false;
            this.isVisible = false;
            if (this.email.Contains("@consultoriaorganizacional.com"))
            {

                Login1(this.Email, this.Password);
            }
            else
            {
                Login1(this.Email+ "@consultoriaorganizacional.com", this.Password);
            }           

           
            this.IsEnabled = true;
            this.isVisible = true;
            
            this.Email = QRManager.Utils.Settings.LastUsedEmail;
            this.Password = QRManager.Utils.Settings.LastPasword;
        }
        private async void Recovery()
        {
            MainViewModel.GetInstance().RecoPassword = new RecoveryPasswordViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new RecoveryPasswordPage());
        }
        #endregion
    }
}
