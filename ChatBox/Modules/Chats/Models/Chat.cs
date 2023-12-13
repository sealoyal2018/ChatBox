﻿using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OpenAI.ObjectModels.SharedModels;
using Stylet;

namespace ChatBox.Modules.Chats.Models;
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

public record ChatReceiveMessage(string Id, List<ChatChoiceResponse> Body);
