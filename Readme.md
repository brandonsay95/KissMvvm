# KissMVVM
## Keep is simple Stupid MVVM
- This project has one simple design, keep mvvm stupid simple 
- No nonsense startup configuration
- plug and play 
- simple setup

## How to setup
- Download this source,
- Add it to your project
- Add it as a reference
- Create a new Wpf project
- Delete MainWindow and App xaml files
- Create Startup.cs
- Paste the following code in 
```csharp
public class Startup
    {

        [STAThread]
        public static void Main(string[] args)=> new App(new NavigationService(true),"Home",true);

        or 
        
        public static void Main(string[] args) => new App(new KissMvvm.ViewModels.MainViewModel("Ghost Conductor",400,400,new NavigationService(true)), "Home", true);//custom title and width and height

        
    }
```
- Add a HomeViewModel.cs file that implements ViewModelBase
- Add a HomeView.xaml file that is a UserControl
- Run the code (you're done)

- Includes Test project

## How does it work?
KisMVVM uses a backend system to wire up the navigation **NavigationService**,
Uses its own App and Window file to speed up dev

## NavigationService
NavigationService accepts a boolean **autoWire** defaulted to false 
this will grab all classes ending in ViewModel that implement ViewModelBase and auto add them to the navigation
```csharp
        public NavigationService(bool autoWire=false)
        
     var navigationService = new NavigationService();
        navigationService.WireUp<HomeViewModel, HomeView>("Home");// manual wiring of components
        navigationService.Navigate("Home", "argument"); //navigate with url and argument
        navigationService.Navigate<HomeViewModel>();//navigate based off view model

```

## App
app is a xaml implementation used for simplifying startup
```csharp
  public App(NavigationService navigationService,string defaultRoute = null,bool autoStart=false);// auto start will execute.Run();
  new App(navigationService).Run();// create app and run non automatically
```

## RelayCommand
used to wire up buttons on the ui
```csharp
 public class RelayCommand : CommandBase
    {
        public RelayCommand(Action<object> execute, bool canExecute = true);
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute);

    }
```
## Credits
- With help from youtube tutorials SingletonSean  https://www.youtube.com/watch?v=bBoYHl3pLEo&list=PLA8ZIAm2I03hS41Fy4vFpRw8AdYNBXmNm&index=6
## Contributions
- Feel free to contribute , but keep it simple.
- If it impacts startup time by adding extra actions , find a simpler way.
- If you have to add more extravagant features keep backwards capabilities intact
