using AutoMapper;

namespace GestionaryWebsite.DataAccess
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Dbo.ClientItems, EfModels.Items>();
            CreateMap<EfModels.Items, Dbo.ClientItems>();

            CreateMap<Dbo.ClientLogs, EfModels.Logs>();
            CreateMap<EfModels.Logs, Dbo.ClientLogs>();

            CreateMap<Dbo.ClientRoles, EfModels.Roles>();
            CreateMap<EfModels.Roles, Dbo.ClientRoles>();

            CreateMap<Dbo.ClientTypes, EfModels.Types>();
            CreateMap<EfModels.Types, Dbo.ClientTypes>();

            CreateMap<Dbo.ClientUsers, EfModels.Users>();
            CreateMap<EfModels.Users, Dbo.ClientUsers>();

            CreateMap<Dbo.ClientItemsTypes, EfModels.ItemsTypes>();
            CreateMap<EfModels.ItemsTypes, Dbo.ClientItemsTypes>();

            CreateMap<Dbo.ClientUsersRoles, EfModels.UsersRoles>();
            CreateMap<EfModels.UsersRoles, Dbo.ClientUsersRoles>();

            CreateMap<Dbo.ClientStockItems, EfModels.StockItems>();
            CreateMap<EfModels.StockItems, Dbo.ClientStockItems>();
        }
    }
}
