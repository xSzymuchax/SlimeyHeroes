using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class CollectedElementsInformation
    {
        public ElementType elementType { get; set; }
        public float amount { get; set; }

        public CollectedElementsInformation(ElementType elementType, float amount)
        {
            this.elementType = elementType;
            this.amount = amount;
        }

        public void Increment()
        {
            amount += 1;
        }
    }
}
