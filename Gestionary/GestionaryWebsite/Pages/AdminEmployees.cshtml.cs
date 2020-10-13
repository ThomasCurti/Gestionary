using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GestionaryWebsite.DataAccess.Interfaces;
using GestionaryWebsite.Dbo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionaryWebsite
{
    public class AdminEmployeesModel : PageModel
    {
        public string userName;

        [BindProperty(SupportsGet = true)]
        [Required]
        public string EmployeeId { get; set; }

        private IEmployeeRepository _employeeRepository { get; }
        private ILogRepository _logRepository { get; }
   

        public AdminEmployeesModel(IEmployeeRepository employeeRepository, ILogRepository logRepository)
        {
            _employeeRepository = employeeRepository;
            _logRepository = logRepository;
        }
        public IActionResult OnGet()
        {
            userName = Tools.checkUser(Request.Cookies["user"], _employeeRepository, 2);
            if (userName == null)
                return RedirectToPage("/Login");
            return Page();
        }

        public IEnumerable<Tuple<ClientUsers, string>> GetAllUser()
        {

            var listClientUsers = _employeeRepository.Get().Result;
            var roles = _employeeRepository.GetRoles();

            List<Tuple<ClientUsers, string>> res = new List<Tuple<ClientUsers, string>>();
            foreach (var item in listClientUsers)
            {
                res.Add(new Tuple<ClientUsers, string>(item, getRoleName(item.RoleId, roles)));
            }
            return res;
        }

        private string getRoleName(long roleId, List<ClientUsersRoles> roles)
        {
            var role = roles.Where(role => role.RoleId == roleId).FirstOrDefault();
            if (role == null)
                return "None";
            return role.RoleName;
        }

        public async Task<IActionResult> OnPost()
        {
            ClientUsers employee = _employeeRepository.Get().Result.Where(emp => emp.Id == Int64.Parse(EmployeeId)).FirstOrDefault();
            await _employeeRepository.Delete(Int64.Parse(EmployeeId));

            ClientLogs log = new ClientLogs {
                Id = 0,
                Date = DateTime.Now,
                Class = this.GetType().Name,
                Type = "Delete",
                Logtype = LoggerType.INFO.GetFriendlyName(),
                Message = "Employee: " + employee == null ? "" : employee.Username + " has been removed from the employee base",
            };

            await _logRepository.Insert(log);

            userName = Tools.checkUser(Request.Cookies["user"], _employeeRepository, 2);
            if (userName == null)
                return RedirectToPage("/Login");

            return Page();
        }
    }
}