using Unity.Entities;

namespace PriceAdjuster.Components
{
    public struct NetPriceAdjusted : IComponentData
    {
        public uint OriginalPrice { get; private set; }
        public float OriginalUpkeep { get; private set; }

        public NetPriceAdjusted(uint originalPrice, float originalUpkeep)
        {
            OriginalPrice = originalPrice;
            OriginalUpkeep = originalUpkeep;
        }
    }
}