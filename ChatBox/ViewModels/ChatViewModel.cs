using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBox.Models;
using ChatGPT.Net;
using ChatGPT.Net.DTO.ChatGPT;
using Stylet;
using Stylet.Avalonia.Primitive;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels;

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
    

    public string Title
    {
        get => title;
        set => SetAndNotify(ref title, value);
    }

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
            _openAiService = new OpenAIService(new OpenAiOptions
            {
                ApiKey = apiKey,
                BaseDomain = baseUrl,
            });
        }
        if(Chats.Count < 1)
        {
            if (question.Length < 6)
                Title = question[..question.Length];
            else 
                Title = question[..6];
        }
        var questionChat = new UserChat(question);
        var newResponseChat = new BotChat();
        Chats.Add(questionChat);
        Question = string.Empty;
        await Task.Delay(100);
        Chats.Add(newResponseChat);
        
        var request = new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromUser("帮我一写一段c#入门级的代码！")
            },
            Model = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo,
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
