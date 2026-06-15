using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.Logic
{
    public partial class TrackPricingSystem : AbstractNetPricingSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            InitialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetComposition>(),
                    ComponentType.ReadOnly<TrackComposition>()
                },
                None = new[] { ComponentType.ReadOnly<OriginalPlaceableNetProps>() }
            });

            RecalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetComposition>(),
                    ComponentType.ReadOnly<TrackComposition>(),
                    ComponentType.ReadWrite<ScheduledPriceRecalculation>()
                },
            });

            RequireAnyForUpdate(InitialQuery, RecalcQuery);
        }

        protected override float PriceCoefficient() => Mod.Settings.TrackPriceMultiplierSlider;

        protected override float UpkeepCoefficient() => Mod.Settings.TrackUpkeepMultiplierSlider;
    }
}