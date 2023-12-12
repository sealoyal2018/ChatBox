using System;
using System.Windows.Input;

namespace ChatBox;

public sealed class RelayCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;
    private readonly Action execute;
    private readonly Func<bool>? canExecute;
    
    public RelayCommand(Action execute)
    {
        ArgumentNullException.ThrowIfNull(execute);

        this.execute = execute;
    }
    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public RelayCommand(Action execute, Func<bool> canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        ArgumentNullException.ThrowIfNull(canExecute);

        this.execute = execute;
        this.canExecute = canExecute;
    }
    
    public bool CanExecute(object? parameter)
    {
        return this.canExecute?.Invoke() != false;
    }

    public void Execute(object? parameter)
    { 
        this.execute.Invoke();
    }

}