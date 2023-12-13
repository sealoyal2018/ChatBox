using Avalonia.Markup.Xaml;
using ChatBox.Interfaces;
using ChatBox.Modules.Chats.Services;
using ChatBox.Modules.Chats.ViewModels;
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
        services.AddSingleton<IChatSetting, OpenAiSetting>();
        services.AddSingleton<IChatSetting, CleanRecordSetting>();
        services.AddSingleton<IChatSetting, AboutSetting>();
        services.AddSingleton<IChatSetting, PromptSetting>();
    }
}