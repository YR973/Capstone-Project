using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Test.Pages;

public class Search : PageModel
{
    public void OnGet(string query)
    {
        ViewData["Query"] = query;
    }
}