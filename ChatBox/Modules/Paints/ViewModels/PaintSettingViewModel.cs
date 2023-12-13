using System.Collections.Generic;
using ChatBox.Modules.Paints.Models;
using OpenAI.ObjectModels;

namespace ChatBox.Modules.Paints.ViewModels;

public class PaintSettingViewModel
{
    private IReadOnlyCollection<Ratios> Ratios =>
    [
        new Ratios(StaticValues.ImageStatics.Size.Size256, "avarea://ChatBox/Assets/ratios-1x1.png"),
        new Ratios(StaticValues.ImageStatics.Size.Size512, "avarea://ChatBox/Assets/ratios-1x1.png"),
        new Ratios(StaticValues.ImageStatics.Size.Size1024, "avarea://ChatBox/Assets/ratios-1x1.png"),
    ];

}