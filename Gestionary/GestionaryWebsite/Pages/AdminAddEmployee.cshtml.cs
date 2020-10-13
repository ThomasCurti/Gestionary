using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GestionaryWebsite.Dbo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionaryWebsite
{
    public class AdminAddEmployeeModel : PageModel
    {
        public string userName;

        private readonly DataAccess.Interfaces.IEmployeeRepository _employeeRepository;
        private readonly DataAccess.Interfaces.ILogRepository _logRepository;

        [BindProperty(SupportsGet = true)]
        [Required]
        public string newName { get; set; }
        [BindProperty(SupportsGet = true)]
        [Required]
        public string newPassword { get; set; }
        [BindProperty(SupportsGet = true)]
        [Required]
        public string confirmPassword { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool newRole { get; set; }
        public const string MessageKey = nameof(MessageKey);

        public AdminAddEmployeeModel(DataAccess.Interfaces.IEmployeeRepository employeeRepository, DataAccess.Interfaces.ILogRepository logRepository)
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

        public async Task<IActionResult>  OnPost()
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageKey] = "Le formulaire n'est pas valide.";
                return Page();
            }
            if (newName != null && newPassword == confirmPassword)
            {
                var roles = _employeeRepository.GetTypeRoles();
                var nbRoleAdmin = roles.Where(item => item.Name.ToLower().Contains("admin")).FirstOrDefault();
                var nbRoleEmployee = roles.Where(item => item.Name.ToLower().Contains("employee")).FirstOrDefault();
                
                //default value
                long role = 1;

                if (nbRoleEmployee != null)
                    role = nbRoleEmployee.Id;

                if (newRole && nbRoleAdmin != null)
                    role = nbRoleAdmin.Id;

                

                newPassword =  Tools.Hash(newPassword);
                Dbo.ClientUsers newUser = new Dbo.ClientUsers
                {
                    Username = newName,
                    PasswordHash = newPassword,
                    RoleId = role
                };
                var test = await _employeeRepository.Insert(newUser);


                ClientLogs log = new ClientLogs
                {
                    Id = 0,
                    Date = DateTime.Now,
                    Class = this.GetType().Name,
                    Type = "Add Employee",
                    Logtype = LoggerType.INFO.GetFriendlyName(),
                    Message = "Employee: " + newName + " bas been added to the base",
                };

                await _logRepository.Insert(log);

                return RedirectToPage("./AdminEmployees");
            }
            return Page();

        }
    }
}