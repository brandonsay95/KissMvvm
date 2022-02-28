using KissMvvm.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KissMvvm.ViewModels
{
    public class ViewModelBase : NotificationBase,IDisposable
    {
        private readonly App app;

        public ViewModelBase(App app=null)
        {
            if (app == null)
                return;
            app.OnAppExit += App_OnAppExit;
            this.app = app;
        }
        /// <summary>
        /// Function called when app is closed
        /// Use Class AppExitViewModel or supply app to base ex
        /// constructor(App app):base(app)
        /// if this is not included the functin will not be called
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void App_OnAppExit(object sender, App e)
        {
            app.OnAppExit -= App_OnAppExit;
        }

        public void Dispose()
        {
            if (app == null)
                return;
            app.OnAppExit-= App_OnAppExit;
        }
    }
    public class AppExitViewModel : ViewModelBase
    {
        public AppExitViewModel(App app) : base(app)
        {
        }
    }
}
