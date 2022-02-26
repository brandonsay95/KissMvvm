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
        private List<object> injection = new List<object>();
        private List<NavigationPart> navigations = new List<Services.NavigationPart>();
        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel { get => _currentViewModel; private set { _currentViewModel = value;OnPropertyChanged(); } }

        private UserControl _currentView;
        public void Inject(object o)
        {
            injection.Add(o);
        }
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
            var viewType = navigations.FirstOrDefault(o => o.ViewModel == typeof(T));
            if (viewType == null)
                throw new Exception($"Route '{typeof(T).Name}' associated view Does not exist in the registry");

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
            {
                var p = constructorToUse.GetParameters().ToList();
                List<object> pars = new List<object>();
                foreach(var par in p)
                {
                    bool hasfoundMatch = false;
                    foreach(var o in injection)
                    {
                        if (o.GetType() == par.ParameterType)
                        {
                            pars.Add(o);
                            hasfoundMatch = true;
                            break;
                        }
                        if (!hasfoundMatch)
                            throw new Exception("Could not resolve parameter " + par.ParameterType.Name);
                    }
                }
                return Activator.CreateInstance(type, pars.ToArray());


            }

        }
        public void Navigate(string url,object arguments = null)
        {
            var part = navigations.FirstOrDefault(o => o.Route == url);
            if (part == null)
                throw new Exception($"Route '{url}' Does not exist in the registry");

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
            if (this.navigations.Count == 0)
                throw new Exception("No pairs found, You Must have a view model class that inherits 'ViewModelBase' and ends with ViewModel to use autowire, Either turn off autowire or create such a class");
        }
    }
}
