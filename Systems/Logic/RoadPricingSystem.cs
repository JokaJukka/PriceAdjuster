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
                None = new[] { ComponentType.ReadOnly<OriginalPlaceableNetProps>() }
            });

            RecalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetComposition>(),
                    ComponentType.ReadOnly<RoadComposition>(),
                    ComponentType.ReadWrite<ScheduledPriceRecalculation>()
                }
            });

            RequireAnyForUpdate(InitialQuery, RecalcQuery);
        }

        protected override float PriceCoefficient() => Mod.Settings.RoadPriceMultiplier;

        protected override float UpkeepCoefficient() => Mod.Settings.RoadUpkeepMultiplier;
    }
}