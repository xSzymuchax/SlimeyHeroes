using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class ElementsTrackerData
    {
        public ElementType ElementType { get; set; }
        public float MaxElements { get; set; }
        public float CurrentElements { get; set; }
        public float AllCollected { get; set; }

        public ElementsTrackerData(ElementType elementType)
        {
            ElementType = elementType;
            MaxElements = 30f; // TODO - Change value depending on eq
            CurrentElements = 0f;
            AllCollected = 0f;
        }

        public void Collect(float amount)
        {
            CurrentElements += amount;
        }

        // TODO - change value depending on eq
        private void GetElementStats() { }
    }
}
