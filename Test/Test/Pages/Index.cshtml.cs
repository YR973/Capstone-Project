using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Test.Models;


namespace Test.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ProductContext _context;

        public IndexModel(ILogger<IndexModel> logger, ProductContext context)
        {
            _logger = logger;
            _context = context;
        }

        //list of products
        public List<Product> Products { get; set; }

        //get the first 8 products
        public async Task OnGetAsync()
        {
            Products = await _context.Product.Take(8).ToListAsync();
        }

    }
}