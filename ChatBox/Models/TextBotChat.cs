using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using OpenAI.ObjectModels.SharedModels;
using Stylet;
using Stylet.Avalonia;

namespace ChatBox.Models;
public class TextBotChat : Chat, IHandle<ChatReceiveMessage<List<ChatChoiceResponse>>>
{
    private StringBuilder bodyBuilder = new();

    public override Bitmap Avatar => new Bitmap(AssetLoader.Open(new System.Uri("avares://ChatBox/Assets/openai.png")));


    public override string Body => bodyBuilder.ToString();

    public TextBotChat()
    {
        var eventAggregator = IoC.Get<IEventAggregator>();
        eventAggregator.Subscribe(this);
    }

    public void Handle(ChatReceiveMessage<List<ChatChoiceResponse>> message)
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
