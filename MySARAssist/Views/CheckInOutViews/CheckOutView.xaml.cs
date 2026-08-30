using Microsoft.Extensions.Logging;
using MySARAssist.Services;
using Plugin.Maui.ScreenBrightness;

namespace MySARAssist.Views.CheckInOut;

public partial class CheckOutView : ContentPage
{
    private readonly ILogger<MainPage> logger;
    float _lastBrightness = 0.5f;

    public CheckOutView(ILogger<MainPage> logger)
	{
        try
        {
            this.logger = logger;
            InitializeComponent();
            _lastBrightness = ScreenBrightness.Default.Brightness;
            ScreenBrightness.Default.Brightness = 1;



        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in CheckOutView constructor");
        }
    }

    //cancel brightness change if user navigates away
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ScreenBrightness.Default.Brightness = _lastBrightness;
    }

}