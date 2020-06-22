using Microsoft.EntityFrameworkCore;

namespace CsharpBelt.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        
        public DbSet<User> Users {get; set;}
        public DbSet<Plan> Plans {get; set;}
        public DbSet<Invite> Invites {get; set;}
    }
}