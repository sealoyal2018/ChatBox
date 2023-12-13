using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using OpenAI.ObjectModels.ResponseModels.ImageResponseModel;
using Stylet;

namespace ChatBox.Models;

public class ImageBotChat : Chat, IHandle<ChatReceiveMessage<ImageCreateResponse>>
{
    public override string Body { get; }
    public override Bitmap Avatar => new(AssetLoader.Open(new System.Uri("avares://ChatBox/Assets/openai.png")));
    public BindableCollection<string> Images { get; }

    public ImageBotChat()
    {
        Images = new BindableCollection<string>();
    }

    public void Handle(ChatReceiveMessage<ImageCreateResponse> message)
    {
        if (message.Id == this.Id)
        {
            Images.AddRange(message.Body.Results.Select(v=> v.Url));
        }
    }
}