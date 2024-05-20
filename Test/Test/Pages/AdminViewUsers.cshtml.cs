using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Test.Models;

namespace Test.Pages
{
    public class AdminViewUsers : PageModel
    {
        private readonly UserContext _context;
        public List<User> Users { get; set; }

        // Constructor
        public AdminViewUsers(UserContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("User")==null)
            {
                return RedirectToPage("/Login");
            }
            User current = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            if (!current.Admin)
            {
                return RedirectToPage("/Index");
            }
            
            // Get all users from the database
            Users = _context.User.ToList();
            return null;
        }
        public async Task<IActionResult> OnPostToggleAdminStatusAsync(int userId)
        {
            var user = await _context.User.FindAsync(userId);
            if (user != null)
            {
                user.Admin = !user.Admin;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/AdminViewUsers");
        }
    }
}