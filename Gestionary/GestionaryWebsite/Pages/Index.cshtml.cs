using GestionaryWebsite.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionaryWebsite.Pages
{
    public class IndexModel : PageModel
    {
        public string userName;
        public string UserNameAdmin;

        private IEmployeeRepository _employeeRepository { get; }


        public IndexModel(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public void OnGet()
        {
            userName = Request.Cookies["userName"];
            UserNameAdmin = Tools.checkUser(Request.Cookies["user"], _employeeRepository, 2);
        }
    }
}
