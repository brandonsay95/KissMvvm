using KissMvvm.Commands;
using KissMvvm.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KissMvvm.Helpers
{
    public class ImageButton : NotificationBase
    {

        private RelayCommand _command;

        public RelayCommand Command
        {
            get { return _command; }
            set { _command = value;OnPropertyChanged(); }
        }

        public double Opacity
        {
            get
            {
                return IsEnabled ? 1 : 0.5;
            }
        }


        private string _enabledImagePath;

        public string EnabledImagePath
        {
            get { return _enabledImagePath; }
            set { _enabledImagePath = value; OnPropertyChanged(); }
        }


        private string _disabledImagePath;

        public string DisabledImagePath
        {
            get { return _disabledImagePath; }
            set { _disabledImagePath = value; OnPropertyChanged(); }
        }

        public string ImagePath
        {
            get {
                return IsEnabled ? EnabledImagePath : DisabledImagePath;
            }
        }
        private bool lastEnabled;
        private Func<bool> isEnabledFunc;
        public bool IsEnabled
        {
            get {
                if (isEnabledFunc != null)
                {
                    var e = isEnabledFunc();
                    if (lastEnabled == null)
                        lastEnabled = e;
                    if (e != lastEnabled)
                    {
                        lastEnabled = e;

                        this.Command.ExecuteChanged();
                    }else
                        lastEnabled = e;

                    return e;
                }
                return _isEnabled.Value;
            }
            set { _isEnabled = value; OnPropertyChanged("ImagePath"); OnPropertyChanged("Opacity"); }
        }
        private bool? _isEnabled;
        public ImageButton(string imagePath, RelayCommand command, Func<bool> isEnabled)
        {
            isEnabledFunc = isEnabled;
            EnabledImagePath = imagePath;
            DisabledImagePath = imagePath;
            Command = command;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagePath"> PathToImage</param>
        /// <param name="command"> OnClick Event</param>
        /// <param name="isEnabled"> Enabled Function</param>
        public ImageButton(string imagePath, Action<object> command, Func<bool> isEnabled)
        {
            isEnabledFunc = isEnabled;
            EnabledImagePath = imagePath;
            DisabledImagePath = imagePath;
            Command = new RelayCommand(command,(a)=>this.IsEnabled);
        }
        public ImageButton(string imagePath,RelayCommand command,bool isEnabled=true)
        {
            IsEnabled = isEnabled;
            EnabledImagePath = imagePath;
            DisabledImagePath = imagePath;
            Command = command;
        }
        public ImageButton(string enabledImagePath, string disabledImagePath = null, bool isEnabled = true,RelayCommand command = default)
        {
            EnabledImagePath = enabledImagePath;
            if (disabledImagePath == null)
                DisabledImagePath = enabledImagePath;
            else
                DisabledImagePath = disabledImagePath;
            IsEnabled = isEnabled;
            Command = command;

        }

       



    }
}
