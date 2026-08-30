using MySarAssistModels.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MySARAssist.Converters
{
    public static class WebServiceConverters
    {
        public static Organization OrganizationFromWebserviceOrg(this sca_web_service_reference.Organization webOrg)
        {
            Organization organization = new Organization();

            organization.OrganizationID = webOrg.OrganizationID;
            organization.OrganizationName = webOrg.OrganizationName;
            
            organization.LogoFileName = webOrg.LogoFileName;
            organization.ParentOrganizationID = webOrg.ParentOrganizationID;


            return organization;
        }

    }
}
