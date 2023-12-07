using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ChatBox.Models;
public class Chat
{
    private readonly SolidColorBrush leftBackgrond;
    private readonly SolidColorBrush rightBackground;
    private readonly string botAvatar;
    private readonly string userAvatar;

    public Chat()
    {
        this.leftBackgrond = new SolidColorBrush(Color.Parse("#f4f6f8"));
        this.rightBackground = new SolidColorBrush(Color.Parse("#d2f9d1"));
        botAvatar = "avares://ChatBox/Assets/openai.png";
        userAvatar = "avares://ChatBox/Assets/user.jpg";
    }

    public string Body { get; set; }
    public Dock Dock { get; set; }


    public HorizontalAlignment HorizontalAlignment
    {
        get => Dock.Left == Dock ? HorizontalAlignment.Left : HorizontalAlignment.Right;
    }
    public Brush Background
    {
        get => Dock.Left == Dock ? this.leftBackgrond : this.rightBackground;
    }
    public Bitmap Avatar
    {
        get
        {
            var avares = Dock.Left == Dock ? this.botAvatar : this.userAvatar;
            return new Bitmap(AssetLoader.Open(new System.Uri(avares)));
        }
    }
}
