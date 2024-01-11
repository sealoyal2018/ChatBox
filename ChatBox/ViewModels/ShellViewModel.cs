using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Styling;
using ChatBox.Interfaces;
using Projektanker.Icons.Avalonia;
using Stylet;
using Stylet.Avalonia;

namespace ChatBox.ViewModels;

public class ShellViewModel : Conductor<ChatViewModel>.Collection.OneActive
{
    private readonly BindableCollection<ChatViewModel> chatGroups;

    public string Icon => "fa-solid fa-message";
    public int Sort => 0;
    public List<IChatSetting> Settins { get; }
    public BindableCollection<ChatViewModel> ChatGroups => chatGroups;

    public ShellViewModel(ChatViewModel chatViewModel,IEnumerable<IChatSetting> settins)
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
    
    public void ChangeTheme()
    {
        if (Application.Current.ActualThemeVariant == ThemeVariant.Light)
            Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
        else
            Application.Current.RequestedThemeVariant = ThemeVariant.Light;
    }

    protected override void OnInitialActivate()
    {
        base.OnInitialActivate();
    }

    public void ShowMoreFlyout(PointerReleasedEventArgs e)
    {
        if (e.Pointer.IsPrimary && e.Source is Image i)
        {
            FlyoutBase.ShowAttachedFlyout(i);
            e.Handled = true;
        }
    }
    
}
    