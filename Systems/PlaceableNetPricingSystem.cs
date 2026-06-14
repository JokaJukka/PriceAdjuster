using System;
using Game;
using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Collections;
using Unity.Entities;

namespace PriceAdjuster.Systems
{
    public partial class PlaceableNetPricingSystem : GameSystemBase
    {
        private EntityQuery _query;

        protected override void OnCreate()
        {
            base.OnCreate();

            _query = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] { ComponentType.ReadWrite<PlaceableNetData>() },
                None = new[] { ComponentType.ReadOnly<PriceAdjusted>() }
            });

            RequireForUpdate(_query);
        }

        protected override void OnUpdate()
        {
            var entities = _query.ToEntityArray(Allocator.Temp);
            var roads = _query.ToComponentDataArray<PlaceableNetData>(Allocator.Temp);

            for (var i = 0; i < roads.Length; i++)
            {
                var road = roads[i];
                if (road.m_DefaultConstructionCost == 0)
                {
                    // Skip not-yet initialized ecs & zero-priced entities 
                    continue;
                }

                var oldPrice = road.m_DefaultConstructionCost;
                var newPrice = oldPrice * Mod.Settings.RoadPricePercentageSlider / 100;
                Mod.log.Info($"Modifying price of {road} from {oldPrice} to {newPrice}");
                road.m_DefaultConstructionCost = ClampToUInt(newPrice);

                var oldUpkeep = road.m_DefaultUpkeepCost;
                var newUpkeep = oldUpkeep * Mod.Settings.RoadUpkeepPercentageSlider / 100;
                Mod.log.Info($"Modifying price of {road} from {oldUpkeep} to {newUpkeep}");
                road.m_DefaultUpkeepCost = newUpkeep;

                EntityManager.AddComponentData(entities[i], new PriceAdjusted(oldPrice, oldUpkeep));
                EntityManager.SetComponentData(entities[i], road);
            }

            // _query.CopyFromComponentDataArray(roads);
            entities.Dispose();
            roads.Dispose();
        }

        private uint ClampToUInt(long value)
        {
            switch (value)
            {
                case > uint.MaxValue:
                    Mod.log.Warn($"Clamping {value} to {uint.MaxValue}");
                    return uint.MaxValue;
                case < uint.MinValue:
                    Mod.log.Warn($"Clamping {value} to {uint.MinValue}");
                    return uint.MinValue;
                default:
                    return Convert.ToUInt32(value);
            }
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;
    }
}