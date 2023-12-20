using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using OpenAI.ObjectModels.SharedModels;
using Stylet;
using Stylet.Avalonia;

using Tmds.DBus.Protocol;

namespace ChatBox.Models;
public class TextBotChat : Chat, IHandle<ChatReceiveMessage<List<ChatChoiceResponse>>>,IDisposable
{
    private StringBuilder bodyBuilder = new();
    private bool disposedValue;

    public override Bitmap Avatar => new Bitmap(AssetLoader.Open(new System.Uri("avares://ChatBox/Assets/openai.png")));


    public override string Body => bodyBuilder.ToString();

    public TextBotChat()
    {
        Application.Current.ActualThemeVariantChanged += Current_ActualThemeVariantChanged;
        var eventAggregator = IoC.Get<IEventAggregator>();
        eventAggregator.Subscribe(this);
    }

    private void Current_ActualThemeVariantChanged(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    public void Handle(ChatReceiveMessage<List<ChatChoiceResponse>> message)
    {
        if (message.Id == this.Id)
        {
            Monitor.Enter(this);
            try
            {
                bodyBuilder.Append(string.Join("",message.Body.Select(v => v.Message.Content)));
                NotifyOfPropertyChange(nameof(Body));
            }
            finally
            {
                Monitor.Exit(this);
            }
        }
    }

    internal void SetBody(string md)
    {
        bodyBuilder.Append(md);
        NotifyOfPropertyChange(nameof(Body));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            disposedValue = true;
        }
    }

    // // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    // ~TextBotChat()
    // {
    //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
