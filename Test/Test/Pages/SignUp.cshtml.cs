using System.Text;
using System.Text.RegularExpressions;
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
    public class SignUp : PageModel
    {
        private readonly UserContext _context;

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.User.AnyAsync(u => u.Email == email);
        }
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public SignUp(UserContext context)
        {
            _context = context;
        }

        // Get the maximum user id
        int max;
        public async void GetMaxUserIdAsync()
        {
            int maxId = await _context.User.MaxAsync(u => u.UserID);
            max = maxId;
        }
        
        
        public void OnGet()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                Response.Redirect("/Account");
            }
            GetMaxUserIdAsync();
        }

        public async Task<IActionResult> OnPostAsync(string username, string email, string password, string firstname,
            string lastname, string address, string phonenumber)
        {
            // Validate the data
            string emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            string usernamePattern = @"^[\w-]{3,}$";
            string passwordPattern = @"^(?=.*\d).{8,}$";
            if (!Regex.IsMatch(username,usernamePattern) ||
                !Regex.IsMatch(email,emailPattern) ||
                !Regex.IsMatch(password,passwordPattern) ||
                string.IsNullOrWhiteSpace(firstname) ||
                string.IsNullOrWhiteSpace(lastname) ||
                string.IsNullOrWhiteSpace(address) ||
                string.IsNullOrWhiteSpace(phonenumber))
            {
                //for debugging
                Console.Write("FAILURE");
                return Page();
            }
            string hashed = HashPassword(password);
            if (EmailExistsAsync(email).Result)
            {
                Console.Write("E-Mail exists");
                return Page();
            }
            {
                
            }
            string connectionString = "Server=localhost,3306;User ID=root;Password=admin;Database=main;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sqlStatement = $"INSERT INTO User (UserID, Username, Email, Password, FirstName, LastName, Address, PhoneNumber, Admin) VALUES ({max+2}, '{username}', '{email}', '{hashed}', '{firstname}', '{lastname}', '{address}', '{phonenumber}', 0)";
                using (MySqlCommand command = new MySqlCommand(sqlStatement, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            //for debugging
            Console.Write("SUCCESS");
            // Redirect to a success page
            return RedirectToPage("/login");
        }
    }
}