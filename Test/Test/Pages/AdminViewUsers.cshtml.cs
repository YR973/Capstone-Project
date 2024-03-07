using Microsoft.AspNetCore.Mvc.RazorPages;
using Test.Models;

namespace Test.Pages
{

    public class AdminViewUsers : PageModel
    {
        private readonly UserContext _context;
        public List<User> Users { get; set; }

        public AdminViewUsers(UserContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Users = _context.User.ToList();
        }
    }
}