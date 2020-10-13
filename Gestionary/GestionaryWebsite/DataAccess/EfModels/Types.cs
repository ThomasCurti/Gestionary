using System;
using System.Collections.Generic;

namespace GestionaryWebsite.DataAccess.EfModels
{
    public partial class Types
    {
        public Types()
        {
            Items = new HashSet<Items>();
        }

        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Items> Items { get; set; }
    }
}
