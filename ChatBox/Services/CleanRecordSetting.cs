using System.Windows.Input;
using ChatBox.Interfaces;
using ChatBox.ViewModels;
using Stylet;
using Stylet.Avalonia;

namespace ChatBox.Services;

public class CleanRecordSetting : ISetting
{
    public string DisplayName => "清空会话";
    public string Icon => "fa-solid fa-trash";
    public int Sort => 9;
    public ICommand ActiveCommand { get; }

    public CleanRecordSetting()
    {
        ActiveCommand = new RelayCommand(Execute, CanExecute);
    }

    private void Execute()
    {
        var shellViewModel = IoC.Get<ShellViewModel>();
        shellViewModel.ChatGroups.Clear();
        shellViewModel.NewChat();
    }

    private bool CanExecute()
    {
        var shellViewModel = IoC.Get<ShellViewModel>();
        return shellViewModel.ChatGroups.Count > 0;
    }
    
}