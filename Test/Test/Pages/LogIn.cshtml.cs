using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


namespace Test.Pages
{
    public class LogIn : PageModel
    {
        private readonly UserContext _context; 

        public LogIn(UserContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("User") != null)
            {
                return RedirectToPage("/Account");
            }

            return null;
        }
       
       

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            // for debugging
            Console.Write("email: " + email);
            //query the database for the user
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email.Trim() && u.Password == password);
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