using System;
using System.Diagnostics;
using System.Text;
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
    private StringBuilder bodyBuilder = new StringBuilder();

    public override Dock Dock => Dock.Left;

    public override HorizontalAlignment HorizontalAlignment => HorizontalAlignment.Left;

    public override Brush Background => new SolidColorBrush(Color.Parse("#d2f9d1"));

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
            Debug.WriteLine(message.Body);
            this.bodyBuilder.Append(message.Body);
            NotifyOfPropertyChange(nameof(Body));
        }
    }

}
