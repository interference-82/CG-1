namespace LabsCG.Commands
{
    using System;
    using System.Windows.Input;

    internal class DelegateCommand : ICommand
    {
        private readonly Func<object, bool> canExecute;
        private readonly Action<object> execute;

        public DelegateCommand(Action<object> executeAction) : this(executeAction, null)
        {
        }

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteFunc)
        {
            canExecute = canExecuteFunc;
            execute = executeAction;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute != null) return canExecute(parameter);
            return true;
        }

        public void Execute(object parameter)
        {
            execute?.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}