using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Test.Pages
{
    public class SignUp : PageModel
    {
        private readonly UserContext _context;

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.User.AnyAsync(u => u.Email == email);
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
        
        
        public async Task<IActionResult> OnGetasync()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                return Redirect("/Account");
            }
            GetMaxUserIdAsync();
            return null;
        }

        public async Task<IActionResult> OnPostAsync(string username, string email, string password, string firstname,
            string lastname, string address, string phonenumber)
        {
            // Validate the data
            string emailPattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
            string usernamePattern = @"^[\w-]{3,}$";
            string passwordPattern = @"^(?=.*\d).{8,}$";
            if (!Regex.IsMatch(username,usernamePattern) == false ||
                !Regex.IsMatch(email,emailPattern) == false ||
                !Regex.IsMatch(password,passwordPattern) ==false ||
                string.IsNullOrWhiteSpace(firstname) ||
                string.IsNullOrWhiteSpace(lastname) ||
                string.IsNullOrWhiteSpace(address) ||
                string.IsNullOrWhiteSpace(phonenumber))
            {
                //for debugging
                Console.Write("FAILURE");
                return Page();
            }
            
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
                string sqlStatement = $"INSERT INTO User (UserID, Username, Email, Password, FirstName, LastName, Address, PhoneNumber, Admin) VALUES ({max+2}, '{username}', '{email}', '{password}', '{firstname}', '{lastname}', '{address}', '{phonenumber}', 0)";
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