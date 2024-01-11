using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ChatBox.Models;
public class UserChat: Chat
{
    public override string Name => "You";
    public override string Body { get; }

    public override Bitmap Avatar => new Bitmap(AssetLoader.Open(new System.Uri("avares://ChatBox/Assets/user.jpg")));

    public UserChat(string body)
    {
        this.Body = body;
    }
}
