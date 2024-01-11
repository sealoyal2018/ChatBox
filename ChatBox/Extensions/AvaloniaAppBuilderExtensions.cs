using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Fonts;

namespace ChatBox.Extensions;

public static class AvaloniaAppBuilderExtensions
{
    public class FontOption
    {
        public string DefaultFontFamily { get; set; }
        public Uri Key { get; set; }
        public Uri Source { get; set; }
    }
    
    public static AppBuilder UseCustomFontManager(this AppBuilder builder, Action<FontOption>? configDelegate = default)
    {
        var setting = new FontOption();
        configDelegate?.Invoke(setting);

        return builder
            .ConfigureFonts(manager => manager.AddFontCollection(new EmbeddedFontCollection(setting.Key, setting.Source)))
            .With(new FontManagerOptions
            {
                DefaultFamilyName = setting.DefaultFontFamily,
                FontFallbacks = new[]
                {
                    new FontFallback
                    {
                        FontFamily = new FontFamily(setting.DefaultFontFamily),
                    },
                },
            });
    }
}