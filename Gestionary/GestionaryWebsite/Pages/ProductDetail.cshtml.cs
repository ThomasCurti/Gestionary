using GestionaryWebsite.DataAccess.Interfaces;
using GestionaryWebsite.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace GestionaryWebsite
{
    public class ProductDetailModel : PageModel
    {
        public string userName;

        public long Id { get; set; }

        public string Name { get; set; }
        public string Type { get; set; }
        public string PicName { get; set; }
        public decimal Price { get; set; }
        public long Stock { get; set; }

        public IStockRepository StockRepository { get; }

        public ProductDetailModel(IStockRepository stockRepository)
        {
            StockRepository = stockRepository;
        }

        public IActionResult OnGet(long id)
        {
            userName = Request.Cookies["userName"];
                

            Id = id;
            Dbo.ClientItems item = StockRepository.Get().Result.FirstOrDefault(it => it.Id == id);
            if (item == null)
            {
                return RedirectToPage("/");
            }
            Name = item.Name;
            Price = item.Price;
            Stock = item.Stock;
            PicName = item.PicName;
            Type = StockRepository.GetTypes().FirstOrDefault(t => t.Id == item.TypeId).Name;
            return Page();
        }

        public IActionResult OnPostDelete()
        {
            long id = long.Parse(Request.Form["Id"]);
            bool res = StockRepository.Delete(id).Result;
            return RedirectToPage("/Stock");
        }

        public IActionResult OnPostEdit()
        {
            long id = long.Parse(Request.Form["Id"]);
            return Redirect("/StockHandlerDetail/" + id);
        }
    }
}