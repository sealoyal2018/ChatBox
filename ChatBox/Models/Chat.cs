using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Avalonia.Media.Imaging;
using OpenAI.ObjectModels.SharedModels;
using Stylet;

namespace ChatBox.Models;
public abstract class Chat : PropertyChangedBase
{
    public abstract string Name { get; }
    public abstract string Body { get;}
    public abstract Bitmap Avatar { get; }
    public virtual string Id { get; }
    protected Chat()
    {
        Id = Guid.NewGuid().ToString("N");
    }
}

public record ChatReceiveMessage<T>(string Id, T Body);

