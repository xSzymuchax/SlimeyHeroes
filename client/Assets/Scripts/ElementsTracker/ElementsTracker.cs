using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class ElementsTracker
    {
        private List<ElementsTrackerData> _collectedElementsDatas;
        private BarsController _barsController;
        public ElementsTracker(List<ElementType> elementTypes)
        {
            _collectedElementsDatas = new();
            _barsController = BarsController.Instance;
            foreach (ElementType et in elementTypes)
            {
                _collectedElementsDatas.Add(new ElementsTrackerData(et));
            }

            _barsController.InitializeBars(_collectedElementsDatas);
        }

        public void IncrementCollectedData(CollectedElementsInformation cei)
        {
            foreach (var etd in _collectedElementsDatas)
            {
                if (etd.ElementType == cei.elementType)
                    etd.Collect(cei.amount);
            }
        }

        public void UpdateTracker(CollectedElementsInformation cei)
        {
            if (cei == null)
                return;

            IncrementCollectedData(cei);
            _barsController.UpdateBars();
        }
    }
}
