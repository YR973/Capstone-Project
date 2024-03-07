using System.Data;
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

        public Checkout(ILogger<IndexModel> logger, ProductContext context, UserContext userContext,
            OrderContext orderContext)
        {
            _logger = logger;
            _context = context;
            _userContext = userContext;
            _orderContext = orderContext;
        }


        //list of products
        public List<Product> Products { get; set; }
        int max = 0;

        //get the max user id from the database
        public async Task GetMaxUserIdAsync()
        {
            if (await _orderContext.Order.AnyAsync())
            {
                int maxId = await _orderContext.Order.MaxAsync<Order, int>(u => u.OrderId);
                max = maxId;
            }
            else
            {
                max = 0;
            }
        }

        //dictionary to store the cart items
        public Dictionary<int, int> CartDict { get; set; }
        //user object to store the current user
        public User CurrentUser { get; set; }
        //string to store the products
        string products = "";
        //total items and cost
        public int TotalItems { get; set; }
        //total items and cost
        public float TotalCost { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //get the cart from the session
            var cart = HttpContext.Session.GetString("cart");
            //get the user from the session
            var user = HttpContext.Session.GetString("User");
            if (cart == null || user == null)
            {

                Console.WriteLine("No user or cart");
                return RedirectToPage("/Cart");
            }

            //deserialize the user from the session
            CurrentUser = JsonConvert.DeserializeObject<User>(user);
            //store the user in the session
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(CurrentUser));
            //deserialize the cart from the session
            CartDict = string.IsNullOrEmpty(cart)
                ? new Dictionary<int, int>()
                : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
            //store the cart in the session
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(CartDict));

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

            //initialize the products string
            foreach (var item in CartDict)
            {
                products += $"{item.Key}-{item.Value},";
            }

            //remove the last comma
            products = products.TrimEnd(',');
            return null;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var cart = HttpContext.Session.GetString("cart");
            CartDict = string.IsNullOrEmpty(cart)
                ? new Dictionary<int, int>()
                : JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(CartDict));
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

            //initialize the products string
            foreach (var item in CartDict)
            {
                products += $"{item.Key}-{item.Value},";
            }
            //remove the last comma
            products = products.TrimEnd(',');
            //connection string to connect to the database
            string connectionString = "Server=localhost,3306;User ID=root;Password=admin;Database=main;";
            //get the max user id
            await GetMaxUserIdAsync();
            var user = HttpContext.Session.GetString("User");
            if (user != null)
            {
                //deserialize the user from the session
                CurrentUser = JsonConvert.DeserializeObject<User>(user);
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(CurrentUser));

            }

            //check if the user is logged in and the products list is not empty
            if (CurrentUser != null && products != null)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        //insert the order into the database using SQL query
                        string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string sqlStatement =
                            $"INSERT INTO `Order` (OrderID, UserID, Products, TotalPrice, Date, Status) VALUES ({max + 1}, '{CurrentUser.UserID}', '{products}', '{TotalCost}', '{currentDate}', 'Pending')";
                        using (MySqlCommand command = new MySqlCommand(sqlStatement, connection))
                        {
                            command.ExecuteNonQuery();
                        }

                        //loop through the cart dictionary and calculate the total items and cost
                        foreach (var item in CartDict)
                        {
                            var product = Products.FirstOrDefault(p => p.ProductID == item.Key);
                            if (product != null)
                            {
                                totalItems += item.Value;
                                totalCost += item.Value * product.Price;

                                // decrease the stock of the product using SQL query
                                string sqlStatement2 =
                                    $"UPDATE Product SET Stock = Stock - {item.Value} WHERE ProductID = {item.Key}";
                                // execute the SQL command
                                using (MySqlCommand command = new MySqlCommand(sqlStatement2, connection))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    //clear the cart
                    HttpContext.Session.Remove("cart");
                    return RedirectToPage("/Orders");
                }

                return null;
            }
            return null;
        }
    }
}