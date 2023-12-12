using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBox.Models;
using Stylet;
using Stylet.Avalonia.Primitive;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.Interfaces;

namespace ChatBox.ViewModels;
public class ChatViewModel : Screen
{
    private readonly ChatSettingViewModel _chatSettingViewModel;
    private readonly IWindowManager _windowManager;
    private readonly IEventAggregator eventAggregator;
    private readonly BindableCollection<Chat> _chats;
    private string title;
    private string question = "写一段入门级的C#代码";
    private OpenAIService? _openAiService;
    
    public string Question
    {
        get => question;
        set => SetAndNotify(ref question, value);
    }

    public BindableCollection<Chat> Chats => _chats;

    public bool VisibleChat => Chats.Count > 0;

    public ChatViewModel(ChatSettingViewModel chatSettingViewModel,IWindowManager windowManager, IEventAggregator eventAggregator)
    {
        _chatSettingViewModel = chatSettingViewModel;
        _windowManager = windowManager;
        this.eventAggregator = eventAggregator;
        title = "新话题";
        _chats = new BindableCollection<Chat>();
        _chats.CollectionChanged += (sender, args) =>
        {
            NotifyOfPropertyChange(nameof(VisibleChat));
        };
        DisplayName = "新的对话";
    }

    public async Task SendMessage()
    {
        if (string.IsNullOrWhiteSpace(_chatSettingViewModel.Key))
        {
            await _windowManager.ShowMessageBox<bool>("请先配置openai key", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (string.IsNullOrWhiteSpace(Question))
            return;
        if (_openAiService is null)
        {
            var apiKey = _chatSettingViewModel.Key;
            var baseUrl = _chatSettingViewModel.BaseUrl;
            var providerType = _chatSettingViewModel.ApiProvider;
            var deploymentId = _chatSettingViewModel.DeploymentId;
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
        if(Chats.Count < 1)
        {
            if (Question.Length < 6)
                DisplayName = Question[..Question.Length];
            else 
                DisplayName = Question[..6];
        }
        var questionChat = new UserChat(Question);
        var newResponseChat = new BotChat();
        Chats.Add(questionChat);
        Question = string.Empty;
        await Task.Delay(100);
        Chats.Add(newResponseChat);
        
        var request = new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromUser(questionChat.Body)
            },
            //Model = _chatSettingViewModel.AiType,
            Model = OpenAI.ObjectModels.Models.Dall_e_3,
        };

        _ = Task.Run(async () =>
        {
            await foreach (var response in _openAiService.CreateCompletionAsStream(request))
            {
                if (response.Successful)
                {
                    this.eventAggregator.PublishOnUIThread(new ChatReceiveMessage(newResponseChat.Id, response.Choices));
                }
            }
        });
    }
}
