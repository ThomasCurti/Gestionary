using AutoMapper;
using GestionaryWebsite.DataAccess.Interfaces;
using GestionaryWebsite.Dbo;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace GestionaryWebsite.DataAccess
{
    public class StockRepository : Repository<EfModels.Items, Dbo.ClientItems>, IStockRepository
    {
        public StockRepository(GestionaryContext context, ILogger<StockRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public List<ClientItemsTypes> GetItemTypes()
        {
            var result = _context.ItemsTypes.ToList();
            return _mapper.Map<List<ClientItemsTypes>>(result);
        }

        public List<ClientStockItems> GetStockItems()
        {
            var result = _context.StockItems.ToList();
            return _mapper.Map<List<ClientStockItems>>(result);
        }

        public List<ClientTypes> GetTypes()
        {
            var result = _context.Types.ToList();
            return _mapper.Map<List<ClientTypes>>(result);
        }
    }
}
