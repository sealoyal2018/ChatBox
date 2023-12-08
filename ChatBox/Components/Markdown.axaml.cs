using System;
using System.IO;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform;
using Stylet;
using Stylet.Avalonia;
using System.Diagnostics;

namespace ChatBox.Components;
public partial class Markdown : UserControl, IHandle<StreamMessage>
{
    public static readonly StyledProperty<string> TextProperty;
    public static readonly StyledProperty<string> IdProperty;
    private static string? markdownStyle;
    private string html;
    private static readonly string _htmlTemplate = """
        <html>
        <head>
            <title>Intro</title>
        <link
          rel="stylesheet"
          href="StyleSheet"
        />
        </head>
        <body>
            <div class="markdown-body">
        #TEMPLATE
            </div>
        </body>
        </html>
        """;
    private readonly IEventAggregator eventAggregator;

    static Markdown()
    {
        TextProperty = AvaloniaProperty.Register<Markdown, string>("Text", string.Empty);
        TextProperty.Changed.Subscribe(e =>
        {
            var markdown = e.Sender as Markdown;
            markdown.TextChanged(e.NewValue.Value);
        });
        IdProperty = AvaloniaProperty.Register<Markdown, string>("Id", string.Empty);
    }

    private async void TextChanged(string? newValue)
    {
        if (newValue is null)
        {
            Html = string.Empty;
            Execute.OnUIThread(() => this._htmlPanel.Text = Html);
            return;
        }
        var markdown = await Request.ConvertMarkdown(newValue);
        Html = _htmlTemplate.Replace("#TEMPLATE", markdown);
        Execute.OnUIThread(() => this._htmlPanel.Text = Html);
    }
    
    private void StylesheetLoadHandle(object? sender, TheArtOfDev.HtmlRenderer.Avalonia.HtmlRendererRoutedEventArgs<TheArtOfDev.HtmlRenderer.Core.Entities.HtmlStylesheetLoadEventArgs> e)
    {
        if (e.Event.Src == "StyleSheet")
            e.Event.SetStyleSheet = markdownStyle;
    }

    public string Text
    {
        get
        {
            return (string)GetValue(TextProperty);
        }
        set
        {
            this.SetValue(TextProperty, value);
        }
    }

    public string Id
    {
        get => (string)GetValue(IdProperty);
        set => this.SetValue(IdProperty, value);
    }

    public string Html { 
        get => html; 
        set => html = value; 
    }

    public Markdown()
    {
        this.eventAggregator = IoC.Get<IEventAggregator>();
        InitializeComponent();
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(this.Id))
        {
            eventAggregator.Subscribe(this);
        }

        if (markdownStyle is null)
        {
            using StreamReader sr = new StreamReader(AssetLoader.Open(new System.Uri("avares://ChatBox/Assets/github.markdown.css")));
            markdownStyle = await sr.ReadToEndAsync();
        }
        base.OnLoaded(e);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(this.Id))
        {
            eventAggregator.Unsubscribe(this);
        }
        base.OnUnloaded(e);
    }

    public void Handle(StreamMessage message)
    {
        if(this.Id == message.Id)
        {
            Debug.WriteLine("receive>>>>>>>: "+ message.Content);
            this.Text += message.Content;
        }
    }
}

public record StreamMessage(string Id, string Content);


public class Request
{
    private static readonly HttpClient http = new HttpClient();

    private class Body
    {
        public string Mode { get; set; }
        public string Text { get; set; }
    }
    public static async Task<string> ConvertMarkdown(string text)
    {
        var body = new Body
        {
            Mode = "gfm",
            Text = text
        };
        var httpContent = JsonContent.Create(body);
        var req = new HttpRequestMessage(HttpMethod.Post, "https://api.github.com/markdown");
        req.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/119.0");
        req.Content = httpContent;
        var response = await http.SendAsync(req);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();
        return text;
    }
}

