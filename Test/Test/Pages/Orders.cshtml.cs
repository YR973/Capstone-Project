using Microsoft.AspNetCore.Mvc.RazorPages;
using Test.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Test.Pages
{

    public class Orders : PageModel
    {
        private readonly OrderContext _orderContext;
        public List<Order> Order { get; set; }
        public User CurrentUser { get; set; }
        
        public Orders(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                return Redirect("/LogIn");
            }
            CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(CurrentUser));
            Order = await _orderContext.Order.Where(o => o.UserId == CurrentUser.UserID).ToListAsync();
            if (Order==null)
            {
                return Redirect("/Error");
            }
            return null;
        }
    }
}