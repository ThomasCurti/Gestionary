using System;
using System.Collections.Generic;

namespace GestionaryWebsite.DataAccess.EfModels
{
    public partial class ItemsTypes
    {
        public string ItemName { get; set; }
        public long ItemId { get; set; }
        public string TypeName { get; set; }
        public long TypeId { get; set; }
    }
}
