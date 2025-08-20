using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Data class for containing tracking informations.
    /// It's used to track elements and it's passed to corresponding turnbar.
    /// </summary>
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

        /// <summary>
        /// TODO - return information about move
        /// Increments amount of elmenets. If more than limit, return information about granded player's move. 
        /// </summary>
        /// <param name="amount"></param>
        public void Collect(float amount)
        {
            CurrentElements += amount;
        }

        // TODO - change value depending on eq
        private void GetElementStats() { }
    }
}
