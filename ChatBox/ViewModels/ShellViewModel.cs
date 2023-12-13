using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using ChatBox.Interfaces;
using ChatBox.Modules.Chats.ViewModels;
using Stylet;
using Stylet.Avalonia;

namespace ChatBox.ViewModels;
public class ShellViewModel : Conductor<IAppModule>
{
    private int currentModuleIndex;
    public List<IAppModule> AppModules { get; }

    public int CurrentModuleIndex
    {
        get => currentModuleIndex;
        set => SetAndNotify(ref currentModuleIndex, value);
    }
    
    public ShellViewModel(IEnumerable<IAppModule> appModules)
    {
        AppModules = appModules.OrderBy(v=> v.Sort).ToList();
        CurrentModuleIndex = 0;
        ActiveItem = AppModules.First();
    }
    
}
