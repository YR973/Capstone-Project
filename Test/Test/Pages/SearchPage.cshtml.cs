﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Test;
using Test.Models;

namespace Test.Pages
{

    public class SearchPage : PageModel
    {
        
        public List<Product> Products;
        private readonly ProductContext _context;
        

        public SearchPage(ProductContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(string query)
        {
            var list = PythonClass.RunScript(query);
            var ids = list.Select(id => int.Parse(id)).ToList();
            Products = await _context.Product.Where(m => ids.Contains(m.ProductID)).ToListAsync<Product>();
            return null;
        }

        
        
    }
}