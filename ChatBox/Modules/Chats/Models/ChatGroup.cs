using Stylet;

namespace ChatBox.Modules.Chats.Models;

public class ChatGroup : PropertyChangedBase
{
    private string title;

    public string Title
    {
        get => title;
        set => SetAndNotify(ref title, value);
    }

    public ChatGroup()
    {
        title = "新话题";
    }
}