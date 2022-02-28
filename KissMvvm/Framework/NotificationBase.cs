using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KissMvvm.Framework
{
    public class NotificationBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        /// <summary>
        /// SAME as OnPropertyChanged Aliased for other users
        /// </summary>
        /// <param name="propertyname"></param>
        public void RaisePropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
