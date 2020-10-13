using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionaryWebsite.DataAccess.EfModels
{
    public partial class UsersRoles
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public long RoleId { get; set; }
    }
}
