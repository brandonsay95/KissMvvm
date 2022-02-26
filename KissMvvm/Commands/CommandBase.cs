using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KissMvvm.Commands
{
    public abstract class CommandBase : ICommand
    {
        private bool canExecute { get; set; }
        public event EventHandler CanExecuteChanged;
        protected void OnCanExecutedChanged() => CanExecuteChanged?.Invoke(this, new EventArgs());

        public virtual bool CanExecute(object parameter) => true;
        public virtual void Execute(object parameter) { }
        public void ExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
    public class RelayCommand : CommandBase
    {
        public Visibility Visibility = Visibility.Hidden;
        public RelayCommand(Action<object> execute, bool canExecute = true)
        : this(execute, (a) => canExecute) { }

        private readonly Func<object, bool> canExecute;
        private readonly Action<object> execute;
        
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }
        public override bool CanExecute(object parameter) => canExecute(parameter);
        public override void Execute(object parameter) => execute(parameter);
    }
}
