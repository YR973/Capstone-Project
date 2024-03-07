using Microsoft.AspNetCore.Mvc.RazorPages;
using Test.Models;
using MySql.Data.MySqlClient;


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
        
        // Convert the string of products in the cart to a dictionary
        public Dictionary<int, int> ConvertStringToCartDict(string products)
        {
            // Create a dictionary to hold the products
            Dictionary<int, int> cartDict = new Dictionary<int, int>();
            // Split the string into pairs of product id and quantity
            string[] pairs = products.Split(',');
            // Loop through the pairs and add them to the dictionary
            foreach (string pair in pairs)
            {
                string[] parts = pair.Split('-');
                int productId = int.Parse(parts[0]);
                int quantity = int.Parse(parts[1]);
                cartDict[productId] = quantity;
            }
            //  Return the dictionary
            return cartDict;
        }
        
        public void SetAsDelivered(int order)
        {
            // Create a connection to the database
            string connectionString = "Server=localhost,3306;User ID=root;Password=admin;Database=main;";
            // Create the SQL to update the order status
            string updateOrderStatusSql = $"UPDATE `order` SET Status = 'Delivered' WHERE OrderID = @order";
            // Create a connection to the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Create a command to update the order status
                using (MySqlCommand command = new MySqlCommand(updateOrderStatusSql, connection))
                {
                    // Add the order parameter to the command
                    command.Parameters.AddWithValue("@order", order);
                    // Open the connection and execute the command
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task OnGetAsync(int id)
        {
            // Check if the user is logged in
            if (HttpContext.Session.GetString("User") == null)
            {
                Response.Redirect("/Login");
            }
            // Get the order from the database
            order = _orderContext.Order.Find(id);
            // Check if the order is not found
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
            // Set the order as delivered
            SetAsDelivered(oid);
            Console.WriteLine("Here");
            // Get the order from the database
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
                // Get the product from the database
                var product =  _productContext.Product.Find(item.Key);
                if (product != null)
                {
                    // Add the product to the list
                    Products.Add(product);
                }
            }
            
            
        }
       
    }
}