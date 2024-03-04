using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test
{

    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}