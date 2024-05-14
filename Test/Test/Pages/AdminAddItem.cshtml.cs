using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Test.Models;

namespace Test.Pages;

public class AdminAddItem : PageModel
{
    private readonly ProductContext _context;

    public AdminAddItem(ProductContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGet()
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

        return null;
    }
    public async Task<IActionResult> OnPostAsync(string name, string description, float price, int stock, IFormFile image)
    {
        // Create a new Product object
        //find the highest product id in the database
        var maxId = _context.Product.Max(p => p.ProductID);
        //set the new product id to the highest product id + 1
        int id = maxId + 1;
        var product = new Product
        {
            ProductID = id,
            Name = name,
            Description = description,
            Price = price,
            Stock = stock
        };
        
        // Add the Product object to the database
        _context.Product.Add(product);
        await _context.SaveChangesAsync();
        
        // Get the ID of the new product and use it to create a unique name for the image file
        var uniqueFileName = product.ProductID + ".png";
        // Create a path to the directory where you want to save the image
        var imagesDirectory = Path.Combine("C:\\Users\\yfroo\\Documents\\GitHub\\hello\\Capstone-Project\\Test\\Test\\wwwroot\\img\\img\\product", uniqueFileName);

        // Save the image to the specified path
        using (var fileStream = new FileStream(imagesDirectory, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }
        PythonClass.AddItem(product.Name, product.Description, product.ProductID);
        return RedirectToPage("/AdminAddItem");
        
    }
}