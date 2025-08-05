using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class ColumnMissing
    {
        private int _columnIndex;
        private int _missingElements;
        public ColumnMissing(int index, int amount)
        {
            _columnIndex = index;
            _missingElements = amount;
        }

        public int ColumnIndex { get => _columnIndex; private set => _columnIndex = value; }
        public int MissingElements { get => _missingElements; private set => _missingElements = value; }
    }
}
