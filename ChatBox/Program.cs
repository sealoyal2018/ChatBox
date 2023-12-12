using System;
using System.Linq;
using System.Threading;

using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Fonts;

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
    private static AppBuilder BuildAvaloniaApp()
    {
        IconProvider.Current
            .Register<FontAwesomeIconProvider>();
        
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .With(new FontManagerOptions()
            {
                FontFallbacks = [new FontFallback{ FontFamily = "avares://ChatBox/Assets/fonts/LXGWWenKai.ttf#LXGW WenKai" }],
            })
            .LogToTrace();
    }
    private static void SetCultureSpecificFontOptions(AppBuilder builder, string culture, string fontFamily)
    {
        if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == culture)
        {
            FamilyNameCollection families = new(fontFamily);
            _ = builder.With(new FontManagerOptions()
            {
                DefaultFamilyName = families.PrimaryFamilyName,
                FontFallbacks = families
                    .Skip(1)
                    .Select(name => new FontFallback()
                    {
                        FontFamily = name
                    })
                    .ToList()
            });
        }
    }
}
