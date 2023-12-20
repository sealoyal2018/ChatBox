using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBox.Models;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using Stylet;
using Stylet.Avalonia.Primitive;

namespace ChatBox.Modules.Chats.ViewModels;
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
//         var md = """
//             当然，我可以为你提供一个简单的C#代码示例。以下是一个简单的控制台应用程序，它将提示用户输入姓名并打印出一个问候语：
//
//             ```csharp
//             using System;
//
//             class Program
//             {
//                 static void Main()
//                 {
//                     // 提示用户输入姓名
//                     Console.Write("请输入您的姓名: ");
//
//                     // 读取用户输入
//                     string userName = Console.ReadLine();
//
//                     // 打印问候语
//                     Console.WriteLine($"你好，{userName}！欢迎使用C#。");
//
//                     // 等待用户按下任意键退出程序
//                     Console.WriteLine("按任意键退出...");
//                     Console.ReadKey();
//                 }
//             }
//             ```
//
//             这个简单的程序演示了基本的输入输出和字符串操作。用户将被提示输入姓名，程序会读取输入并打印出一条问候语。然后，程序等待用户按下任意键才会退出。
//
//             你可以将这段代码粘贴到一个C#文件中（扩展名为`.cs`），然后使用C#编译器编译运行。希望这能帮助你入门C#编程！
//             """;
//         
//         var questionChat1 = new UserChat(Question);
//         var newResponseChat1 = new TextBotChat();
//         Chats.Add(questionChat1);
//         newResponseChat1.SetBody(md);
//         Chats.Add(newResponseChat1);
//         return;
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
        var newResponseChat = new TextBotChat();
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
            Model = _chatSettingViewModel.AiType,
        };

        _ = Task.Run(async () =>
        {
            await foreach (var response in _openAiService.CreateCompletionAsStream(request))
            {
                if (response.Successful)
                {
                    this.eventAggregator.PublishOnUIThread(new ChatReceiveMessage<List<ChatChoiceResponse>>(newResponseChat.Id, response.Choices));
                }
                else
                {
                    await Execute.PostToUIThreadAsync(async () =>
                    {
                        await _windowManager.ShowMessageBox<bool>(response.Error.Message, "错误", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    });
                }
            }
        });
    }
}
