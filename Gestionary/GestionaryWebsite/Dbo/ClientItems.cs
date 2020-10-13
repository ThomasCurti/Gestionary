namespace GestionaryWebsite.Dbo
{
    public class ClientItems : IObjectWithId
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Stock { get; set; }
        public long TypeId { get; set; }
        public string PicName { get; set; }
    }
}
