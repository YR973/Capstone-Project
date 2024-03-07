﻿using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Test.Models;
using MySql.Data.MySqlClient;

namespace Test.Pages
{

    public class Account : PageModel
    {
        public User CurrentUser { get; set; }
        public void OnGet()
        {
            // If the user is not logged in, redirect to the login page
            if (HttpContext.Session.GetString("User") == null)
            {
                Response.Redirect("/LogIn");
            }
            else
            {
                // Get the user data from the session
                var userData = HttpContext.Session.GetString("User");
                CurrentUser = JsonConvert.DeserializeObject<User>(userData);
            }
        }
        
        // Hash the password
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
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
            // If the user is not logged in, redirect to the login page
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToPage("/LogIn");
            }
            // Get the user data from the session
            var userData = HttpContext.Session.GetString("User");
            var currentUser = JsonConvert.DeserializeObject<User>(userData);
            string hashed = HashPassword(password);
            
            string connectionString = "Server=localhost,3306;User ID=root;Password=admin;Database=main;";
            // Update the user data in the database
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string sqlStatement;
                // If the password is not being updated, don't include it in the SQL statement
                if (password==null)                   
                {
                     sqlStatement = $"UPDATE User SET FirstName = '{firstName}', LastName = '{lastName}', Username = '{username}', Email = '{email}', Address = '{address}', PhoneNumber = '{phone}' WHERE UserID = {currentUser.UserID}";

                }
                else
                {
                     sqlStatement = $"UPDATE User SET FirstName = '{firstName}', LastName = '{lastName}', Username = '{username}', Email = '{email}', Address = '{address}', PhoneNumber = '{phone}', Password = '{hashed}' WHERE UserID = {currentUser.UserID}";
                }

                using (MySqlCommand command = new MySqlCommand(sqlStatement, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Update the user data in the session
            currentUser.FirstName = firstName;
            currentUser.LastName = lastName;
            currentUser.Username = username;
            currentUser.Email = email;
            currentUser.Address = address;
            currentUser.PhoneNumber = phone;
            currentUser.Password = password;

            // Update the user data in the session
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(currentUser));

            return RedirectToPage();
        }
    }
}