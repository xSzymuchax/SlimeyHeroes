using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Data class, contains information about how many elements of certain type were collected.
    /// </summary>
    public class CollectedElementsInformation
    {
        public ElementType elementType { get; set; }
        public float amount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementType">Type of collected element.</param>
        /// <param name="amount">Amount of collected element.</param>
        public CollectedElementsInformation(ElementType elementType, float amount)
        {
            this.elementType = elementType;
            this.amount = amount;
        }

        /// <summary>
        /// Allows to increment amount of elmeents.
        /// </summary>
        public void Increment()
        {
            amount += 1;
        }
    }
}
