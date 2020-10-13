using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GestionaryWebsite.DataAccess.Interfaces;
using GestionaryWebsite.Dbo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GestionaryWebsite
{
    public class LoginModel : PageModel
    {

        private readonly IEmployeeRepository _employeeRepository;

        [BindProperty(SupportsGet = true)]
        [Required]
        public string login { get; set; }
        [BindProperty(SupportsGet = true)]
        [Required]
        public string password { get; set; }

        private ILogRepository _logRepository;

        public LoginModel(IEmployeeRepository employeeRepository, ILogRepository logRepository)
        {
            _employeeRepository = employeeRepository;
            _logRepository = logRepository;
        }

        public void OnGet()
        {
            Response.Cookies.Delete("user");
            Response.Cookies.Delete("userName");
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            if (login == null || password == null)
                return Page();

            var employees = _employeeRepository.Get().Result;

            if (employees == null)
                return Page();

            var employee = employees.Where(emp => emp.Username == login).FirstOrDefault();

            if (employee == null)
                return Page();

            if (Tools.CheckPassword(employee.PasswordHash, password) == false)
            {
                ClientLogs l = new ClientLogs
                {
                    Id = 0,
                    Date = DateTime.Now,
                    Class = this.GetType().Name,
                    Type = "Login",
                    Logtype = LoggerType.WARNING.GetFriendlyName(),
                    Message = "Employee: " + login + " tried to connect but typed a wrong password",
                };

                await _logRepository.Insert(l);
                return Page();
            }
                

            WriteCookie("user", employee.Id.ToString());
            WriteCookie("userName", employee.Username);

            ClientLogs log = new ClientLogs
            {
                Id = 0,
                Date = DateTime.Now,
                Class = this.GetType().Name,
                Type = "Login",
                Logtype = LoggerType.INFO.GetFriendlyName(),
                Message = "Employee: " + login + " connected successfully",
            };

            await _logRepository.Insert(log);

            //Connected
            return RedirectToPage("./Index");

        }

        private void WriteCookie(string cookiename, string cookieValue)
        {
            CookieOptions cookie = new CookieOptions();
            cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append(cookiename, cookieValue);
        }

        
    }
}