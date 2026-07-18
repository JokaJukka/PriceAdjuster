using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.Net.UI
{
    public partial class UIRoadPricingSystem : AbstractUINetPricingSystem<RoadData>
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

        protected override float PriceCoefficient(RoadData detailData)
        {
            return (detailData.m_Flags & RoadFlags.UseHighwayRules) != 0
                ? Mod.Settings.HighwayPriceMultiplier
                : Mod.Settings.RoadPriceMultiplier;
        }

        protected override float UpkeepCoefficient(RoadData detailData)
        {
            return (detailData.m_Flags & RoadFlags.UseHighwayRules) != 0
                ? Mod.Settings.HighwayUpkeepMultiplier
                : Mod.Settings.RoadUpkeepMultiplier;
        }
    }
}