/* Unmerged change from project 'MySARAssist (net8.0-ios)'
Added:
using MySARAssist.Models.Personnel;
using MySARAssist.Models.Personnel.Personnel;
using MySARAssist.Models.Personnel.Personnel.Personnel;
*/

/* Unmerged change from project 'MySARAssist (net8.0-android)'
Added:
using MySARAssist.Models.Personnel;
using MySARAssist.Models.Personnel.Personnel;
*/

/* Unmerged change from project 'MySARAssist (net8.0-windows10.0.19041.0)'
Added:
using MySARAssist.Models.Personnel;
*/
using MetroLog.Maui;
using Microsoft.Extensions.Logging;
using MySARAssist.Converters;
using MySARAssist.Services;
using MySarAssistModels.Interfaces;
using MySarAssistModels.People;
using MySarAssistModels.Services;

namespace MySARAssist
{
    public partial class App : Application
    {
        private readonly PersonnelService _personnelService;
        private readonly ILogger<MainPage> _logger;

        public static Personnel? CurrentPerson { get; set; }


        public App(ILogger<MainPage> logger)
        {
            InitializeComponent();
            this._personnelService = new PersonnelService();

             MainPage = new AppShell();


            // Set optional agent configuration
            // Options are: crashReportingEnabled, loggingEnabled, logLevel, collectorAddress, crashCollectorAddress
            // AgentStartConfiguration agentConfig = new AgentStartConfiguration(true, true, LogLevel.INFO, "mobile-collector.newrelic.com", "mobile-crash.newrelic.com");



            LogController.InitializeNavigation(
                page => MainPage!.Navigation.PushModalAsync(page),
                () => MainPage!.Navigation.PopModalAsync());

            this._logger = logger;

            LoadCurrentPerson();
            Task.Run(() => CreateInitialOrganizationsAsNeeded()).Wait();

            // Sync organizations from the remote API in the background instead of
            // blocking the UI thread — a slow/unreachable network here was holding
            // up app startup long enough to trigger an ANR.
            _ = Task.Run(async () =>
            {
                try
                {
                    await UpdateOrganizationsFromAPI();
                }
                catch (Exception ex)
                {
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ex.ToString());
                }
            });

        }

        private async Task CreateInitialOrganizationsAsNeeded()
        {
            OrganizationService service = new OrganizationService();
            List<Organization> orgs = null;

            try
            {
                orgs = (await service.GetItems()).ToList();

            } catch
            {
                orgs = new List<Organization>();
            }

                List<Organization> staticOrgs = OrganizationTools.GetStaticOrganizations(Guid.Empty);
            List<Organization> parentOrgs = OrganizationTools.GetStaticParentOrganizations();
                
            if (!orgs.Any() || !orgs.Any(o=>o.ParentOrganizationID == Guid.Empty) || orgs.Count < staticOrgs.Count + parentOrgs.Count)
            {
                //get the static orgs and save them

                staticOrgs.AddRange(parentOrgs);
                Guid bcsara = new Guid("CC3A9DC9-01A3-4D39-B806-2128B51120BC");
                int bcsaraCount = staticOrgs.Count(o => o.OrganizationID == bcsara);
                foreach(Organization org in staticOrgs)
                {
                    await service.UpsertItemAsync(org);
                }
            }


            


        }

        private async Task UpdateOrganizationsFromAPI()
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;

            if (accessType == NetworkAccess.Internet)
            {
                var syncService = new OrganizationSyncService(new RestService(), new OrganizationService());
                await syncService.SyncOrganizationsAsync();
            }
        }

        private async void LoadCurrentPerson()
        {
            CurrentPerson = await _personnelService.GetCurrentPersonAsync();
            if(CurrentPerson == null)
            {
                ;
            }
        }
    }
}
