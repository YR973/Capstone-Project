using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.Models;

namespace Test
{

    public class SuggestionFetch
    {
        private readonly ProductContext _context;
        private List<Product>  Products;
        public SuggestionFetch(ProductContext context)
        {
            _context = context;
        }

        // CLASS NOT USED

    }
}