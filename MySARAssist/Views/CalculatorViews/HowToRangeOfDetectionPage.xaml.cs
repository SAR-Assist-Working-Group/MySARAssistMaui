using MetroLog.Maui;
using Microsoft.Extensions.Logging;
using MySARAssist.Services;

namespace MySARAssist.Views.Calculators;

public partial class HowToRangeOfDetectionPage : ContentPage
{
    private readonly ILogger<MainPage> logger;

    public HowToRangeOfDetectionPage(ILogger<MainPage> logger)
	{
		InitializeComponent();
        LogController.SuspendShake();
        this.logger = logger;
        lblConvertPaces.Text = $"7. Convert the paces to {UnitSettings.Current.ShortDistanceName}, take an average distance and report it to command.";
    }
}