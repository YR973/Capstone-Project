using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Test.Models;
using Microsoft.EntityFrameworkCore;

namespace Test.Pages
{

    public class Checkout : PageModel
    {

        private readonly ILogger<IndexModel> _logger;
        private readonly ProductContext _context;
        private readonly UserContext _userContext;
        private readonly OrderContext _orderContext;

        public Checkout(ILogger<IndexModel> logger, ProductContext context, UserContext userContext, OrderContext orderContext)
        {
            _logger = logger;
            _context = context;
            _userContext = userContext;
            _orderContext = orderContext;
        }
        

        public List<Product> Products { get; set; }
        int max;
        public async void GetMaxUserIdAsync()
        {
            int maxId = await _orderContext.Order.MaxAsync<Order, int>(u => u.OrderId);
            max = maxId;
        }

        //dictionary to store the cart items
        public Dictionary<int, int> CartDict { get; set; }
        public User CurrentUser { get; set; }
        string products = "";
        public int TotalItems { get; set; }
        public float TotalCost { get; set; }

        public async Task OnGetAsync()
        {
            //get the cart from the session
            var cart = HttpContext.Session.GetString("cart");
            var user = HttpContext.Session.GetString("User");
            if (cart == null || user == null)
            {
                RedirectToPage("/Cart");
            }
            GetMaxUserIdAsync();
            //deserialize the cart from the session
            CurrentUser = JsonConvert.DeserializeObject<User>(user);
            CartDict = string.IsNullOrEmpty(cart)
                ? new Dictionary<int, int>()
                : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
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

            int totalItems = 0;
            float totalCost = 0;
            //loop through the cart dictionary and calculate the total items and cost
            foreach (var item in CartDict)
            {
                var product = Products.FirstOrDefault(p => p.ProductID == item.Key);
                if (product != null)
                {
                    totalItems += item.Value;
                    totalCost += item.Value * product.Price;
                }
            }

            TotalItems = totalItems;
            TotalCost = totalCost;
            
            foreach (var item in CartDict)
            {
                products += $"{item.Key}-{item.Value},";
            }

            products = products.TrimEnd(',');
        }

        public async Task OnPostAsync()
        {
            string connectionString = "Server=localhost,3306;User ID=root;Password=admin;Database=main;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string sqlStatement = $"INSERT INTO Order (OrderID, UserID, Products, TotalPrice) VALUES ({max+2}, '{CurrentUser.UserID}', '{products}', '{TotalCost}')";
                using (MySqlCommand command = new MySqlCommand(sqlStatement, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}