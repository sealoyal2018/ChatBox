using System;
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
public abstract class Chat : PropertyChangedBase
{
    public abstract Dock Dock { get; }
    public abstract HorizontalAlignment HorizontalAlignment { get; }
    public abstract Brush Background { get; }
    public abstract string Body { get;}
    public abstract Bitmap Avatar { get; }
    public virtual string Id { get; }

    public Chat()
    {
        Id = Guid.NewGuid().ToString("N");
    }
}

public record ChatReceiveMessage(string Id, string Body);

