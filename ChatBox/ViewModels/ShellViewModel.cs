using Avalonia.Controls;
using Avalonia.Input;
using ChatBox.Models;
using Stylet;

namespace ChatBox.ViewModels;
public class ShellViewModel : Conductor<ChatViewModel>
{
    private readonly BindableCollection<ChatViewModel> chatGroups;

    public BindableCollection<ChatViewModel> ChatGroups => chatGroups;

    public ShellViewModel(ChatViewModel chatViewModel)
    {
        chatGroups = new BindableCollection<ChatViewModel>
        {
            chatViewModel
        };

        ActiveItem = chatViewModel;
    }

    public void NewChat()
    {
        ChatGroups.Add(new ChatViewModel());
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
