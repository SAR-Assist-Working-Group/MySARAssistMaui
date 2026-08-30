using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySARAssist.ViewModels.UtilitiesViewModels
{
    public class UtilitiesListViewModel : ObservableObject
    {
        public UtilitiesListViewModel()
        {
            NavigateToAltitudeCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync($"{nameof(Views.Utilities.AltimeterPage)}");
            });
        }
        public Command NavigateToAltitudeCommand { get; }

    }
}
