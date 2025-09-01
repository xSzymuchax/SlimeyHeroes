using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Mapping
{
    public class CharacterIDToCharacterSprite : MonoBehaviour
    {
        private static Dictionary<int, string> _map = new Dictionary<int, string>()
        {
            { 1, "debugCharacterImage" },
            { 2, "debugCharacterImage" },
            { 3, "debugCharacterImage" },
            { 4, "debugCharacterImage" }
        };

        public static Sprite GetImageOfId(int id)
        {
            if (_map.TryGetValue(id, out string spriteName))
            {
                Sprite sprite = Resources.Load<Sprite>($"Sprite/Characters/{spriteName}");

                if (sprite == null)
                {
                    Debug.LogError($"Found sprite name but not found sprite resource: id: {id}, name: {spriteName}");
                }
                return sprite;
            }
            else
            {
                Debug.LogError($"No mapping for sprite with ID: {id}");
                return null;
            }
        }
    }
}
