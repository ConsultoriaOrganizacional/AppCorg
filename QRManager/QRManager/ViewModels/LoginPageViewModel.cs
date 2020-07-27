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

namespace QRManager.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        #region Attributes

        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        private bool isVisible;


        #endregion
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
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
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
            this.IsRunning = true;
            this.IsRemembered = true;
            this.IsEnabled = true;
            Email = QRManager.Utils.Settings.LastUsedEmail;
            Password = QRManager.Utils.Settings.LastPasword;
        }
        #endregion
        
        #region Commands
        public ICommand ClickCommand => new Command<string>((url) =>
        {
            UserDialogs.Instance.ShowLoading();
            Device.OpenUri(new System.Uri(url));
        }
        );

        public ICommand LoginCommand 
        {
        get
        {
                this.IsRunning = true;
                this.IsEnabled = true;
                this.IsVisible = true;
                return new RelayCommand(Login);
        }
    }
        public void Login1(string username, string password)
        {
            UserDialogs.Instance.ShowLoading();
            // We are using the RestSharp library which provides many useful
            // methods and helpers when dealing with REST.
            // We first create the request and add the necessary parameters

            var client = new RestClient("https://corgqr.herokuapp.com");
            var request = new RestRequest("/users/login", Method.POST);
            var yourobject = new User {
                email = username,
                password = password
            };    
           
            var json = JsonConvert.SerializeObject(yourobject);
            request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);


            // We execute the request and capture the response
            // in a variable called `response`
          
            try
            {
                UserDialogs.Instance.ShowLoading();
                IRestResponse response = client.Execute(request);
                LoginToken token = JsonConvert.DeserializeObject<LoginToken>(response.Content);
                if (token.token != null)
                {
                    UserDialogs.Instance.ShowLoading();
                    Application.Current.Properties["token"] = token.token;
                    GetUserData(token.token);
                   
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Oh No!", "Las credenciales ingresadas no son correctas, por favor validalos nuevamente.", "OK");
                    UserDialogs.Instance.HideLoading();
                    return;
                };
            }
            catch {

                Application.Current.MainPage.DisplayAlert("Oh No!", "Las credenciales ingresadas no son correctas, por favor validalos nuevamente.", "OK");
                UserDialogs.Instance.HideLoading();
                return;

            }
            // Using the Newtonsoft.Json library we deserialaize the string into an object,
            // we have created a LoginToken class that will capture the keys we need
            

            // We check to see if we received an `id_token` and if we did make a secondary call
            // to get the user data. If we did not receive an `id_token` we can safely assume
            // that the authentication failed so we display an error message telling the user
            // to try again.
          
        }

        public async void GetUserData(string token)
        {
            UserDialogs.Instance.ShowLoading();
            var client = new RestClient("https://corgqr.herokuapp.com");
            var request = new RestRequest("/users/me", Method.GET);
            request.AddHeader("Authorization", "Bearer "+token);


            IRestResponse response = client.Execute(request);

            DataUser dataUser = JsonConvert.DeserializeObject<DataUser>(response.Content);




            // Finally, we navigate the user the the Orders page
            
            MainViewModel.GetInstance().QRGenerator = new QRGeneratorViewModel();
            UserDialogs.Instance.HideLoading();
            await Application.Current.MainPage.Navigation.PushAsync(new QRGeneratorPage(dataUser));
           
        }
        private async void Login()
        {
        
            if (String.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "El Correo es necesario",
                    "Accept");
                this.Password = "123456aA";
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
        
          
            if (this.email.Contains("@consultoriaorganizacional.com"))
            {

                Login1(this.Email, this.Password);
            }
            else
            {
                Login1(this.Email+ "@consultoriaorganizacional.com", this.Password);
            }           
            this.IsRunning = true;
            this.IsEnabled = true;
            this.isVisible = true;

            this.Email = QRManager.Utils.Settings.LastUsedEmail;
            this.Password = QRManager.Utils.Settings.LastPasword;
        }
        #endregion
    }
}
