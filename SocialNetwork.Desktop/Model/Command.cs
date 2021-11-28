using System;
using System.Windows.Input;

namespace SocialNetwork.Desktop.Model
{
    public class Command : ICommand
    {
        private readonly Action<object> _command;

#pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

        public Command(Action<object> execute)
        {
            _command = execute;
        }

        public bool CanExecute(object parametr)
        {
            return true;
        }

        public void Execute(object parametr)
        {
            _command(parametr);
        }
    }
}
