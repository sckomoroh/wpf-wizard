using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WizardTest.WizardContainer.Common
{
    public class SimpleCommand : ICommand
    {
        private readonly Action _action;

        public event EventHandler CanExecuteChanged;
        
        public SimpleCommand(Action action)
        {
            _action = action;
        }

        public SimpleCommand(Action action, Predicate<object> predicate)
            : this(action)
        {
            Predicate = predicate;
        }

        public bool CanExecute(object parameter)
        {
            if (Predicate == null)
            {
                return true;
            }

            return Predicate(null);
        }

        public Predicate<object> Predicate { get; set; }

        public void Execute(object parameter)
        {
            if (_action != null)
            {
                _action();
            }
        }

        public void RaiseExecuteStateChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }
    }

    public class SimpleCommand<T> : ICommand
    {
        private readonly Action<T> _action;
        
        public event EventHandler CanExecuteChanged;
        
        public SimpleCommand(Action<T> action)
        {
            _action = action;
        }

        public SimpleCommand(Action<T> action, Predicate<T> predicate)
            : this(action)
        {
            Predicate = predicate;
        }

        public Predicate<T> Predicate { get; set; }

        public bool CanExecute(object parameter)
        {
            if (Predicate == null)
            {
                return true;
            }

            return Predicate((T)parameter);
        }

        public void Execute(object parameter)
        {
            if (_action != null)
            {
                _action((T)parameter);
            }
        }

        public void RaiseExecuteStateChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, new EventArgs());
            }
        }
    }
}
