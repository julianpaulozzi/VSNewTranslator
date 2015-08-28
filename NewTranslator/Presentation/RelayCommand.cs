using System;
using System.Windows.Input;

namespace NewTranslator.Presentation
{
    public class RelayCommand<T> : ICommand where T : class , new()
    {
        private Action<T> execute;

        private Predicate<T> canExecute;

        private event EventHandler CanExecuteChangedInternal;

        public RelayCommand(Action<T> execute)
            : this(execute, DefaultCanExecute)
        {
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }
        
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }

            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute != null && canExecute(parameter as T);
        }

        public void Execute(object parameter)
        {
            execute(parameter as T);
        }
        
        public void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChangedInternal;
            if (handler != null)
            {
                //DispatcherHelper.BeginInvokeOnUIThread(() => handler.Invoke(this, EventArgs.Empty));
                handler.Invoke(this, EventArgs.Empty);
            }
        }

        public void Destroy()
        {
            canExecute = _ => false;
            execute = _ => { return; };
        }

        private static bool DefaultCanExecute(T parameter)
        {
            return true;
        }
    }
}