using System.Threading.Tasks;
using ChatBox.Interfaces;
using ChatBox.Models;
using ChatBox.Modules.Chats.ViewModels;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.ImageResponseModel;
using Stylet;
using Stylet.Avalonia;
using Stylet.Avalonia.Primitive;

namespace ChatBox.Modules.Paints.ViewModels;

public class HomeViewModel : Screen, IAppModule
{
    private readonly IEventAggregator _eventAggregator;
    private readonly IWindowManager _windowManager;
    private readonly BindableCollection<Chat> _chats;
    private string prompt = "画一只水里的小猫";
    private OpenAIService? _openAiService;

    public PaintSettingViewModel PaintSettingViewModel { get; }
    public string Icon => "fa-solid fa-palette";
    public int Sort => 20;

    public string Prompt
    {
        get => prompt;
        set => SetAndNotify(ref prompt, value);
    }
    public BindableCollection<Chat> Chats => _chats;

    public HomeViewModel(PaintSettingViewModel paintSettingViewModel, IEventAggregator eventAggregator, IWindowManager windowManager)
    {
        PaintSettingViewModel = paintSettingViewModel;
        _chats = [];
        _eventAggregator = eventAggregator;
        _windowManager = windowManager;
        DisplayName = "绘图";
    }

    public async Task SendMessage()
    {
        if (_openAiService is null)
        {
            var chatSettingViewModel = IoC.Get<ChatSettingViewModel>();
            var apiKey = chatSettingViewModel.Key;
            var baseUrl = chatSettingViewModel.BaseUrl;
            var providerType = chatSettingViewModel.ApiProvider;
            var deploymentId = chatSettingViewModel.DeploymentId;
            if(providerType == ProviderType.Azure && string.IsNullOrWhiteSpace(deploymentId))
            {
                await _windowManager.ShowMessageBox<bool>("部署Id不能为空", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _openAiService = new OpenAIService(new OpenAiOptions
            {
                ApiKey = apiKey,
                BaseDomain = baseUrl,
                ProviderType = providerType,
                DeploymentId = deploymentId
            });
        }
        
        var promptChat = new UserChat(Prompt);
        var newResponseChat = new ImageBotChat();
        Chats.Add(promptChat);
        Prompt = string.Empty;
        await Task.Delay(100);
        Chats.Add(newResponseChat);
        
        var image = new ImageCreateRequest
        {
            Prompt = promptChat.Body,
            N = 1,
            Size = StaticValues.ImageStatics.Size.Size256,
            ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
            User = "Sealoyal"
        };
        
        var result = await this._openAiService.CreateImage(image);
        _eventAggregator.PublishOnUIThread(new ChatReceiveMessage<ImageCreateResponse>(newResponseChat.Id, result));
    }
}