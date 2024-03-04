using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Pages
{
    public class LogIn : PageModel
    {
        private readonly UserContext _context; 

        public LogIn(UserContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
            
        }
       
       

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            // for debugging
            Console.Write("email: " + email);
            Console.Write("password: " + password);
            //query the database for the user
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email.Trim() && u.Password == password.Trim());
            // if the user is found, store the user in the session and redirect to the index page
            if (user != null)
            {
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
                //for debugging
                Console.Write("SUCCESS");
                return RedirectToPage("/Account");
            }
            //for debugging
            Console.Write("FAILURE");
            //if user not found
            return Page();
        }
    }
}