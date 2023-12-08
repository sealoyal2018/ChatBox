using System;
using System.Linq;
using System.Threading.Tasks;

using ChatBox.Components;
using ChatBox.Models;
using ChatGPT.Net;

using OpenAI_API;
using OpenAI_API.Models;
using Stylet;
using Stylet.Avalonia.Primitive;

namespace ChatBox.ViewModels;
public class ChatViewModel : Screen
{
    private readonly ChatSettingViewModel _chatSettingViewModel;
    private readonly IWindowManager _windowManager;
    private readonly IEventAggregator eventAggregator;
    private string title;
    private string question = "写一段入门级的C#代码";

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
    
    private readonly BindableCollection<Chat> _chats;
    private readonly ChatGpt bot;

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
        this.bot = new ChatGpt("", config: new ChatGPT.Net.DTO.ChatGPT.ChatGptOptions
        {
            BaseUrl = "https://api.gptapi.us"
        });
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

        bot.APIKey = _chatSettingViewModel.Key;

        if(Chats.Count < 1)
        {
            if (question.Length < 6)
                Title = question[..question.Length];
            else 
                Title = question[..6];
        }
        var questionChat = new UserChat(question);
        var newResponseChat = new BotChat();
        Execute.PostToUIThread(() => Chats.Add(questionChat));
        Question = string.Empty;
        await Task.Delay(100);
        Chats.Add(newResponseChat);
        _ = Task.Run(async () =>
        {
            await Task.Delay(100);
            var message = await bot.Ask(questionChat.Body);
            this.eventAggregator.PublishOnUIThread(new ChatReceiveMessage(newResponseChat.Id, message));
        });
        return;
    }
}
