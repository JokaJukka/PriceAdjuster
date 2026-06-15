using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.Logic
{
    public partial class RoadPricingSystem : AbstractNetPricingSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            InitialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetComposition>(),
                    ComponentType.ReadOnly<RoadComposition>()
                },
                None = new[] { ComponentType.ReadOnly<OriginalPlacableNetProps>() }
            });

            RecalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetComposition>(),
                    ComponentType.ReadOnly<RoadComposition>(),
                    ComponentType.ReadWrite<ScheduledPriceRecalculation>()
                },
            });

            RequireAnyForUpdate(InitialQuery, RecalcQuery);
        }

        protected override float PriceCoefficient() => (float)Mod.Settings.RoadPricePercentageSlider;

        protected override float UpkeepCoefficient() => (float)Mod.Settings.RoadUpkeepPercentageSlider;
    }
}