using ChatBox.Interfaces;
using ChatBox.Modules.Paints.ViewModels;
using ChatBox.Modules.Paints.Views;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBox.Modules.Paints;

public static class DependencyInjection
{
    public static IServiceCollection AddPaints(this IServiceCollection services)
    {
        services.AddSingleton<IAppModule, HomeViewModel>();
        services.AddSingleton<PaintSettingViewModel>();
        services.AddSingleton<PaintView>();
        return services;
    }
}