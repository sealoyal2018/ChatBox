namespace ChatBox.ViewModels;
public class ShellViewModel
{
    public ShellViewModel(ChatViewModel chatViewModel)
    {
        ChatViewModel = chatViewModel;
    }

    public ChatViewModel ChatViewModel { get; }
}
