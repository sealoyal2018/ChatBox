using Avalonia.Markup.Xaml;
using ChatBox.Modules.Chats;
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
        services.AddChats();
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<ShellViewModel>();
        services.AddHttpClient();
    }
}