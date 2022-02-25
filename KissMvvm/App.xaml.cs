using KissMvvm.Services;
using KissMvvm.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
namespace KissMvvm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public object Class { get; set; }
        private readonly NavigationService navigationService;
        private readonly MainViewModel mainViewModel;
        private readonly string defaultRoute;
        private Window window;
        public App()
        {
            
        }
        public App(MainViewModel mainViewModel,string defaultRoute,bool autoStart)
        {
            this.navigationService = mainViewModel.NavigationService;
            this.mainViewModel = mainViewModel;
            this.defaultRoute = defaultRoute;
            if (autoStart)
                this.Run();
        }
       public App(NavigationService navigationService,string defaultRoute = null,bool autoStart=false, Window window =null)
        {
            this.navigationService = navigationService;
            this.defaultRoute = defaultRoute;
            this.window = window;
            if (autoStart)
                this.Run();
                    
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
           
            if (window == null)
            {
                if (mainViewModel != null)
                    window = new KissMvvm.MainWindow() { DataContext = mainViewModel};
                else
                    window = new KissMvvm.MainWindow() { DataContext = new MainViewModel("KISS(Change ME)", 400, 800,navigationService) };
            }
            this.InitializeComponent();
            if (defaultRoute != null)
                this.navigationService.Navigate(defaultRoute);
            window.Show();
            base.OnStartup(e);
        }
    }
}
