using AutoMapper;
using GestionaryWebsite.DataAccess.Interfaces;
using GestionaryWebsite.Dbo;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GestionaryWebsite.DataAccess
{
    public class EmployeeRepository : Repository<EfModels.Users, Dbo.ClientUsers>, IEmployeeRepository
    {
        public EmployeeRepository(GestionaryContext context, ILogger<EmployeeRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public List<ClientUsersRoles> GetRoles()
        {
            var result = _context.UsersRoles.ToList();
            return _mapper.Map<List<ClientUsersRoles>>(result);
        }
        public List<ClientRoles> GetTypeRoles()
        {
            var result = _context.Roles.ToList();
            return _mapper.Map<List<ClientRoles>>(result); ;
        }
    }
}
