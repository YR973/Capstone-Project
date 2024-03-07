using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Test.Models;
using Newtonsoft.Json;
namespace Test.Pages
{
    public class ViewProductModel : PageModel
    {
        private readonly ProductContext _context;

        public ViewProductModel(ProductContext context)
        {
            _context = context;
        }
        
        
        //product property
        public Product Product { get; set; }
        
        
        //get product by id from database
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Product.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
        
        //add to cart method
        public async Task<IActionResult> OnPostAddToCartAsync(int id, int quantity)
        {
            //get product by id
            var product = await _context.Product.FindAsync(id);

            //check if product exists in database
            if (product == null)
            {
                return NotFound();
            }
            
            //get cart from session
            var cart = HttpContext.Session.GetString("cart");
            //deserialize cart from session
            var cartDict = string.IsNullOrEmpty(cart) ? new Dictionary<int, int>() : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);

            //add product to cart
            if (cartDict.ContainsKey(id))
            {
                //if product already exists in cart, add quantity to existing quantity
                cartDict[id] += quantity;
            }
            else
            {
                //else add product to cart
                cartDict[id] = quantity;
            }
            
            //serialize cart and save to session
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartDict));

            //redirect to cart page
            return RedirectToPage("Cart");
        }
    }
}