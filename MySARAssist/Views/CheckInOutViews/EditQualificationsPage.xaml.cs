using Microsoft.Extensions.Logging;

namespace MySARAssist.Views.CheckInOut;

[QueryProperty(nameof(PersonnelID), nameof(PersonnelID))]
[XamlCompilation(XamlCompilationOptions.Compile)]

public partial class EditQualificationsPage : ContentPage
{
    private readonly ILogger<MainPage> logger;
    ViewModels.CheckInOut.EditQualificationsViewModel _viewModel;

    public EditQualificationsPage(ILogger<MainPage> logger)
    { try { 

        InitializeComponent();
        _viewModel = new ViewModels.CheckInOut.EditQualificationsViewModel(logger);
        this.BindingContext = _viewModel;
        this.logger = logger;
    }catch (Exception ex)
        {
            logger.LogError(ex, "Error in EditQualificationsPage constructor");
        }
    }

    public string PersonnelID
    {
        set
        {
            string temp = Uri.UnescapeDataString(value ?? string.Empty);
            if (!string.IsNullOrEmpty(temp))
            {
                try { _viewModel.TeamMemberID = new Guid(temp); }
                catch (Exception ex) { 
                    _viewModel.TeamMemberID = Guid.NewGuid();
                    logger.LogError(ex, "Error in EditQualificationsPage.PersonnelID.set");
                }
            }
        }
    }

}