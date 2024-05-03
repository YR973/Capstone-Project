using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Test;
using Test.Models;

namespace Test.Pages
{

    public class SearchPage : PageModel
    {
        
        public List<Product> products;
        private readonly ProductContext _context;
        

        public SearchPage(ProductContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(string query)
        {
            var list = PythonClass.RunScript(query);
            if (list[0]==-1)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                products = new List<Product>();
                foreach (var id in list)
                {
                    var product = await _context.Product.FirstOrDefaultAsync(m => m.ProductID == id);
                    if (product != null)
                    {
                        products.Add(product);
                    }
                }

                return null;
            }
            

        }

        
        
    }
}