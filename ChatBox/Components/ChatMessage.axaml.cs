using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using ChatBox.Models;
using Markdown.Avalonia.Full;

namespace ChatBox.Components;

public partial class ChatMessage : UserControl
{
    public static AvaloniaProperty<IEnumerable<Chat>> ChatsProperty
        = AvaloniaProperty.Register<ChatMessage, IEnumerable<Chat>>("Chats");

    public IEnumerable<Chat> Chats
    {
        get => (IEnumerable<Chat>)GetValue(ChatsProperty);
        set => SetValue(ChatsProperty, value);
    }
    
    public ChatMessage()
    {
        InitializeComponent();
    }
}