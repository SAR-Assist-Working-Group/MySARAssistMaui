using MetroLog.Maui;
using Microsoft.Extensions.Logging;
using MySARAssist.ViewModels.UtilitiesViewModels;

namespace MySARAssist.Views.Utilities;

public partial class AltimeterPage : ContentPage
{
    CancellationTokenSource cts;
    private readonly ILogger<MainPage> logger;

    AltimeterViewModel viewModel = null;

    public AltimeterPage(ILogger<MainPage> logger)
	{
		InitializeComponent();
        LogController.SuspendShake();
        this.logger = logger;
        cts = new CancellationTokenSource();

        viewModel = new AltimeterViewModel();
        this.BindingContext = viewModel;
    }
}