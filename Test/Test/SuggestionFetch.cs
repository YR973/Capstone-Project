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

        public List<Product> Suggest(string query)
        {
            var suggestions = PythonClass.Suggest(query);
            int[] ids = new int[2];
            for (int i = 0; i < 2; i++)
            {
                ids[i] = int.Parse(suggestions[i]);
            }
            Products = _context.Product.Where(m => ids.Contains(m.ProductID)).ToList();

            return Products;
        }

    }
}