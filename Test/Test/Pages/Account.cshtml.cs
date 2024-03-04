using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Test.Models;

namespace Test.Pages
{

    public class Account : PageModel
    {
        public User CurrentUser { get; set; }
        public void OnGet()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                Response.Redirect("/LogIn");
            }
            else
            {
                var userData = HttpContext.Session.GetString("User");
                CurrentUser = JsonConvert.DeserializeObject<User>(userData);
            }
        }
        
        public async Task<IActionResult> OnPostUpdateAsync(
            string firstName,
            string lastName, 
            string username, 
            string email,
            string address, 
            string phone, 
            string password)
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToPage("/LogIn");
            }

            var userData = HttpContext.Session.GetString("User");
            var currentUser = JsonConvert.DeserializeObject<User>(userData);

            string connectionString = "Server=localhost,3306;User ID=root;Password=admin;Database=main;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sqlStatement;
                if (password==null)                   
                {
                     sqlStatement = $"UPDATE User SET FirstName = '{firstName}', LastName = '{lastName}', Username = '{username}', Email = '{email}', Address = '{address}', PhoneNumber = '{phone}' WHERE UserID = {currentUser.UserID}";

                }
                else
                {
                     sqlStatement = $"UPDATE User SET FirstName = '{firstName}', LastName = '{lastName}', Username = '{username}', Email = '{email}', Address = '{address}', PhoneNumber = '{phone}', Password = '{password}' WHERE UserID = {currentUser.UserID}";
                }

                using (MySqlCommand command = new MySqlCommand(sqlStatement, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            currentUser.FirstName = firstName;
            currentUser.LastName = lastName;
            currentUser.Username = username;
            currentUser.Email = email;
            currentUser.Address = address;
            currentUser.PhoneNumber = phone;
            currentUser.Password = password;

            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(currentUser));

            return RedirectToPage();
        }
    }
}