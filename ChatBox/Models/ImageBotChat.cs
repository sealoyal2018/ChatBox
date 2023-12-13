using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using OpenAI.ObjectModels.ResponseModels.ImageResponseModel;
using Stylet;
using Stylet.Avalonia;

namespace ChatBox.Models;

public class ImageBotChat : Chat, IHandle<ChatReceiveMessage<ImageCreateResponse>>
{
    private readonly IEventAggregator _eventAggregator;
    public override string Body { get; }
    public override Bitmap Avatar => new(AssetLoader.Open(new System.Uri("avares://ChatBox/Assets/openai.png")));
    public BindableCollection<string> Images { get; }

    public ImageBotChat()
    {
        _eventAggregator = IoC.Get<IEventAggregator>();
        Images = new BindableCollection<string>();
        _eventAggregator.Subscribe(this);
    }

    public void Handle(ChatReceiveMessage<ImageCreateResponse> message)
    {
        if (message.Id == this.Id)
        {
            // Images.AddRange([
            // "https://oaidalleapiprodscus.blob.core.windows.net/private/org-50VgV4wIGhu5xQCAEhyqduiZ/user-fJQ0eCWTERdT6hItllevzpSm/img-7ZqLseiTpoQJoZLnEU1k03VO.png?st=2023-12-13T07%3A33%3A03Z&se=2023-12-13T09%3A33%3A03Z&sp=r&sv=2021-08-06&sr=b&rscd=inline&rsct=image/png&skoid=6aaadede-4fb3-4698-a8f6-684d7786b067&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2023-12-12T23%3A29%3A39Z&ske=2023-12-13T23%3A29%3A39Z&sks=b&skv=2021-08-06&sig=8UrFfmrkH/Q/OgDzrrJ79dnB6n2PpPxZcH9NfvriAt4%3D",
            // "https://oaidalleapiprodscus.blob.core.windows.net/private/org-50VgV4wIGhu5xQCAEhyqduiZ/user-fJQ0eCWTERdT6hItllevzpSm/img-wlqASk0Zt7IviKazpmpxAVLS.png?st=2023-12-13T07%3A33%3A03Z&se=2023-12-13T09%3A33%3A03Z&sp=r&sv=2021-08-06&sr=b&rscd=inline&rsct=image/png&skoid=6aaadede-4fb3-4698-a8f6-684d7786b067&sktid=a48cca56-e6da-484e-a814-9c849652bcb3&skt=2023-12-12T23%3A29%3A39Z&ske=2023-12-13T23%3A29%3A39Z&sks=b&skv=2021-08-06&sig=DqzMyj6LMAnkcPaD%2BsOeCdJZt5PAJqdsIxrt2QX27Qg%3D"
            // ]);
            if (message.Body.Successful)
            {
                var urls = message.Body.Results.Select(v => v.Url);
                Images.AddRange(urls);
                NotifyOfPropertyChange(nameof(Images));
            }
        }
    }
}