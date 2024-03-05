using Microsoft.AspNetCore.Mvc.RazorPages;
using Test.Models;

namespace Test.Pages
{

    public class ViewOrderDetails : PageModel
    {
        private readonly OrderContext _orderContext;
        private readonly ProductContext _productContext;
        private readonly UserContext _userContext;
        private readonly ILogger<IndexModel> _logger;
        public List<Product> Products { get; set; }

        public Dictionary<int, int> CartDict { get; set; }
        
        public Order order { get; set; }

        public ViewOrderDetails(ILogger<IndexModel> logger, ProductContext context, UserContext userContext,
            OrderContext orderContext)
        {
            _logger = logger;
            _productContext = context;
            _userContext = userContext;
            _orderContext = orderContext;
        }
        
        public Dictionary<int, int> ConvertStringToCartDict(string products)
        {
            Dictionary<int, int> cartDict = new Dictionary<int, int>();
            string[] pairs = products.Split(',');

            foreach (string pair in pairs)
            {
                string[] parts = pair.Split('-');
                int productId = int.Parse(parts[0]);
                int quantity = int.Parse(parts[1]);
                cartDict[productId] = quantity;
            }

            return cartDict;
        }

        public async Task OnGetAsync(int id)
        {
            order = _orderContext.Order.Find(id);
            //initialize the list of products
            Products = new List<Product>();
            CartDict = ConvertStringToCartDict(order.Products);
            //loop through the cart items and add the products to the list
            foreach (var item in CartDict)
            {
                var product = await _productContext.Product.FindAsync(item.Key);
                if (product != null)
                {
                    Products.Add(product);
                }
            }
        }
    }
}