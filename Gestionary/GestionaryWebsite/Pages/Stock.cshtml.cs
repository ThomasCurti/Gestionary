using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using GestionaryWebsite.DataAccess.Interfaces;
using GestionaryWebsite.Dbo;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionaryWebsite
{
    public class StockModel : PageModel
    {
        public string userName;

        private IStockRepository _stockRepository { get; }

        public ICollection<ClientStockItems> StockItems { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string TypeFilterInput { get; set; }
        public SelectList TypesFilters { get; set; }

        public StockModel(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public void OnGet()
        {
            userName = Request.Cookies["userName"];

            var types = from t in _stockRepository.GetTypes()
                        orderby t.Name
                        select t.Name;

            var items = from item in _stockRepository.GetStockItems()
                        select item;

            if (!string.IsNullOrEmpty(SearchString))
            {
                items = items.Where(item => item.Name.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(TypeFilterInput))
            {
                items = items.Where(item => item.Type == TypeFilterInput);
            }

            TypesFilters = new SelectList(types.Distinct().ToList());
            StockItems = items.ToList();
        }
    }
}