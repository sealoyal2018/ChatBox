using Stylet;

namespace ChatBox.ViewModels;

public class ChatSettingViewModel: PropertyChangedBase
{
    private string key;

    public string Key
    {
        get => key;
        set => SetAndNotify(ref key, value);
    }
}