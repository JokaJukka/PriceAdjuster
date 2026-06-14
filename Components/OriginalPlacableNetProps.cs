using Unity.Entities;

namespace PriceAdjuster.Components
{
    public struct OriginalPlacableNetProps : IComponentData
    {
        public uint OriginalPrice { get; private set; }
        public float OriginalUpkeep { get; private set; }

        public OriginalPlacableNetProps(uint originalPrice, float originalUpkeep)
        {
            OriginalPrice = originalPrice;
            OriginalUpkeep = originalUpkeep;
        }
    }
}