using MetroLog.Maui;
using Microsoft.Extensions.Logging;
using MySARAssist.Services;
using MySARAssist.Views.Calculators;
using System.Globalization;

namespace MySARAssist.Views;

public partial class CalculatorsView : ContentPage
{
    private readonly ILogger<MainPage> logger;

    public CalculatorsView(ILogger<MainPage> logger)
	{
		InitializeComponent();
        LogController.ResumeShakeIfNeeded();
        this.logger = logger;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LogController.ResumeShakeIfNeeded();

        string distanceName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(UnitSettings.Current.ShortDistanceName);
        Pacing_Button.Text = $"Paces to {distanceName}";
        DistToPacing_Button.Text = $"{distanceName} to Paces";
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width > height)
        {
            slPageContent.Orientation = StackOrientation.Horizontal;
        }
        else
        {
            slPageContent.Orientation = StackOrientation.Vertical;
        }

    }

    private async void CoordinateConverterButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(Views.CalculatorsView)}/{nameof(Views.Calculators.CoordinateConverterView)}");

    }


    private async void GridSearch_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(Views.CalculatorsView)}/{nameof(Views.Calculators.GridSearchView)}");
    }

    private async void LinearSearch_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(Views.CalculatorsView)}/{nameof(Views.Calculators.LinearSearchView)}");
    }

    private async void VisualSearchResources_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(Views.CalculatorsView)}/{nameof(Views.Calculators.VisualSearchResourceEstimationView)}");

    }

    private async void Pacing_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(Views.CalculatorsView)}/{nameof(Views.Calculators.PacingToDistancePage)}");

    }

    private async void VisualSearch_Button_Clicked(object sender, EventArgs e)
    {
        //await Navigation.PushAsync(new Views.Calculators.SweepWidthCalculatorView());
        await Shell.Current.GoToAsync($"{nameof(Views.CalculatorsView)}/{nameof(Views.Calculators.SweepWidthCalculatorView)}");

    }


    private async void DistToPacing_Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(Views.CalculatorsView)}/{nameof(Views.Calculators.DistanceToPacingPage)}");

    }
}