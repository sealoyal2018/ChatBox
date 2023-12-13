using ChatBox.Interfaces;
using ChatBox.Modules.Chats.Services;
using ChatBox.Modules.Chats.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBox.Modules.Chats;

public static class DependencyInjection
{
    public static IServiceCollection AddChats(this IServiceCollection services)
    {
        services.AddSingleton<ChatSettingViewModel>();
        services.AddTransient<ChatViewModel>();
        services.AddSingleton<IChatSetting, OpenAiSetting>();
        services.AddSingleton<IChatSetting, CleanRecordSetting>();
        services.AddSingleton<IChatSetting, AboutSetting>();
        services.AddSingleton<IChatSetting, PromptSetting>();
        services.AddSingleton<IAppModule, HomeViewModel>();
        return services;
    }
}