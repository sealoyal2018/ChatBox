using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Stylet.Avalonia;

namespace ChatBox.Converters;

public class UrlToBitmapConverter : IValueConverter
{
    private readonly HttpClient http;

    public static UrlToBitmapConverter Instance = new();
    private UrlToBitmapConverter()
    {
        this.http = IoC.Get<HttpClient>();
    }
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var url = value?.ToString();
        try
        {
            if (value is null)
                return null;
            if(url.StartsWith("http"))
            {
                var bytes = AsyncHelper.Sync(async () =>
                {
                    var response = await this.http.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var data = await response.Content.ReadAsByteArrayAsync();
                    return data;
                });
                return new Bitmap(new MemoryStream(bytes));
            }

            return new Bitmap(AssetLoader.Open(new Uri(url)));
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred while downloading image '{url}' : {ex.Message}");
            return null;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}