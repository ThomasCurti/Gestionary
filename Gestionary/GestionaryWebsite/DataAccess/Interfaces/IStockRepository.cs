using System;
using System.Collections.Generic;

namespace GestionaryWebsite.DataAccess.Interfaces
{
    public interface IStockRepository : IRepository<EfModels.Items, Dbo.ClientItems>
    {
        public List<Dbo.ClientTypes> GetTypes();
        public List<Dbo.ClientItemsTypes> GetItemTypes();
        public List<Dbo.ClientStockItems> GetStockItems();
    }
}
