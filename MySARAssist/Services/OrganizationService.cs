using MySarAssistModels.Interfaces;
using MySarAssistModels.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using MySARAssist.Models.Exceptions;

namespace MySARAssist.Services
{
    public class OrganizationService : IDataStore<Organization>
    {
        SQLiteAsyncConnection conn;

        public OrganizationService()
        {
            conn = new SQLiteAsyncConnection(Constants.dbPath);

        }

        public bool AreOrgsEqual(Organization org1, Organization org2)
        {
            var properties = from p in typeof(Organization).GetProperties()
                             where p.PropertyType == typeof(string) &&
                                   p.CanRead &&
                                   p.CanWrite
                             select p;

            foreach (var property in properties)
            {
                var value = property.GetValue(org1, null);
                var value2 = property.GetValue(org2, null);
                if (value != value2)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> AddItemAsync(Organization item)
        {
            await conn.CreateTableAsync<Organization>();

            int check = await conn.InsertAsync(item);
            if (check > 0) { return true; }
            else { throw new Exception("Error adding item"); }// return false; }
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            await conn.CreateTableAsync<Organization>();
            Organization itemToDelete = await conn.Table<Organization>().FirstOrDefaultAsync(o => o.OrganizationID == id);

            if (itemToDelete == null) { return false; }
            else
            {
                await conn.DeleteAsync(itemToDelete);
                return true;
            }
        }

        public async Task<Organization> GetItemAsync(Guid id)
        {
            await conn.CreateTableAsync<Organization>();
            Organization p = await conn.Table<Organization>().FirstOrDefaultAsync(o => o.OrganizationID == id);
           
            if (p == null) { throw new NotFoundException(id.ToString()); }
            return p;
        }

        public async Task<IEnumerable<Organization>> GetItems(bool forceRefresh = false)
        {
            await conn.CreateTableAsync<Organization>();

            List<Organization> list = await conn.Table<Organization>().ToListAsync();
            if (list != null && list.Any())
            {
                list = list.OrderBy(o => o.OrganizationName).ToList();

                return list;
            }
            else
            {
                throw new NotFoundException("No records found");
            }
        }



        public async Task<IEnumerable<Organization>> GetItems(Guid ParentID)
        {
            await conn.CreateTableAsync<Organization>();

            List<Organization> list = await conn.Table<Organization>().Where(o=>o.ParentOrganizationID == ParentID).ToListAsync();
            if (list != null && list.Any())
            {
                list = list.OrderBy(o => o.OrganizationName).ToList();

                return list;
            }
            else
            {
                throw new NotFoundException("No records found");
            }
        }

        public async Task<bool> UpdateItemAsync(Organization item)
        {
            await conn.CreateTableAsync<Organization>();

            int check = await conn.UpdateAsync(item);
            if (check > 0) { return true; }
            else { throw new Exception("Error updating item"); }
        }

        public async Task<bool> UpsertItemAsync(Organization item)
        {
            await conn.CreateTableAsync<Organization>();
            bool exists = await conn.Table<Organization>().CountAsync(o => o.OrganizationID == item.OrganizationID) > 0;
            if (exists)
            {
                return await UpdateItemAsync(item);
            }
            else
            {
                return await AddItemAsync(item);
            }
        }
    }
}
