using MetroLog;
using MetroLog.Maui;
using Microsoft.Extensions.Logging;
using MySARAssist.Services;
using MySARAssist.Views;
using MySARAssist.Views.CheckInOut;

using MySARAssist.Views.UrgencyViews;
using MySarAssistModels.People;

namespace MySARAssist
{
    public partial class MainPage : ContentPage
    {
        private readonly ILogger<MainPage> _logger;

        public MainPage(ILogger<MainPage> logger)
        {
            InitializeComponent();
            BindingContext = new LogController();
            var foo = new LogController();
            foo.IsShakeEnabled = true;

            this._logger = logger;

         


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LogController.ResumeShakeIfNeeded();

        }


        private async void CalculatorsButton_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new Views.CalculatorsView());
            await Shell.Current.GoToAsync($"{nameof(CalculatorsView)}");
            _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Moving to the calculator page");
        }


        private async void CheckInOutButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync( nameof(CheckInOutView));
            _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Moving to the check in page");

        }

        private async void RADeMSButton_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new Views.RADeMSView());
            await Shell.Current.GoToAsync(nameof(RADeMSView));
            _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Moving to the radems page");



        }


        private async void UrgencyButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(UrgencyCalculatorView));
            _logger.Log(Microsoft.Extensions.Logging.LogLevel.Information, "Moving to the urgency assessment page");

        }
    }

}
