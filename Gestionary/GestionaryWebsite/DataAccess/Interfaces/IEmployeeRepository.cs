using System.Collections.Generic;

namespace GestionaryWebsite.DataAccess.Interfaces
{
    public interface IEmployeeRepository : IRepository<EfModels.Users, Dbo.ClientUsers>
    {
        public List<Dbo.ClientUsersRoles> GetRoles();

        public List<Dbo.ClientRoles> GetTypeRoles();
    }
}
