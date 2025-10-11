using Microsoft.Extensions.Logging;
using MySARAssist.Services;
using Plugin.Maui.ScreenBrightness;

namespace MySARAssist.Views.CheckInOut;

public partial class CheckInView : ContentPage
{
    private readonly ILogger<MainPage> logger;
    float _lastBrightness = 0.5f;

    public CheckInView(ILogger<MainPage> logger)
	{try
        {
            this.logger = logger;
            InitializeComponent();
            _lastBrightness = ScreenBrightness.Default.Brightness;
            ScreenBrightness.Default.Brightness = 1;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in CheckInView.xaml.cs");
        }
        // Example usage in your MAUI application



        // (Optional) Later on reset the brightness to what it was
        //brightnessService.SetBrightness(previousBrightness);

    }

    //cancel brightness change if user navigates away
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ScreenBrightness.Default.Brightness = _lastBrightness;
    }


}