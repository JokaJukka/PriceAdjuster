using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.Prefab
{
    public partial class NetObjectPricingSystem : AbstractPrefabPricingSystem<NetObjectData>
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            InitialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableObjectData>(),
                    ComponentType.ReadOnly<NetObjectData>()
                },
                None = new[] { ComponentType.ReadOnly<OriginalPlaceableNetProps>() }
            });

            RecalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableObjectData>(),
                    ComponentType.ReadOnly<NetObjectData>(),
                    ComponentType.ReadWrite<ScheduledPriceRecalculation>()
                }
            });

            RequireAnyForUpdate(InitialQuery, RecalcQuery);
        }

        protected override float PriceCoefficient(PlaceableObjectData objectData, NetObjectData detailData)
        {
            return (detailData.m_CompositionFlags.m_General & CompositionFlags.General.Roundabout) != 0
                ? Mod.Settings.RoundaboutPriceMultiplier
                : 1f;
        }
    }
}