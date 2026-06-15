using Unity.Entities;

namespace PriceAdjuster.Components
{
    public struct OriginalPlaceableNetProps : IComponentData
    {
        public uint OriginalPrice { get; private set; }
        public float OriginalUpkeep { get; private set; }

        public OriginalPlaceableNetProps(uint originalPrice, float originalUpkeep)
        {
            OriginalPrice = originalPrice;
            OriginalUpkeep = originalUpkeep;
        }
    }
}