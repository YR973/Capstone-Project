using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public void OnGet()
        {
            // Get all users from the database
            Users = _context.User.ToList();
        }
    }
}