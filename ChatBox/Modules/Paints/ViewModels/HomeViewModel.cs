using ChatBox.Interfaces;
using Stylet;

namespace ChatBox.Modules.Paints.ViewModels;

public class HomeViewModel : Screen, IAppModule
{
    public string Icon => "fa-solid fa-palette";
    public int Sort => 20;

    public HomeViewModel()
    {
        DisplayName = "绘图";
    }
}