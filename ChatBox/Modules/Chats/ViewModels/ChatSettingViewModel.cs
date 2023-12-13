using System.Collections.Generic;
using OpenAI;
using Stylet;

namespace ChatBox.Modules.Chats.ViewModels;

public class ChatSettingViewModel: PropertyChangedBase
{
    private string key;
    private string baseUrl = "https://api.openai.com/";
    private readonly IReadOnlyList<string> _aiTypes;
    private string aiType;
    private readonly IReadOnlyList<ProviderType> _apiProviders;
    private ProviderType apiProvider;
    private string deploymentId;
    public IReadOnlyCollection<string> AiTypes => _aiTypes;
    public string AiType
    {
        get => aiType;
        set => SetAndNotify(ref aiType, value);
    }
    public IReadOnlyCollection<ProviderType> ApiProviders => _apiProviders;
    
    public ProviderType ApiProvider
    {
        get => apiProvider;
        set
        {
            SetAndNotify(ref apiProvider, value);
            NotifyOfPropertyChange(nameof(IsShowDeployment));
        }
    }
    
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

    public string DeploymentId
    {
        get => deploymentId;
        set => SetAndNotify(ref deploymentId, value);
    }

    public bool IsShowDeployment => apiProvider == ProviderType.Azure;

    public ChatSettingViewModel()
    {
        _aiTypes = [OpenAI.ObjectModels.Models.Gpt_3_5_Turbo, OpenAI.ObjectModels.Models.Gpt_4, OpenAI.ObjectModels.Models.Gpt_4_32k];
        aiType = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo;
        _apiProviders = [ProviderType.OpenAi, ProviderType.Azure];
        apiProvider = ProviderType.OpenAi;
    }

}