using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.Logic
{
    public partial class RoadPricingSystem : AbstractNetPricingSystem<RoadComposition>
    {
        private ComponentLookup<Game.Prefabs.PlaceableNetData> _placeableNetDataLookup;

        protected override void OnCreate()
        {
            base.OnCreate();

            _placeableNetDataLookup = GetComponentLookup<Game.Prefabs.PlaceableNetData>(isReadOnly: true);

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

        protected override float PriceCoefficient(Entity entity, RoadComposition detailData)
        {
            if (_placeableNetDataLookup.TryGetComponent(entity, out var placeableNetData))
            {
                if ((placeableNetData.m_SetUpgradeFlags.m_General & CompositionFlags.General.Roundabout) != 0)
                    return Mod.Settings.RoundaboutPriceMultiplier;
                if ((placeableNetData.m_SetUpgradeFlags.m_General & CompositionFlags.General.DeadEnd) != 0)
                    return Mod.Settings.CulDeSacPriceMultiplier;
            }

            return (detailData.m_Flags & RoadFlags.UseHighwayRules) != 0
                ? Mod.Settings.HighwayPriceMultiplier
                : Mod.Settings.RoadPriceMultiplier;
        }

        protected override float UpkeepCoefficient(Entity entity, RoadComposition detailData)
        {
            if (_placeableNetDataLookup.TryGetComponent(entity, out var placeableNetData))
            {
                if ((placeableNetData.m_SetUpgradeFlags.m_General & CompositionFlags.General.Roundabout) != 0)
                    return Mod.Settings.RoundaboutUpkeepMultiplier;
                if ((placeableNetData.m_SetUpgradeFlags.m_General & CompositionFlags.General.DeadEnd) != 0)
                    return Mod.Settings.CulDeSacUpkeepMultiplier;
            }

            return (detailData.m_Flags & RoadFlags.UseHighwayRules) != 0
                ? Mod.Settings.HighwayUpkeepMultiplier
                : Mod.Settings.RoadUpkeepMultiplier;
        }
    }
}