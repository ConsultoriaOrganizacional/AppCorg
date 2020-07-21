using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QRManager.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QRManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRPage : ContentPage
    {
        public QRPage(string TeamCodeIn)
        {
            InitializeComponent();
            BindingContext = new QRViewModel(TeamCodeIn);
        }
    }
}