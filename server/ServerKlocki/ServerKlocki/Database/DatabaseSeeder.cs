using Microsoft.EntityFrameworkCore;
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
                    new User { Email = "mucha@example.org", Password = "mucha", Username = "mucha", Trophies = 69 }
                );
            }

            if (!context.Rarities.Any())
            {
                context.AddRange(
                    new Rarity { Name = "COMMON" },
                    new Rarity { Name = "RARE" },
                    new Rarity { Name = "EPIC" },
                    new Rarity { Name = "LEGENDARY" }
                );
            }

            if (!context.ElementTypes.Any())
            {
                context.AddRange(
                    new ElementType { Type = "FIRE"},
                    new ElementType { Type = "WATER"},
                    new ElementType { Type = "NATURE"},
                    new ElementType { Type = "LIGHTNING"}
                );
            }

            context.SaveChanges();

            if (!context.Characters.Any())
            {
                context.AddRange(
                    new Character { Name = "FireDebugCharacter", ElementTypeId = 1, RarityId = 1, Description = "Character created for debugging."},
                    new Character { Name = "WaterDebugCharacter", ElementTypeId = 2, RarityId = 1, Description = "Character created for debugging."},
                    new Character { Name = "NatureDebugCharacter", ElementTypeId = 3, RarityId = 1, Description = "Character created for debugging."},
                    new Character { Name = "LightningDebugCharacter", ElementTypeId = 4, RarityId = 1, Description = "Character created for debugging."}
                );
            }

            if (!context.UserOwnCharacters.Any())
            {
                context.AddRange(
                    new UserOwnCharacters { CharacterId = 1, CharacterLevel = 1, SoulsAmount = 0, UserId = 1},
                    new UserOwnCharacters { CharacterId = 2, CharacterLevel = 1, SoulsAmount = 0, UserId = 1},
                    new UserOwnCharacters { CharacterId = 3, CharacterLevel = 1, SoulsAmount = 0, UserId = 1},
                    new UserOwnCharacters { CharacterId = 4, CharacterLevel = 1, SoulsAmount = 0, UserId = 1}
                );
            }

            context.SaveChanges();

        }
    }
}
