using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using ChatBox.Interfaces;
using Stylet;
using Stylet.Avalonia;

namespace ChatBox.Modules.Chats.ViewModels;

public class HomeViewModel  : Conductor<ChatViewModel>, IAppModule
{
    private readonly BindableCollection<ChatViewModel> chatGroups;

    public string Icon => "fa-solid fa-message";
    public int Sort => 0;
    public List<IChatSetting> Settins { get; }
    public BindableCollection<ChatViewModel> ChatGroups => chatGroups;

    public HomeViewModel(ChatViewModel chatViewModel,IEnumerable<IChatSetting> settins)
    {
        Settins = settins.OrderBy(v=> v.Sort).ToList();
        chatGroups = new BindableCollection<ChatViewModel>
        {
            chatViewModel
        };

        ActiveItem = chatViewModel;
        DisplayName = "聊天";
    }

    public void NewChat()
    {
        var newChat = IoC.Get<ChatViewModel>();
        ChatGroups.Add(newChat);
        ActivateItem(newChat);
    }
    
    public void EditChat(PointerReleasedEventArgs e)
    {
        if (e.Pointer.IsPrimary && e.Source is Image i && i.DataContext is ChatViewModel group)
        {
            e.Handled = true;
        }
    }

    public void DeleteChat(PointerReleasedEventArgs e)
    {
        if (e.Pointer.IsPrimary && e.Source is Image i && i.DataContext is ChatViewModel group)
        {
            ChatGroups.Remove(group);
            if(ChatGroups.Count < 1)
                NewChat();
            e.Handled = true;
        }
    }
    
    public void SelectChat(PointerReleasedEventArgs e)
    {
        if (e.Pointer.IsPrimary && e.Source is Border { DataContext: ChatViewModel group } && group != ActiveItem)
        {
            ActiveItem = group;
            e.Handled = true;
        }
    }
}