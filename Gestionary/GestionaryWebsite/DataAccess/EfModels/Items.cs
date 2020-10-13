using System;
using System.Collections.Generic;

namespace GestionaryWebsite.DataAccess.EfModels
{
    public partial class Items
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Stock { get; set; }
        public long TypeId { get; set; }
        public string PicName { get; set; }

        public virtual Types Type { get; set; }
    }
}
