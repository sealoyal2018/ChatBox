using System;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Fonts;
using ChatBox.Extensions;
using Projektanker.Icons.Avalonia;
using Projektanker.Icons.Avalonia.FontAwesome;

namespace ChatBox;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current
            .Register<FontAwesomeIconProvider>();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseCustomFontManager(opts =>
            {
                opts.DefaultFontFamily = "fonts:CustomFontFamilies#LXGW WenKai";
                opts.Key = new Uri("fonts:CustomFontFamilies", UriKind.Absolute);
                opts.Source = new Uri("avares://ChatBox/Assets/fonts", UriKind.Absolute);
            })
            .LogToTrace();
    }
}