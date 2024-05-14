using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Test;
using Test.Models;

namespace Test.Pages
{


    public class AdminAddStock : PageModel
    {
        private readonly ProductContext _context;

        public AdminAddStock(ProductContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("User")==null)
            {
                return RedirectToPage("/Login");
            }
            User current = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            if (!current.Admin)
            {
                return RedirectToPage("/Index");
            }
            Product = await _context.Product.ToListAsync();
            return null;
        }

        public async Task<IActionResult> OnPostAsync(int productId, int quantity)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product != null)
            {
                product.Stock += quantity;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/AdminAddStock");
        }
    }
}