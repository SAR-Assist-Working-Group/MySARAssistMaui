using MySarAssistModels.Interfaces;
using MySarAssistModels.People;

namespace MySarAssistModels.Services
{
    public class OrganizationSyncService
    {
        private readonly IRestService _restService;
        private readonly IDataStore<Organization> _orgStore;

        public OrganizationSyncService(IRestService restService, IDataStore<Organization> orgStore)
        {
            _restService = restService;
            _orgStore = orgStore;
        }

        public async Task SyncOrganizationsAsync()
        {
            List<Organization>? syncOrgs = await _restService.GetOrganizationsAsync();

            if (syncOrgs != null)
            {
                foreach (Organization org in syncOrgs)
                {
                    await _orgStore.UpsertItemAsync(org);
                }
            }
        }
    }
}