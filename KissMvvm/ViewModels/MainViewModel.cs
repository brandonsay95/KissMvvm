using KissMvvm.Services;

namespace KissMvvm.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private NavigationService _navigationService;
        public NavigationService NavigationService { get { return _navigationService; } set { _navigationService = value; OnPropertyChanged(); } }
        public MainViewModel(Services.NavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }
    }
}
