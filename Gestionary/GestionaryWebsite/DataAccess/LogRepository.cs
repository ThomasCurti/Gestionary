using AutoMapper;
using GestionaryWebsite.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace GestionaryWebsite.DataAccess
{
    public class LogRepository : Repository<EfModels.Logs, Dbo.ClientLogs>, ILogRepository
    {
        public LogRepository(GestionaryContext context, ILogger<LogRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }
    }
}
