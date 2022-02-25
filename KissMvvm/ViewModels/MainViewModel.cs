using KissMvvm.Services;

namespace KissMvvm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private NavigationService _navigationService;
        public NavigationService NavigationService { get { return _navigationService; } set { _navigationService = value; OnPropertyChanged(); } }
        public MainViewModel(string title, int width, int height,Services.NavigationService navigationService)
        {
            this.NavigationService = navigationService;

            Title = title;
            Width = width;
            Height = height;
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }


        private int _width;

        public int Width
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged(); }
        }


        private int _height;

        public int Height
        {
            get { return _height; }
            set { _height = value; OnPropertyChanged(); }
        }

    }
}
