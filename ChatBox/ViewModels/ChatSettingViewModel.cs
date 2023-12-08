using Stylet;

namespace ChatBox.ViewModels;

public class ChatSettingViewModel: PropertyChangedBase
{
    private string key;
    private string baseUrl = "https://api.openai.com/";

    public string Key
    {
        get => key;
        set => SetAndNotify(ref key, value);
    }
    public string BaseUrl
    {
        get => baseUrl;
        set => SetAndNotify(ref baseUrl, value);
    }
}