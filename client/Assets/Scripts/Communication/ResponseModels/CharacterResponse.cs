using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Communication.ResponseModels
{
    /// <summary>
    /// Data model for retrieving characters form server's API.
    /// </summary>
    [Serializable]
    public class CharacterResponse
    {
        /// <summary>
        /// Id of a character, used for mapping.
        /// </summary>
        public int characterId;

        /// <summary>
        /// Level of a character.
        /// </summary>
        public int level;

        /// <summary>
        /// Souls amount of character.
        /// </summary>
        public int soulsAmount;
    }
}
