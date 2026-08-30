using MySarAssistModels.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySarAssistModels.Interfaces
{
    public interface IRestService
    {
        Task<List<Organization>?> GetOrganizationsAsync();
    }
}