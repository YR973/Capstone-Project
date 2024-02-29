namespace Test.Pages
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using Test.Models;
    using Newtonsoft.Json;

    public class ViewProductModel : PageModel
    {
        private readonly ProductContext _context;

        public ViewProductModel(ProductContext context)
        {
            _context = context;
        }
        
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Product.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPostAddToCartAsync(int id, int quantity)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetString("cart");
            var cartDict = string.IsNullOrEmpty(cart) ? new Dictionary<int, int>() : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);

            if (cartDict.ContainsKey(id))
            {
                cartDict[id] += quantity;
            }
            else
            {
                cartDict[id] = quantity;
            }

            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartDict));

            return RedirectToPage("Cart");
        }
    }
}