using System;
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
    private string question = "你是谁？";

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

        var bot = new ChatGpt(_chatSettingViewModel.Key, config: new ChatGPT.Net.DTO.ChatGPT.ChatGptOptions
        {
            BaseUrl = "https://api.gptapi.us"
        });
        var questionChat = new UserChat(question);
        var newResponseChat = new BotChat();
        Chats.Add(questionChat);
        Question = string.Empty;
        Chats.Add(newResponseChat);
        await Task.Delay(1000).ContinueWith(async (_) =>
        {
            await bot.AskStream((s) =>
            {
                this.eventAggregator.PublishOnUIThread(new StreamMessage(newResponseChat.Id, s));
            }, questionChat.Body);
        });

        
        return;
    }
}
