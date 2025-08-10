using Microsoft.EntityFrameworkCore;
using ServerKlocki.Database.Models;

namespace ServerKlocki.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // add models to database
        public DbSet<User> Users { get; set; }
    }
}
