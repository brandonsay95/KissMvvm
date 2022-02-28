using KissMvvm.Services;

namespace KissMvvm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private NavigationService _navigationService;
        public NavigationService NavigationService { get { return _navigationService; } set { _navigationService = value; OnPropertyChanged(); } }
        public MainViewModel(App app, string title, double width, double height, Services.NavigationService navigationService) : base(app)
        {
            this.NavigationService = navigationService;

            Title = title;
            StartupTitle = title;
            Width = width;
            Height = height;
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }


        private string _startupTitle;

        public string StartupTitle
        {
            get { return _startupTitle; }
            set { _startupTitle = value; OnPropertyChanged(); }
        }


        private double _width;

        public double Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged(); }
        }


        private double _height;

        public double Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged(); }
        }

    }
}
