using Unity.Entities;

namespace PriceAdjuster.Components
{
    public struct PriceAdjusted : IComponentData
    {
        public uint OriginalPrice { get; private set; }
        public float OriginalUpkeep { get; private set; }

        public PriceAdjusted(uint originalPrice, float originalUpkeep)
        {
            OriginalPrice = originalPrice;
            OriginalUpkeep = originalUpkeep;
        }
    }
}