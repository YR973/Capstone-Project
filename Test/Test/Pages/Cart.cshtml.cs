using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using Test.Models;
using Microsoft.AspNetCore.Mvc;

namespace Test.Pages
{
    public class Cart : PageModel
    {
        
        private readonly ILogger<IndexModel> _logger;
        private readonly ProductContext _context;
    
        public Cart(ILogger<IndexModel> logger, ProductContext context)
        {
            _logger = logger;
            _context = context;
        }
        public List<Product> Products { get; set; }
        public Dictionary<int, int> CartDict { get; set; }

        public async Task OnGetAsync()
        {
            var cart = HttpContext.Session.GetString("cart");
            CartDict = string.IsNullOrEmpty(cart) ? new Dictionary<int, int>() : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);

            Products = new List<Product>();

            foreach (var item in CartDict)
            {
                var product = await _context.Product.FindAsync(item.Key);
                if (product != null)
                {
                    Products.Add(product);
                }
            }
        }
        public async Task<IActionResult> OnPostRemoveFromCartAsync(int id)
        {
            var cart = HttpContext.Session.GetString("cart");
            var cartDict = string.IsNullOrEmpty(cart) ? new Dictionary<int, int>() : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);

            if (cartDict.ContainsKey(id))
            {
                cartDict.Remove(id);
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartDict));
            }

            return RedirectToPage();
        }
    }
}