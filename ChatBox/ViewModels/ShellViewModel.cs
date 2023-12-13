using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using ChatBox.Interfaces;
using Stylet;

namespace ChatBox.ViewModels;
public class ShellViewModel : Conductor<IAppModule>.Collection.OneActive
{
    private int currentModuleIndex = 0;
    public List<IAppModule> AppModules { get; }

    public int CurrentModuleIndex
    {
        get => currentModuleIndex;
        set => SetAndNotify(ref currentModuleIndex, value);
    }
    
    public ShellViewModel(IEnumerable<IAppModule> appModules)
    {
        AppModules = appModules.OrderBy(v=> v.Sort).ToList();
        CurrentModuleIndex = 1;
        var currentModule = AppModules[CurrentModuleIndex];
        ActiveItem = currentModule;
    }

    public void OpenModule(PointerReleasedEventArgs e)
    {
        if (e.Pointer.IsPrimary && e.Source is Border { DataContext: IAppModule module } && ActiveItem != module)
        {
            ActivateItem(module);
            e.Handled = true;
        }
    }
    
}
