using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Test.Models;
using Microsoft.AspNetCore.Mvc;

namespace Test.Pages
{
    public class Cart : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;
        private readonly ProductContext _context;
        //list to store the products in the cart
        public List<Product> Products { get; set; }
        //dictionary to store the cart items
        public Dictionary<int, int> CartDict { get; set; }
        
        public Cart(ProductContext context)
        {
            //_logger = logger;
            _context = context;
        }
        
        public async Task OnGetAsync()
        {
            //get the cart from the session
            var cart = HttpContext.Session.GetString("cart");
            //deserialize the cart from the session
            CartDict = string.IsNullOrEmpty(cart) ? new Dictionary<int, int>() : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
            //initialize the list of products
            Products = new List<Product>();
            //loop through the cart items and add the products to the list
            foreach (var item in CartDict)
            {
                var product = await _context.Product.FindAsync(item.Key);
                if (product != null)
                {
                    Products.Add(product);
                }
            }
        }
        public async Task<IActionResult> OnPostRemoveFromCartAsync(int id)
        {
            //get the cart from the session
            var cart = HttpContext.Session.GetString("cart");
            //deserialize the cart from the session
            var cartDict = string.IsNullOrEmpty(cart) ? new Dictionary<int, int>() : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
            //remove the product from the cart
            if (cartDict.ContainsKey(id))
            {
                cartDict.Remove(id);
                //serialize the cart and store it in the session
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cartDict));
            }
            //redirect to the cart page
            return RedirectToPage();
        }
    }
}