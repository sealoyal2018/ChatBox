using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using ChatBox.Interfaces;
using ChatBox.Modules.Chats.ViewModels;
using Stylet;
using Stylet.Avalonia;

namespace ChatBox.ViewModels;
public class ShellViewModel : Conductor<ChatViewModel>
{
    public List<IChatSetting> Settins { get; }
    private readonly BindableCollection<ChatViewModel> chatGroups;

    public BindableCollection<ChatViewModel> ChatGroups => chatGroups;

    public ShellViewModel(ChatViewModel chatViewModel,IEnumerable<IChatSetting> settins)
    {
        Settins = settins.OrderBy(v=> v.Sort).ToList();
        chatGroups = new BindableCollection<ChatViewModel>
        {
            chatViewModel
        };

        ActiveItem = chatViewModel;
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
