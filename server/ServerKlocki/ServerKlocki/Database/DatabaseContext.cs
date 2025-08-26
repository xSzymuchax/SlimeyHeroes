using Microsoft.EntityFrameworkCore;
using ServerKlocki.Database.Models;

namespace ServerKlocki.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // add models to database
        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Rarity> Rarities { get; set; }
        public DbSet<UserOwnCharacters> UserOwnCharacters { get; set; }
        public DbSet<ElementType> ElementTypes { get; set; }

    }
}
