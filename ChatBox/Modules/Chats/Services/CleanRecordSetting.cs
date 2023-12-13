using System.Windows.Input;
using ChatBox.Interfaces;
using ChatBox.Modules.Chats.ViewModels;
using Stylet.Avalonia;

namespace ChatBox.Modules.Chats.Services;

public class CleanRecordSetting : IChatSetting
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
        var homeViewModel = IoC.Get<HomeViewModel>();
        homeViewModel.ChatGroups.Clear();
        homeViewModel.NewChat();
    }

    private bool CanExecute()
    {
        var homeViewModel = IoC.Get<HomeViewModel>();
        return homeViewModel.ChatGroups.Count > 0;
    }
}