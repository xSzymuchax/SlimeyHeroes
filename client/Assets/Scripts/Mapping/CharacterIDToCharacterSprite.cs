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
        private static Dictionary<int, string> _map;

        private void Start()
        {
            _map = new Dictionary<int, string>()
        {
            { 1, "debugCharacterImage" },
            { 2, "debugCharacterImage" },
            { 3, "debugCharacterImage" },
            { 4, "debugCharacterImage" }
        };
        }



        public static Sprite GetImageOfId(int id)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprite/Characters/{_map[id]}");
            return sprite;
        }
    }
}
