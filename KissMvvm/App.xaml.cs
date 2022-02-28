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
        public event EventHandler<App> OnAppExit;
        public object Class { get; set; }
        private readonly NavigationService navigationService;
        private readonly MainViewModel mainViewModel;
        private readonly string defaultRoute;
        private readonly Action<App,Window> onAppClose;
        private Window window;
        public App()
        {
            
        }
        public App(MainViewModel mainViewModel, string defaultRoute, bool autoStart, Action<App,Window> onAppClose = null)
        {
            this.navigationService = mainViewModel.NavigationService;
            this.mainViewModel = mainViewModel;
            this.defaultRoute = defaultRoute;
            this.onAppClose = onAppClose;
            registerTypes(this.navigationService);

            if (autoStart)
                this.Run();
        }

        private void registerTypes(NavigationService navigationService)
        {
            if(this.mainViewModel!=null)
                 navigationService.Inject(this.mainViewModel);
            navigationService.Inject(this.navigationService);
            //navigationService.Inject(this.window);
            navigationService.Inject(this);

        }
        //Deprecated
        //public App(NavigationService navigationService,string defaultRoute = null,bool autoStart=false)
        //{
        //    this.navigationService = navigationService;
            
        //    this.defaultRoute = defaultRoute;
        //    this.window = window;
        //    registerTypes(this.navigationService);

        //    if (autoStart)
        //        this.Run();
                    
        //}
        
        protected override void OnStartup(StartupEventArgs e)
        {
           
            if (window == null)
            {
                if (mainViewModel != null)
                    window = new KissMvvm.MainWindow() { DataContext = mainViewModel};
                else
                {
                    var mainViewModel = new MainViewModel(this,"KISS(Change ME)", 400, 800, navigationService);
                    this.navigationService.Inject(mainViewModel);
                    window = new KissMvvm.MainWindow() { DataContext = mainViewModel };
                }
            }
            this.InitializeComponent();
            if (defaultRoute != null)
                this.navigationService.Navigate(defaultRoute);
            window.Show();
            base.OnStartup(e);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            onAppClose?.Invoke(this,this.window);
            OnAppExit?.Invoke(this,this);
        }
    }
}
