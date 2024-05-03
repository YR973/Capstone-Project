using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Test.Models;


namespace Test.Pages
{

    public class IndexModel : PageModel
    {
        //database context
        private readonly ProductContext _context;

        public IndexModel(ILogger<IndexModel> logger, ProductContext context)
        {
            _context = context;
        }

        //list of products
        public List<Product> Products { get; set; }

        //get the first 8 products
        public async Task OnGetAsync()
        {
            //get the first 8 products
            Products = await _context.Product.Take(8).ToListAsync();
        }

    }
}