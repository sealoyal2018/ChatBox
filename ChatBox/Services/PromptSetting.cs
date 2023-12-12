using System.Windows.Input;

using ChatBox.Interfaces;

namespace ChatBox.Services;
internal class PromptSetting : ISetting
{
    public string DisplayName => "提示词";

    public string Icon => "fa-solid fa-robot";

    public int Sort => 5;

    public ICommand ActiveCommand { get; }

    public PromptSetting()
    {
        ActiveCommand = new RelayCommand(Execute, CanExecute);
    }

    private void Execute()
    {
    }

    private bool CanExecute() => true;
}
