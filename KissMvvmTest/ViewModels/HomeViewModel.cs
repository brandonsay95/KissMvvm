using KissMvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KissMvvmTest.ViewModels
{
    public class HomeViewModel:ViewModelBase
    {
        private string _subHeader;

        public HomeViewModel(string subHeader)
        {
            this.SubHeader = subHeader;
        }

        public string SubHeader
        {
            get { return _subHeader; }
            set { _subHeader = value; OnPropertyChanged(); }
        }


    }
}
