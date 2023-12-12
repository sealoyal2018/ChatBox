using Avalonia.Markup.Xaml;
using ChatBox.Interfaces;
using ChatBox.Services;
using ChatBox.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBox;
public partial class App : ChatBoxApplication
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        base.Initialize();
    }

    protected override void Configuration(ServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<ChatSettingViewModel>();
        services.AddTransient<ChatViewModel>();
        services.AddSingleton<ISetting, OpenAiSetting>();
        services.AddSingleton<ISetting, CleanRecordSetting>();
    }
}