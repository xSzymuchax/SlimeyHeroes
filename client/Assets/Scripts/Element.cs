using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class Element : MonoBehaviour
    {
        private int _x;
        private int _y;

        public void SetPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }

        private void OnMouseDown()
        {
            GameController.Instance.ElementPressed(_x, _y);
        }
    }
}
