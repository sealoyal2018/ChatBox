using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using ChatBox.ViewModels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Stylet;
using Stylet.Avalonia;
using Stylet.Avalonia.Primitive;

namespace ChatBox;
public class ChatBoxApplication : StyletApplicationBase<MainViewModel>
{
    private IServiceProvider? serviceProvider;

    protected override void ConfigureBootstrapper()
    {
        IoC.GetInstances = this.GetInstances;
        IoC.GetInstance = this.GetInstance;
        var services = new ServiceCollection();
        var viewManagerConfig = new ViewManagerConfig()
        {
            ViewFactory = this.GetInstance,
            ViewAssemblies = new List<Assembly>() { this.GetType().Assembly }
        };
        services.AddSingleton(viewManagerConfig);
        services.AddSingleton<IViewManager, ViewManager>();
        services.AddSingleton(this);
        services.AddSingleton<IWindowManager, WindowManager>();
        services.AddSingleton<IEventAggregator, EventAggregator>();
        services.AddSingleton<IMessageBoxViewModel, MessageBoxViewModel>();
        services.AddTransient<MessageBoxView>();
        this.Configuration(services);
        this.AutoRegistView(services);
        serviceProvider = services.BuildServiceProvider();
    }

    protected virtual void Configuration(ServiceCollection services)
    {

    }


    protected override object GetInstance(Type service, string? key)
    {
        if(serviceProvider is null)
            throw new TypeInitializationException(nameof(serviceProvider), new Exception(nameof(serviceProvider)));
        if (string.IsNullOrEmpty(key))
            return serviceProvider.GetService(service);
        return serviceProvider.GetRequiredKeyedService(service, key);
    }

    protected override object GetInstance(Type type)
    {
        return serviceProvider.GetService(type);
    }

    protected override IEnumerable<object> GetInstances(Type service)
    {
        return serviceProvider.GetServices(service);
    }

    private void AutoRegistView(IServiceCollection services)
    {
        var viewManager = services.BuildServiceProvider().GetRequiredService<IViewManager>() as ViewManager;
        var list = viewManager.ViewAssemblies.SelectMany(v => v.GetTypes()).Where(v => v.Name.EndsWith(viewManager.ViewNameSuffix)).ToImmutableList();
        foreach(var item in list)
        {
            services.TryAddTransient(item);
        }
    }
}
