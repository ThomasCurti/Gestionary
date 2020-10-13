using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GestionaryWebsite.DataAccess.Interfaces;
using GestionaryWebsite.Dbo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GestionaryWebsite
{
    public class EmployeeProfileModel : PageModel
    {

        public  string userName;

        private static string userNAME;
       
        [BindProperty(SupportsGet = true)]
        [Required]
        public string oldPassword { get; set; }

        [BindProperty(SupportsGet = true)]
        [Required]
        public string newPassword { get; set; }
        [BindProperty(SupportsGet = true)]
        [Required]
        public string confirmPassword { get; set; }

        private IEmployeeRepository _employeeRepository { get; }
        private ILogRepository _logRepository { get; }

        public const string MessageKey = nameof(MessageKey);
        public EmployeeProfileModel(IEmployeeRepository employeeRepository, DataAccess.Interfaces.ILogRepository logRepository)
        {
            _employeeRepository = employeeRepository;
            _logRepository = logRepository;
        }
        public IActionResult OnGet()
        {
            if (Request.Cookies["user"] == null)
                return RedirectToPage("/Login");

            long userId = long.Parse(Request.Cookies["user"]);
            var listClientUsers = _employeeRepository.Get().Result;
          
            foreach(var item in listClientUsers)
            {
                if(item.Id == userId)
                {
                    userName = item.Username;
                    userNAME = item.Username;
                    break;
                }
            }
            if(userName == null)
                 return RedirectToPage("/Login");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData[MessageKey] = "Le formulaire n'est pas valide.";
                return Page();
            }

            var employees = _employeeRepository.Get().Result;
            var employee = employees.Where(emp => emp.Username == userNAME).FirstOrDefault();
            if (Tools.CheckPassword(employee.PasswordHash, oldPassword) == false )
            {
                TempData[MessageKey] = "Votre ancien mot de passe n'est pas correct.";
                return Page();
            }
               

            if (newPassword != confirmPassword)
            {
                TempData[MessageKey] = "Les 2 champs nouveaux mots de passe ne correspondent pas.";
                return Page();
            }
               

            newPassword = Tools.Hash(newPassword);
            Dbo.ClientUsers newUser = new Dbo.ClientUsers
            {
                Id = employee.Id,
                Username = employee.Username,
                PasswordHash = newPassword,
                RoleId = employee.RoleId
            };
            var result = _employeeRepository.Update(newUser);


            ClientLogs log = new ClientLogs
            {
                Id = 0,
                Date = DateTime.Now,
                Class = this.GetType().Name,
                Type = "employee.Username",
                Logtype = LoggerType.INFO.GetFriendlyName(),
                Message = "Employee: " + employee.Username + " bas been update to the base",
            };

            await _logRepository.Insert(log);
            TempData[MessageKey] = "Votre profil a bien été éditer !";
            ModelState.Clear();
            OnGet();

            return Page();
        }

    }
}