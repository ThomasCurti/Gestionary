using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionaryWebsite.Dbo
{
    public class ClientLogs : IObjectWithId
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string Logtype { get; set; }
        public string Message { get; set; }
    }
}
