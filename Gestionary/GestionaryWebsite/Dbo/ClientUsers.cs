namespace GestionaryWebsite.Dbo
{
    public class ClientUsers : IObjectWithId
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public long RoleId { get; set; }
    }
}
