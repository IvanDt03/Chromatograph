using System;
using System.Windows.Input;

namespace Chromatograph.Commands;

public class RelayCommand : ICommand
{
    private Action<object?>? _execute;
    private Func<object?, bool>? _canExecute;

    public RelayCommand(Action<object?>? execute, Func<object?, bool>? canExecute = null)
    {
        if (execute == null)
            throw new ArgumentNullException(nameof(execute));

        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public virtual void Execute(object? parameter)
    {
        if (_execute != null)
            _execute.Invoke(parameter);
    }

    public virtual bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke(parameter) ?? true;
    }

    public void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}
