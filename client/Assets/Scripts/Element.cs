using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public enum ElementType
    {
        RED=1, ORANGE=2, GREEN=3, YELLOW=4
    }

    public class Element : MonoBehaviour
    {
        private int _x;
        private int _y;
        public ElementType elementType;

        public void SetPosition(Position2D position2D)
        {
            _x = position2D.X;
            _y = position2D.Y;
        }

        private void OnMouseDown()
        {
            Position2D position2D = new Position2D(_x, _y);
            GameController.Instance.ElementPressed(position2D);
        }
    }
}
