using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
    }
}

