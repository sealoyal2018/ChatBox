using System.Windows.Input;
using ChatBox.Interfaces;
using ChatBox.ViewModels;
using Stylet;
using Stylet.Avalonia;

namespace ChatBox.Services;

public class OpenAiSetting : ISetting
{
    public string DisplayName => "设置";
    public string Icon => "fa-solid fa-gear";
    public ICommand ActiveCommand { get; }

    public int Sort => 10;

    public OpenAiSetting()
    {
        ActiveCommand = new RelayCommand(Execute, CanExecute);
    }

    private void Execute()
    {
        var windowManager = IoC.Get<IWindowManager>();
        var settings = IoC.Get<ChatSettingViewModel>();
        windowManager.ShowDialog<bool>(settings);    
    }

    private bool CanExecute() => true;
}