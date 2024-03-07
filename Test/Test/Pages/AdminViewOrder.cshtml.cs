using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Test.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Test;
using MySql.Data.MySqlClient;
using Test.Models;

namespace Test.Pages
{

    public class AdminViewOrder : PageModel
    {
        private readonly OrderContext _orderContext;
        private readonly ProductContext _productContext;
        private readonly UserContext _userContext;
        private readonly ILogger<IndexModel> _logger;
        public List<Product> Products { get; set; }

        public Dictionary<int, int> CartDict { get; set; }
        
        public Order order { get; set; }

        public AdminViewOrder(ILogger<IndexModel> logger, ProductContext context, UserContext userContext,
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
        
        public void SetAsDelivered(int order)
        {
            string connectionString = "Server=localhost,3306;User ID=root;Password=admin;Database=main;";
            string updateOrderStatusSql = $"UPDATE `order` SET Status = 'Delivered' WHERE OrderID = @order";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(updateOrderStatusSql, connection))
                {
                    command.Parameters.AddWithValue("@order", order);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task OnGetAsync(int id)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                Response.Redirect("/Login");
            }
            {
                
            }
            order = _orderContext.Order.Find(id);
            if (order == null)
            {
                // Handle the case where the order is not found
                return;
            }

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
        
        public void OnPost(int oid)
        {
            
            Console.WriteLine(oid);
            SetAsDelivered(oid);
            Console.WriteLine("Here");
            order =  _orderContext.Order.Find(oid);
            if (order == null)
            {
                Console.WriteLine("Here is null");
                // Handle the case where the order is not found
                
            }

            //initialize the list of products
            Products = new List<Product>();
            CartDict = ConvertStringToCartDict(order.Products);
            //loop through the cart items and add the products to the list
            foreach (var item in CartDict)
            {
                var product =  _productContext.Product.Find(item.Key);
                if (product != null)
                {
                    Products.Add(product);
                }
            }
            
            
        }
       
    }
}