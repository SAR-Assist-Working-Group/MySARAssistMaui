using Microsoft.Extensions.Logging;

namespace MySARAssist.Views;

public partial class SettingsView : ContentPage
{
    private readonly ILogger<MainPage> logger;

    public SettingsView(ILogger<MainPage> logger)
    {
        InitializeComponent();
        this.logger = logger;
    }
}
