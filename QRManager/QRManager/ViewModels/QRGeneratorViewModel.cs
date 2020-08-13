using QRManager.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace QRManager.ViewModels
{
    public class QRGeneratorViewModel : ViewModelBase
    {
        #region Campos
        private string teamCode;
        public string TeamCode
        {
            get { return teamCode; }
            set { SetProperty(ref teamCode, value); }
        }

        private string nombre;

        public string Nombre
        {

            get {
                return nombre; }
            set { SetProperty(ref nombre, value); }
        }
        private string compañia;
        public string Compañia
        {

            get
            {
                return compañia;
            }
            set { SetProperty(ref compañia, value); }
        }
        private string cargo;
        public string Cargo
        {

            get
            {
                return cargo;
            }
            set { SetProperty(ref cargo, value); }
        }

        private string telefono;
        public string Telefono
        {

            get
            {
                return telefono;
            }
            set { SetProperty(ref telefono, value); }
        }
        private string telefonocorp;
        public string Telefonocorp
        {
            get
            {
                return telefonocorp;
            }
            set { SetProperty(ref telefonocorp, value); }
        }
        private string telefonobogota;

        public string Telefonobogota
        {
            get
            {
                return telefonobogota;
            }
            set
            {
                SetProperty(ref telefonobogota, value);
            }
        }
        private string telefonomedellin;

        public string Telefonomedellin
        {
            get
            {
                return telefonomedellin;
            }
            set
            {
                SetProperty(ref telefonomedellin, value);
            }
        }

        private string correo;
        public string Correo
        {

            get
            {
                return correo;
            }
            set { SetProperty(ref correo, value); }
        }
        private string direccion;
        public string Direccion
        {

            get
            {
                return direccion;
            }
            set { SetProperty(ref direccion, value); }
        }
        private string direccion2;
        public string Direccion2
        {

            get
            {
                return direccion2;
            }
            set { SetProperty(ref direccion2, value); }
        }
      

        private string nota;
        public string Nota
        {

            get
            {
                return nota;
            }
            set { SetProperty(ref nota, value); }
        }


        #endregion
        #region GeneradorQr
        public QRGeneratorViewModel()
        {
            nombre = "Yair Ortiz";
            compañia = "Consultoría Organizacional";
            cargo = "Ingeniero de Desarrollo";
            telefono = "3505340925";
            correo = "yortizr@consultoriaorganizacional.com";
            direccion = "Cr 7 N° 127 - 48 Of.910";
            direccion2 = "Cra 43a No 1-50 Torre 4 of 407 - Medellín";
            nota = "Agregrame a tus contactos";


        }
        public QRGeneratorViewModel(LoginPageViewModel.DataUser Datos)
        {
            nombre = Datos.name;
            compañia = Datos.compañia;
            cargo = Datos.cargo;
            telefono = Datos.telefono;
            Telefonocorp = Datos.telefonocorp;
            Telefonobogota = "+5716052525";
            Telefonomedellin = "+5745407000";
            correo = Datos.email;
            direccion = Datos.direccion;
            direccion2 = Datos.direccion2;
            nota = Datos.nota;


        }
        public ICommand GenerateQRCodeCommand => new Command(GenerateQRCode);


        private async void GenerateQRCode()
        {

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(compañia) || string.IsNullOrEmpty(cargo)
                || string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty("+5716052525") || string.IsNullOrEmpty("+5745407000") ||
                string.IsNullOrEmpty(direccion) || string.IsNullOrEmpty(direccion2))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Todos los campos son obligatorios",
                    "Accept");
                return;
            }
            else
            {
                TeamCode = getVCard(nombre, compañia, cargo, telefono, telefonocorp, telefonobogota, telefonomedellin, correo, direccion, direccion2, nota);
                await Application.Current.MainPage.Navigation.PushAsync(new QRPage(TeamCode));

            }
        }
        private static String getVCard(String name,
                                       String company,
                                       String title,
                                       String tel,
                                       String tel2,
                                       String tel3,
                                       String tel4,
                                       String email,
                                       String address,
                                       String address2,
                                       String memo)
        {
            StringBuilder output = new StringBuilder(100);
            output.Append("BEGIN:VCARD\n");
            output.Append("VERSION:3.0\n");
            maybeAppendvCard(output, "N", name);
            maybeAppendvCard(output, "ORG", company);
            maybeAppendvCard(output, "WORK", title);
            maybeAppendvCard(output, "TEL;type=CELL;type=VOICE;type=pref", tel);
            if (tel2 == "No")
            {

            }
            else
            {
                maybeAppendvCard(output, "TEL;type=WORK;type=VOICE;type=pref", tel2);
            }
            maybeAppendvCard(output, "TEL;type=HOME;type=VOICE;type=pref", tel3);
            maybeAppendvCard(output, "TEL;type=HOME;type=VOICE;type=pref", tel4);
            maybeAppendvCard(output, "EMAIL", email);
            maybeAppendvCard(output, "ADR", buildAddress(address, ""));
            maybeAppendvCard(output, "ADR", buildAddress(address2, ""));
            maybeAppendvCard(output, "NOTE", memo);
            maybeAppendvCard(output, "URL;type=WebPage", "https://www.consultoriaorganizacional.com/index/");
            maybeAppendvCard(output, "URL;type = Lindekin", "https://www.linkedin.com/company/consultoria-organizacional/");
            maybeAppendvCard(output, "URL;type = Facebook", "https://www.facebook.com/ConsultoriaOrganizacional/?fref=ts");
            maybeAppendvCard(output, "URL;type = Twitter", "https://twitter.com/ConsultoriaOrg_");
            maybeAppendvCard(output, "URL;type = Youtube", "https://www.youtube.com/channel/UC6HgwL_d7ywL0MWM0iRqP8A");


            output.Append("END:VCARD");
            return output.ToString();
        }
        private static void maybeAppendvCard(StringBuilder output, String prefix, String value)
        {



            if (!String.IsNullOrEmpty(value))
            {
                value = value.Replace("([\\\\,;])", "\\\\$1");
                value = value.Replace("\\n", "\\\\n");
                output.Append(prefix).Append(':').Append(value).Append('\n');
            }
        }
        private static String buildAddress(String address, String address2)
        {
            if (!String.IsNullOrEmpty(address))
            {
                if (!String.IsNullOrEmpty(address2))
                {
                    return address + ' ' + address2;
                }
                return address;
            }
            if (!String.IsNullOrEmpty(address2))
            {
                return address2;
            }
            return "";
        }
        private static String buildAddress2(String address2)
        {
            if (!String.IsNullOrEmpty(address2))
            {
                return "";
            }
            return "";
        }
        #endregion

    }
}
