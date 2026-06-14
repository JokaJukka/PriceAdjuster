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
        private EntityQuery _initialQuery;
        private EntityQuery _recalcQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            _initialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] { ComponentType.ReadWrite<PlaceableNetData>() },
                None = new[] { ComponentType.ReadOnly<NetPriceAdjusted>() }
            });

            _recalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetData>(),
                    ComponentType.ReadWrite<ScheduledPriceRecalculation>()
                },
            });


            RequireAnyForUpdate(_initialQuery, _recalcQuery);
        }

        protected override void OnUpdate()
        {
            InitializeNewPrices();
            RecalculatePrices();
        }

        private void InitializeNewPrices()
        {
            var entities = _initialQuery.ToEntityArray(Allocator.Temp);
            var roads = _initialQuery.ToComponentDataArray<PlaceableNetData>(Allocator.Temp);

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

                EntityManager.AddComponentData(entities[i], new NetPriceAdjusted(oldPrice, oldUpkeep));
                EntityManager.SetComponentData(entities[i], road);
            }
            
            entities.Dispose();
            roads.Dispose();
        }

        private void RecalculatePrices()
        {
            var entities = _recalcQuery.ToEntityArray(Allocator.Temp);
            var roads = _recalcQuery.ToComponentDataArray<PlaceableNetData>(Allocator.Temp);
            var originalData = _recalcQuery.ToComponentDataArray<NetPriceAdjusted>(Allocator.Temp);

            for (var i = 0; i < roads.Length; i++)
            {
                var road = roads[i];
                if (road.m_DefaultConstructionCost == 0)
                {
                    // Skip not-yet initialized ecs & zero-priced entities 
                    continue;
                }

                var oldPrice = originalData[i].OriginalPrice;
                var newPrice = oldPrice * Mod.Settings.RoadPricePercentageSlider / 100;
                Mod.log.Info($"Modifying price of {road} from {oldPrice} to {newPrice}");
                road.m_DefaultConstructionCost = ClampToUInt(newPrice);

                var oldUpkeep = originalData[i].OriginalUpkeep;
                var newUpkeep = oldUpkeep * Mod.Settings.RoadUpkeepPercentageSlider / 100;
                Mod.log.Info($"Modifying price of {road} from {oldUpkeep} to {newUpkeep}");
                road.m_DefaultUpkeepCost = newUpkeep;

                EntityManager.RemoveComponent<ScheduledPriceRecalculation>(entities[i]);
                EntityManager.SetComponentData(entities[i], road);
            }
            
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