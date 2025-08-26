using Microsoft.Identity.Client;
using ServerKlocki.Database.Models;

namespace ServerKlocki.Database
{
    public class DatabaseSeeder
    {
        public static void Seed(DatabaseContext context)
        {
            if (!context.Users.Any())
            {
                context.AddRange(
                    new User { Email = "mucha@example.org", Password = "mucha", Username = "mucha" }
                );
            }


            context.SaveChanges();
        }
    }
}
