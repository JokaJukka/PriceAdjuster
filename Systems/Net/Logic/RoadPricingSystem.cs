using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.Net.Logic
{
    public partial class RoadPricingSystem : AbstractNetPricingSystem<RoadComposition>
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

        protected override float PriceCoefficient(RoadComposition detailData)
        {
            return (detailData.m_Flags & RoadFlags.UseHighwayRules) != 0
                ? Mod.Settings.HighwayPriceMultiplier
                : Mod.Settings.RoadPriceMultiplier;
        }

        protected override float UpkeepCoefficient(RoadComposition detailData)
        {
            return (detailData.m_Flags & RoadFlags.UseHighwayRules) != 0
                ? Mod.Settings.HighwayUpkeepMultiplier
                : Mod.Settings.RoadUpkeepMultiplier;
        }
    }
}