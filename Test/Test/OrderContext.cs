using Test.Models;
using Microsoft.EntityFrameworkCore;
namespace Test
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        { }
        public DbSet<Order> Order { get; set; }
    }
}