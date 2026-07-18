using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.UI
{
    public partial class UIRoadPricingSystem : AbstractUINetPricingSystem<RoadData>
    {
        private ComponentLookup<PlaceableNetData> _placeableNetDataLookup;

        protected override void OnCreate()
        {
            base.OnCreate();

            _placeableNetDataLookup = GetComponentLookup<PlaceableNetData>(isReadOnly: true);

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

        protected override float PriceCoefficient(Entity entity, RoadData detailData)
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

        protected override float UpkeepCoefficient(Entity entity, RoadData detailData)
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