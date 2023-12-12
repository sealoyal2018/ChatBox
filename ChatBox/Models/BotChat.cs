using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

using Stylet;
using Stylet.Avalonia;

namespace ChatBox.Models;
public class BotChat : Chat, IHandle<ChatReceiveMessage>
{
    // private StringBuilder bodyBuilder = new StringBuilder();
    private readonly string token = Guid.NewGuid().ToString("N");
    private StringBuilder bodyBuilder = new();
    public override Dock Dock => Dock.Left;

    public override HorizontalAlignment HorizontalAlignment => HorizontalAlignment.Left;

    public override Brush Background => new SolidColorBrush(Color.Parse("#f4f6f8"));

    public override Bitmap Avatar => new Bitmap(AssetLoader.Open(new System.Uri("avares://ChatBox/Assets/openai.png")));


    public override string Body => bodyBuilder.ToString();

    public BotChat()
    {
        var eventAggregator = IoC.Get<IEventAggregator>();
        eventAggregator.Subscribe(this);
    }

    public void Handle(ChatReceiveMessage message)
    {
        if (message.Id == this.Id)
        {
            Monitor.Enter(this);
            try
            {
                bodyBuilder.Append(string.Join("",message.Body.Select(v => v.Message.Content)));
                NotifyOfPropertyChange(nameof(Body));
            }
            finally
            {
                Monitor.Exit(this);
            }
        }
    }

}
