using Microsoft.AspNetCore.Mvc.RazorPages;
using Test.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Test.Pages
{

    public class Orders : PageModel
    {
        private readonly OrderContext _orderContext;

        public Orders(OrderContext orderContext)
        {
            _orderContext = orderContext;
        }

        public List<Order> Order { get; set; }
        public User CurrentUser { get; set; }

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("User") == null)
            {
                Response.Redirect("/LogIn");
            }
            else
            {
                CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("User"));
                Order = await _orderContext.Order.Where(o => o.UserId == CurrentUser.UserID).ToListAsync();
            }
        }
    }
}