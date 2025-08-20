using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    /// <summary>
    /// Class used for tracking the progress of collecting elmenets.
    /// </summary>
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

        /// <summary>
        /// Finds and increments data object, containing information about collected amount of certain type of element.
        /// </summary>
        /// <param name="cei">Collected elements</param>
        public void IncrementCollectedData(CollectedElementsInformation cei)
        {
            foreach (var etd in _collectedElementsDatas)
            {
                if (etd.ElementType == cei.elementType)
                    etd.Collect(cei.amount);
            }
        }

        /// <summary>
        /// Updates tracker, and corresponding turn bars.
        /// </summary>
        /// <param name="cei"></param>
        public void UpdateTracker(CollectedElementsInformation cei)
        {
            if (cei == null)
                return;

            IncrementCollectedData(cei);
            _barsController.UpdateBars();
        }
    }
}
