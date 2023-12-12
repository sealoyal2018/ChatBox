using System.Windows.Input;
using ChatBox.Interfaces;
using Stylet.Xaml;

namespace ChatBox.Services;

public class AboutSetting : ISetting
{
    public string DisplayName => "关于(v0.0.10)";
    public string Icon => "fa-solid fa-circle-info";
    public int Sort => 100;
    public ICommand ActiveCommand { get; }

    public AboutSetting()
    {
        ActiveCommand = new RelayCommand(() => { }, () => true);
    }
    
}