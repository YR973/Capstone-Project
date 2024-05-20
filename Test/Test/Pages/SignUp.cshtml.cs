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
        
        public async Task<IActionResult> OnGetasync()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                return Redirect("/Account");
            }
            return null;
        }

        public async Task<IActionResult> OnPostAsync(string username, string email, string password, string firstname,
            string lastname, string address, string phonenumber)
        {
            if (username== null || email == null || password == null || firstname == null || lastname == null || address == null || phonenumber == null)
            {
                return RedirectToPage("/SignUp");
            }
            // Hash the password
            string hashedPassword = Account.HashPassword(password);
            if (EmailExistsAsync(email).Result)
            {
                Console.Write("E-Mail exists");
                return Page();
            }
            string connectionString = "Server=localhost,3306;User ID=root;Password=admin;Database=main;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sqlStatement = $"INSERT INTO User (Username, Email, Password, FirstName, LastName, Address, PhoneNumber, Admin) VALUES ('{username}', '{email}', '{hashedPassword}', '{firstname}', '{lastname}', '{address}', '{phonenumber}', 0)";
                using (MySqlCommand command = new MySqlCommand(sqlStatement, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            // Redirect to a success page
            return RedirectToPage("/login");
        }
    }
}