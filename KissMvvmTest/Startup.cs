using KissMvvm.Services;
using KissMVVM;
using KissMvvmTest.View;
using KissMvvmTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KissMvvmTest
{
    public class Startup
    {

        [STAThread]
        public static void Main(string[] args)
        {
            new App(new NavigationService(true), "home", true);
            var navigationSerivce = new NavigationService();
            navigationSerivce.WireUp<HomeViewModel, HomeView>("Home");
            navigationSerivce.Navigate("Home", "argument");
            navigationSerivce.Navigate<HomeViewModel>();
        }

        
    }
}
