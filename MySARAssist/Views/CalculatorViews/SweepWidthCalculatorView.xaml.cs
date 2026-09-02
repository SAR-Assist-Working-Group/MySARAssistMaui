using MetroLog.Maui;
using Microsoft.Extensions.Logging;

namespace MySARAssist.Views.Calculators;

public partial class SweepWidthCalculatorView : ContentPage
{
    private readonly ILogger<MainPage> logger;

    public SweepWidthCalculatorView(ILogger<MainPage> logger)
	{
		InitializeComponent();
        LogController.SuspendShake();
        this.logger = logger;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        (BindingContext as ViewModels.Calculators.SweepWidthCalculatorViewModel)?.RefreshUnits();
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        

    }

    private async void HowToRDImageButton_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new Views.Calculators.HowToRangeOfDetectionPage());
        await Shell.Current.GoToAsync($"//{nameof(Views.CalculatorsView)}/{nameof(Views.Calculators.SweepWidthCalculatorView)}/{nameof(Views.Calculators.HowToRangeOfDetectionPage)}");

    }
}