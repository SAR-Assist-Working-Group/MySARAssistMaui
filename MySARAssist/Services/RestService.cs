using MySARAssist.Converters;
using MySarAssistModels.Interfaces;
using sca_web_service_reference;
using System.Diagnostics;

namespace MySARAssist.Services
{
    public class RestService : IRestService
    {
        public List<Organization>? Items { get; private set; }

        /// <inheritdoc/>
        public async Task<List<MySarAssistModels.People.Organization>?> GetOrganizationsAsync()
        {
            List<Organization>? wsOrgs = await RefreshDataAsync();
            return wsOrgs?
                .Select(o => o.OrganizationFromWebserviceOrg())
                .Where(o => o != null)
                .ToList()!;
        }

        public async Task<List<Organization>?> RefreshDataAsync()
        {
            List<Organization> parentOrgs = new List<Organization>();
            List<Organization> childOrgs = new List<Organization>();

            try
            {
                CAUpdatesWebserviceSoapClient SCAWebServiceClient = new CAUpdatesWebserviceSoapClient(
                    CAUpdatesWebserviceSoapClient.EndpointConfiguration.ICAUpdatesWebserviceSoap);
                var results = await SCAWebServiceClient.GetAllOrganizationsAsync();

                foreach (Organization org in results.Result)
                {
                    if (org.ParentOrganizationID == Guid.Empty)
                    {
                        if (parentOrgs.Any(o => o.OrganizationID == org.OrganizationID)) continue;
                        parentOrgs.Add(org);
                    }
                    else
                    {
                        if (childOrgs.Any(o => o.OrganizationID == org.OrganizationID)) continue;
                        childOrgs.Add(org);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            Items = new List<Organization>();
            Items.AddRange(parentOrgs);
            Items.AddRange(childOrgs);
            return Items;
        }
    }
}