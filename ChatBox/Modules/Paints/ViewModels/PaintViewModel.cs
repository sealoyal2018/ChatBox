using System.Collections.Generic;
using System.Threading.Tasks;
using ChatBox.Interfaces;
using ChatBox.Models;
using ChatBox.Modules.Paints.Models;
using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.ImageResponseModel;
using Stylet;

namespace ChatBox.Modules.Paints.ViewModels;

public class PaintViewModel
{
    private readonly IEventAggregator _eventAggregator;

    public PaintViewModel(IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
    }
    
    public async Task Send()
    {
        var key = "";
        var url = "";
        var openAiService = new OpenAIService(new OpenAiOptions
        {
            ProviderType = ProviderType.OpenAi,
            ApiKey = key,
            BaseDomain = url,
        });
        
        var image = new ImageCreateRequest
        {
            Prompt = "Laser cat eyes",
            N = 2,
            Size = StaticValues.ImageStatics.Size.Size256,
            ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
            User = "TestUser"
        };

        var result = await openAiService.CreateImage(image);
        _eventAggregator.PublishOnUIThread(new ChatReceiveMessage<ImageCreateResponse>("", result));
    }

}