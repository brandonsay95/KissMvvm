using KissMvvm.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KissMvvm.ViewModels;
using System.Windows.Controls;
using System.Reflection;

namespace KissMvvm.Services
{
    internal class NavigationPart {
        public NavigationPart(Type viewModel, Type view, string route)
        {
            ViewModel = viewModel;
            View = view;
            Route = route;
        }

        public Type ViewModel { get; set; }
        public Type View { get; set; }
        public string Route { get; set; }
    }
    public class NavigationService:NotificationBase
    {
        private List<NavigationPart> navigations = new List<Services.NavigationPart>();
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel { get => _currentViewModel; private set { _currentViewModel = value;OnPropertyChanged(); } }

        private UserControl _currentView;

        public NavigationService(bool autoWire=false)
        {
            if (autoWire)
                AutoWire(Assembly.GetCallingAssembly());
        }

        public UserControl CurrentView { get=> _currentView;private set { _currentView = value; OnPropertyChanged(); }  }
        public void Navigate<T>(object arguments= null) where T : ViewModelBase
        {
            var instance = (T)Activator.CreateInstance(typeof(T),arguments);
            this.CurrentViewModel = instance;
            var viewType = navigations.First(o => o.ViewModel == typeof(T));
            var viewInstance = (UserControl)Activator.CreateInstance(viewType.View);
            viewInstance.DataContext = instance;
            this.CurrentView = viewInstance;
        }
        private object activate(Type type, object arguments = null)
        {
            var constructors = type.GetConstructors();
            //best match 
            ConstructorInfo constructorToUse =
                constructors.OrderByDescending(o => o.GetParameters().Length).FirstOrDefault();
            if (constructors == null)
                return Activator.CreateInstance(type);
            else if(constructorToUse.GetParameters().Length==0)
                return Activator.CreateInstance(type);
            else
                return Activator.CreateInstance(type, arguments);

        }
        public void Navigate(string url,object arguments = null)
        {
            var part = navigations.First(o => o.Route == url);
            var instance = (ViewModelBase)activate(part.ViewModel, arguments);

            this.CurrentViewModel = instance;
            var viewInstance = (UserControl)activate(part.View);
            viewInstance.DataContext = instance;
            this.CurrentView = viewInstance;
        }

        public void WireUp<TViewModel, TView>(string navigationRoute= null) where TViewModel:ViewModelBase where TView :UserControl
        {
            if (navigationRoute == null)
            {
                if (navigations.Any(o => o.ViewModel == typeof(TViewModel)))
                    throw new Exception("Navigation route is already supplied");
            }
            else {
                if (navigations.Any(o => o.ViewModel == typeof(TViewModel) || o.Route == navigationRoute) )
                    throw new Exception("Navigation route is already supplied");

            }
            this.navigations.Add(new NavigationPart(typeof(TViewModel), typeof(TView), navigationRoute));
        }

        public void AutoWire(Assembly assembly = null)
        {
            if (assembly == null)
                assembly = Assembly.GetCallingAssembly();
            var aTypes = assembly.GetTypes();
            var vms = aTypes.Where(o => o.Name.EndsWith("ViewModel") && typeof(ViewModelBase).IsAssignableFrom(o));
            foreach(var model in vms)
            {
                var viewModelName = model.Name;
                var route = model.Name.Replace("ViewModel", "");
                var viewName = route + "View";

                var viewType = aTypes.FirstOrDefault(o => o.Name == viewName);
                if (viewType == null)
                    throw new Exception($"Type:{model.Name}, Does not have a View {viewType}");
                this.navigations.Add(new NavigationPart(model, viewType, route));
            }
        }
    }
}
