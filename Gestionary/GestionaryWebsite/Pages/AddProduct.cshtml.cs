using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using GestionaryWebsite.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using GestionaryWebsite.Dbo;
using System.IO;

namespace GestionaryWebsite
{
    public class AddProductModel : PageModel
    {

        public string userName;

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [Required]
        public long Type { get; set; }

        [BindProperty]
        [Required]
        public decimal Price { get; set; }

        [BindProperty]
        [Required]
        public int Stock { get; set; }

        [BindProperty]
        [Required]
        public string Image { get; set; }

        public const string MessageKey = nameof(MessageKey);

        public List<SelectListItem> TypeOptions { get; set; }
        public List<SelectListItem> Images { get; set; }

        private IStockRepository _stockRepository { get; }
        private IEmployeeRepository _employeeRepository { get; }
        private ILogRepository _logRepository { get; }

        public AddProductModel(IStockRepository stockRepository, IEmployeeRepository employeeRepository, ILogRepository logRepository)
        {
            _stockRepository = stockRepository;
            _employeeRepository = employeeRepository;
            _logRepository = logRepository;
        }

        private void resetTypeDropdownList()
        {
            TypeOptions = _stockRepository.GetTypes().Select(t =>
                                  new SelectListItem
                                  {
                                      Value = t.Id.ToString(),
                                      Text = t.Name
                                  }).ToList();

            Images = Directory.EnumerateFiles("wwwroot/Pics/").Select(item => 
                                  new SelectListItem
                                  {
                                      Value = item.Split("/").Last(),
                                      Text = item.Split("/").Last()
                                  }).ToList();
        }

        public IActionResult OnGet()
        {
            userName =  Tools.checkUser(Request.Cookies["user"], _employeeRepository, 1);
            if(userName == null)
                return RedirectToPage("/Login");

            resetTypeDropdownList();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            resetTypeDropdownList();

            if (!ModelState.IsValid)
            {
                TempData[MessageKey] = "Le formulaire n'est pas valide.";
                return Page();
            }

            await _stockRepository.Insert(new Dbo.ClientItems()
            {
                Name = Name,
                Price = Price,
                Stock = Stock,
                TypeId = Type,
                PicName = Image
            });

            ClientLogs log = new ClientLogs
            {
                Id = 0,
                Date = DateTime.Now,
                Class = this.GetType().Name,
                Type = "Add",
                Logtype = LoggerType.INFO.GetFriendlyName(),
                Message = "Product: " + Name + " has been added to the Stock base",
            };

            await _logRepository.Insert(log);

            TempData[MessageKey] = "Le produit a bien été enregistré !";
            ModelState.Clear();
            return Page();
        }
    }
}