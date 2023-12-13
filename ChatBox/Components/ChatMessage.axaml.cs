using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using ChatBox.Models;
using ChatBox.Modules.Chats.Models;

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