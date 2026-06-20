using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.UI
{
    public partial class UIRoadPricingSystem : AbstractUINetPricingSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            InitialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetData>(),
                    ComponentType.ReadOnly<RoadData>()
                },
                None = new[] { ComponentType.ReadOnly<OriginalPlaceableNetProps>() }
            });

            RecalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetData>(),
                    ComponentType.ReadOnly<RoadData>(),
                    ComponentType.ReadWrite<ScheduledPriceRecalculation>()
                }
            });

            RequireAnyForUpdate(InitialQuery, RecalcQuery);
        }

        protected override float PriceCoefficient() => Mod.Settings.RoadPriceMultiplier;

        protected override float UpkeepCoefficient() => Mod.Settings.RoadUpkeepMultiplier;
    }
}