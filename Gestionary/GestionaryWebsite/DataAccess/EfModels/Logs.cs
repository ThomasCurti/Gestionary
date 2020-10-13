using System;
using System.Collections.Generic;

namespace GestionaryWebsite.DataAccess.EfModels
{
    public partial class Logs
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string Logtype { get; set; }
        public string Message { get; set; }
    }
}
