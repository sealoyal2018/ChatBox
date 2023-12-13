using System.Collections.Generic;
using ChatBox.Modules.Paints.Models;
using OpenAI.ObjectModels;

namespace ChatBox.Modules.Paints.ViewModels;

public class PaintSettingViewModel
{
    private IReadOnlyCollection<Ratios> Ratios =>
    [
        new Ratios(StaticValues.ImageStatics.Size.Size256, "avares://ChatBox/Assets/ratios-1x1.png"),
        new Ratios(StaticValues.ImageStatics.Size.Size256, "avares://ChatBox/Assets/ratios-4x5.png"),
        new Ratios(StaticValues.ImageStatics.Size.Size512, "avares://ChatBox/Assets/ratios-2x3.png"),
        new Ratios(StaticValues.ImageStatics.Size.Size512, "avares://ChatBox/Assets/ratios-5x4.png"),
        new Ratios(StaticValues.ImageStatics.Size.Size1024, "avares://ChatBox/Assets/ratios-3x2.png"),
        new Ratios(StaticValues.ImageStatics.Size.Size1024, "avares://ChatBox/Assets/ratios-7x4.png"),
    ];

}