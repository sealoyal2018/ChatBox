using System.Windows.Input;

namespace ChatBox.Interfaces;

public interface ISetting
{
    string DisplayName { get; }
    string Icon { get; }
    int Sort { get; }
    ICommand ActiveCommand { get; }
}