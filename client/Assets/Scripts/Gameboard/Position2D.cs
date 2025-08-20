using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Class for data organization. It serves as a position of element in gameboard.
    /// </summary>
    public class Position2D
    {
        private int _x;
        private int _y;

        public Position2D(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int Y { get => _y; private set => _y = value; }
        public int X { get => _x; private set => _x = value; }
    }
}
