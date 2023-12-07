namespace ChatBox.ViewModels;
public class MainViewModel
{
    public ShellViewModel ShellViewModel { get; }
    public MainViewModel(ShellViewModel shellViewModel)
    {
        ShellViewModel = shellViewModel;
    }
}
