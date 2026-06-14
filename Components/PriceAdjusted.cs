using Unity.Entities;

namespace PriceAdjuster.Components
{
    public struct PriceAdjusted : IComponentData
    {
        public int OriginalPrice { get; private set; }
    }
}