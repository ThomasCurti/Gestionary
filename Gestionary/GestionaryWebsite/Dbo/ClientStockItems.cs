namespace GestionaryWebsite.Dbo
{
    public class ClientStockItems : IObjectWithId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Stock { get; set; }
        public string Type { get; set; }
        public string PicName { get; set; }
    }
}
