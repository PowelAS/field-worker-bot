using System;
using System.Windows.Input;

namespace Powel.BotClient.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action _executeAction;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action executeAction, Func<bool> canExecute = null)
        {
            _executeAction = executeAction;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute.Invoke();
        }

        public void Execute(object parameter)
        {
            _executeAction.Invoke();
        }

        public event EventHandler CanExecuteChanged;
    }
}