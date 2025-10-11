using sca_web_service_reference;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task<List<Organization>?> TestRefreshDataAsync()
        {
            List<Organization> parentOrgs = new List<Organization>();
            List<Organization> childOrgs = new List<Organization>();

            try
            {
                var results = new sca_web_service_reference.TaskOfListOfOrganization().Result;
                foreach (Organization org in results)
                {
                    if (org.ParentOrganizationID == Guid.Empty)
                    {
                        if (parentOrgs.Any(o => o.OrganizationID == org.OrganizationID))
                        {
                            continue;
                        }
                        parentOrgs.Add(org);
                        //Items.AddRange(await GetChildOrganizationsAsync(org.OrganizationID));
                    }
                    else
                    {
                        if (childOrgs.Any(o => o.OrganizationID == org.OrganizationID))
                        {
                            continue;
                        }
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


        public async Task<List<Organization>?> RefreshDataAsync()
        {
            List<Organization> parentOrgs = new List<Organization>();
            List<Organization> childOrgs = new List<Organization>();

            try
            {
                CAUpdatesWebserviceSoapClient SCAWebServiceClient = new CAUpdatesWebserviceSoapClient(CAUpdatesWebserviceSoapClient.EndpointConfiguration.ICAUpdatesWebserviceSoap);
                var results = await SCAWebServiceClient.GetAllOrganizationsAsync();


                foreach (Organization org in results.Result)
                {
                    if (org.ParentOrganizationID == Guid.Empty)
                    {
                        if (parentOrgs.Any(o => o.OrganizationID == org.OrganizationID))
                        {
                            continue;
                        }
                        parentOrgs.Add(org);
                        //Items.AddRange(await GetChildOrganizationsAsync(org.OrganizationID));
                    }
                    else
                    {
                        if (childOrgs.Any(o => o.OrganizationID == org.OrganizationID))
                        {
                            continue;
                        }
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
