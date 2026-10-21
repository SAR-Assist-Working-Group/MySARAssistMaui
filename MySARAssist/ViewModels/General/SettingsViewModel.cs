using CommunityToolkit.Mvvm.ComponentModel;
using MySARAssist.Services;

namespace MySARAssist.ViewModels.General
{
    /// <summary>
    /// Backs the settings page. The unit choice lives on the shared <see cref="UnitSettings"/>
    /// singleton, so binding straight through it keeps every other page in step.
    /// </summary>
    public class SettingsViewModel : ObservableObject
    {
        public UnitSettings Units => UnitSettings.Current;
    }
}
