using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GestionaryWebsite.DataAccess.Interfaces;
using GestionaryWebsite.Dbo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionaryWebsite
{
    public class StockHandlerDetailModel : PageModel
    {
        public string userName;

        public long Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Type { get; set; }

        [BindProperty(SupportsGet = true)]
        public decimal Price { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Stock { get; set; }

        [BindProperty]
        public string PicName { get; set; }

        private IStockRepository _stockRepository { get; }

        private readonly IEmployeeRepository _employeeRepository;

        public List<SelectListItem> TypeOptions { get; set; }
        public List<SelectListItem> Images { get; set; }

        private ILogRepository _logRepository { get; }

        public StockHandlerDetailModel(IStockRepository stockRepository, IEmployeeRepository employeeRepository, ILogRepository logRepository)
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
            TypeOptions.Insert(0, new SelectListItem {Value = "", Text = "Choisir" });

            Images = Directory.EnumerateFiles("wwwroot/Pics/").Select(item =>
                                  new SelectListItem
                                  {
                                      Value = item.Split("/").Last(),
                                      Text = item.Split("/").Last()
                                  }).ToList();

            Images.Insert(0, new SelectListItem { Value = "", Text = "Choisir" });
        }

        public IActionResult OnGet(long id)
        {
            userName = Tools.checkUser(Request.Cookies["user"], _employeeRepository, 1);
            if (userName == null)
                return RedirectToPage("/Login");

            resetTypeDropdownList();
            this.Id = id;
            Dbo.ClientItems item = _stockRepository.Get().Result.FirstOrDefault(it => it.Id == id);
            if (item == null)
            {
                return RedirectToPage("/");
            }
            Name = item.Name;
            Price = item.Price;
            Stock = item.Stock;
            Type = _stockRepository.GetTypes().FirstOrDefault(t => t.Id == item.TypeId).Name;
            PicName = _stockRepository.Get().Result.FirstOrDefault(it => it.Id == id).PicName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            long id = long.Parse(Request.Form["Id"]);

            PicName = Request.Form["Image"];
            if (PicName == "") // Quand l'user n'a pas fourni d'image, on reprend celle d'avant
                PicName = _stockRepository.Get().Result.FirstOrDefault(it => it.Id == id).PicName;
            long TypeId = 0;
            if (Type == null) // Pareil pour le type
                TypeId = _stockRepository.Get().Result.FirstOrDefault(it => it.Id == id).TypeId;
            else
                TypeId = long.Parse(Type);

            Dbo.ClientItems item = new Dbo.ClientItems{
                Id = id,
                Name = Name,
                PicName = PicName,
                Stock = Stock,
                Price = Price,
                TypeId = TypeId
            };
            var result = _stockRepository.Update(item).Result;

            ClientLogs log = new ClientLogs
            {
                Id = 0,
                Date = DateTime.Now,
                Class = this.GetType().Name,
                Type = "Update",
                Logtype = LoggerType.INFO.GetFriendlyName(),
                Message = "Product: " + Name + " has been updated in the Stock base",
            };

            await _logRepository.Insert(log);

            return Redirect("/ProductDetail/" + id);
        }
    }
}