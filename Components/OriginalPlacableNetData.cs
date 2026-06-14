using Unity.Entities;

namespace PriceAdjuster.Components
{
    public struct OriginalPlacableNetData : IComponentData
    {
        public uint OriginalPrice { get; private set; }
        public float OriginalUpkeep { get; private set; }

        public OriginalPlacableNetData(uint originalPrice, float originalUpkeep)
        {
            OriginalPrice = originalPrice;
            OriginalUpkeep = originalUpkeep;
        }
    }
}