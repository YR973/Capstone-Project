using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Test.Models;


namespace Test.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ProductContext _context;
    
    public IndexModel(ILogger<IndexModel> logger, ProductContext context)
    {
        _logger = logger;
        _context = context;
    }
    public List<Product> Products { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _context.Product.Take(8).ToListAsync();
    }
    
}