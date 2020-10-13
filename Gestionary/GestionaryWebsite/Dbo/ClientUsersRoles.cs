using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionaryWebsite.Dbo
{
    public class ClientUsersRoles
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public long RoleId { get; set; }
    }
}
