using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerKlocki.Database;
using Microsoft.EntityFrameworkCore;
using ServerKlocki.Mapping;

namespace ServerKlocki.Controllers
{
    [ApiController]
    [Route("/characters")]
    public class CharactersController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;

        public CharactersController(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetPlayersCharacters()
        {
            var u = User.FindFirst("id")?.Value;
            if (!int.TryParse(u, out int userId))
            {
                return Unauthorized();
            }    

            var result = await _dbContext.UserOwnCharacters
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return Ok(result);
        }

        [Authorize]
        [HttpPost("{characterId}")]
        public async Task<IActionResult> UpgradeCharacter(int characterId)
        {
            var u = User.FindFirst("id")?.Value;
            if (!int.TryParse(u, out int userId))
            {
                return Unauthorized();
            }

            var record = await _dbContext.UserOwnCharacters
                .FirstOrDefaultAsync(x => x.UserId == userId && x.CharacterId == characterId);

            if (record == null)
                return NotFound();

            int level = record.CharacterLevel;
            int souls = record.SoulsAmount;
            int neededSouls = UpgardeCharacterAmount.map[level];
            
            if (souls >= neededSouls && level <= 5)
            {
                record.CharacterLevel += 1;
                record.SoulsAmount -= neededSouls;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest("Can't upgrade this character.");
            }

            return Ok(record);
        }
    }
}
