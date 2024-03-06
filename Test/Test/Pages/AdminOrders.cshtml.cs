using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Test.Models;

namespace Test.Pages
{

    public class AdminOrders : PageModel
    {
        private readonly OrderContext _context;
        private readonly UserContext _user;

        public AdminOrders(OrderContext context, UserContext UserContext)
        {
            _context = context;
            _user = UserContext;
        }

        public List<Order> Orders { get; set; }
        public Dictionary<Order, string> OrderDictionary { get; set; }

        public User user { get; set; }
        public User quser { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return RedirectToPage("/Login");
            }
            user = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(user));
            if (user.Admin==false)
            {
                return RedirectToPage("/Login");
            }
            Orders = await _context.Order.ToListAsync();
            OrderDictionary = new Dictionary<Order, string>();
            foreach (var o in Orders)
            {
                quser = await _user.User.FirstOrDefaultAsync(m => m.UserID == o.UserId);
                if (quser != null)
                {
                    OrderDictionary.Add(o, quser.Username);
                }
               
            }
            return null;
        }
    }
}