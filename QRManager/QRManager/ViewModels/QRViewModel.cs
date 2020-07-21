using System;
using System.Collections.Generic;
using System.Text;

namespace QRManager.ViewModels
{
    class QRViewModel : ViewModelBase
    {
        public QRViewModel(string TeamCodeIn )
        {
            this.TeamCode = TeamCodeIn;
        }
        private string teamCode;
        public string TeamCode
        {
            get { return teamCode; }
            set { SetProperty(ref teamCode, value); }
        }
    }
}
