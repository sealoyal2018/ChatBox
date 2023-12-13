using System.Threading.Tasks;
using ChatBox.Interfaces;
using Stylet;

namespace ChatBox.Modules.Paints.ViewModels;

public class HomeViewModel : Screen, IAppModule
{
    public PaintSettingViewModel PaintSettingViewModel { get; }
    public string Icon => "fa-solid fa-palette";
    public int Sort => 20;

    public HomeViewModel(PaintSettingViewModel paintSettingViewModel)
    {
        PaintSettingViewModel = paintSettingViewModel;
        DisplayName = "绘图";
    }

    
    
    public async Task Test()
    {
    }
}