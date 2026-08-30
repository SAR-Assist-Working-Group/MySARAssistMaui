
using ServiceReference1;
using System.Text.Json;

namespace MySARAssist.Services
{
    public class RestService
    {
        HttpClient _client;
        JsonSerializerOptions _serializerOptions;

        public List<Organization>? Items { get; private set; }

        public RestService()
        {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

                WriteIndented = true
            };
        }


        public async Task<List<Organization>?> RefreshDataAsync()
        {
            Items = new List<Organization>();

            ServiceReference1.GetParentOrganizationsAsyncRequest request = new ServiceReference1.GetParentOrganizationsAsyncRequest();
            CAUpdatesWebserviceSoapClient client = new CAUpdatesWebserviceSoapClient(CAUpdatesWebserviceSoapClient.EndpointConfiguration.ICAUpdatesWebserviceSoap);
            GetParentOrganizationsAsyncResponse response = await client.GetParentOrganizationsAsyncAsync(request).ConfigureAwait(false);
            if (response.GetParentOrganizationsAsyncResult != null)
            {
                foreach (Organization org in response.GetParentOrganizationsAsyncResult.Result)
                {
                    Items.Add(org);
                }
            }

            List<Organization> childOrgs = new List<Organization>();
            foreach(Organization org in Items)
            {
                childOrgs.AddRange(await GetChildOrganizationsAsync(org.OrganizationID));
            }

            List<Organization> allOrgs = new List<Organization>();
            allOrgs.AddRange(Items);
            allOrgs.AddRange(childOrgs);
            return allOrgs;
        }

        private async Task<List<Organization>> GetChildOrganizationsAsync(Guid Parent)
        {
            Items = new List<Organization>();

            ServiceReference1.GetChildOrganizationsAsyncRequest request = new ServiceReference1.GetChildOrganizationsAsyncRequest(Parent);
            CAUpdatesWebserviceSoapClient client = new CAUpdatesWebserviceSoapClient(CAUpdatesWebserviceSoapClient.EndpointConfiguration.ICAUpdatesWebserviceSoap);
            GetChildOrganizationsAsyncResponse response = await client.GetChildOrganizationsAsyncAsync(request).ConfigureAwait(false);
            if (response.GetChildOrganizationsAsyncResult != null)
            {
                foreach (Organization org in response.GetChildOrganizationsAsyncResult.Result)
                {
                    Items.Add(org);
                }
            }

            return Items;
        }

    }
}
