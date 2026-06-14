using Game;
using Game.Prefabs;
using PriceAdjuster.Components;
using PriceAdjuster.Utils;
using Unity.Collections;
using Unity.Entities;

namespace PriceAdjuster.Systems.UI
{
    /// <summary>
    /// This class modifies just the pricing of networks in UI.
    /// </summary>
    public partial class UIPlaceableNetPricingSystem : GameSystemBase
    {
        private EntityQuery _initialQuery;
        private EntityQuery _recalcQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            _initialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] { ComponentType.ReadWrite<PlaceableNetData>() },
                None = new[] { ComponentType.ReadOnly<OriginalPlacableNetData>() }
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
            var entitiesData = _initialQuery.ToComponentDataArray<PlaceableNetData>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = entitiesData[i];
                var originalPrices = new OriginalPlacableNetData(entityData.m_DefaultConstructionCost, entityData.m_DefaultUpkeepCost);
                entityData = UpdatePrices(entitiesData[i], originalPrices);

                EntityManager.AddComponentData(entities[i], originalPrices);
                EntityManager.SetComponentData(entities[i], entityData);
            }
            
            entities.Dispose();
            entitiesData.Dispose();
        }

        private void RecalculatePrices()
        {
            var entities = _recalcQuery.ToEntityArray(Allocator.Temp);
            var entitiesData = _recalcQuery.ToComponentDataArray<PlaceableNetData>(Allocator.Temp);
            var entitiesOriginalPrices = _recalcQuery.ToComponentDataArray<OriginalPlacableNetData>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = UpdatePrices(entitiesData[i], entitiesOriginalPrices[i]);

                EntityManager.RemoveComponent<ScheduledPriceRecalculation>(entities[i]);
                EntityManager.SetComponentData(entities[i], entityData);
            }
            
            entities.Dispose();
            entitiesData.Dispose();
            
        }

        private PlaceableNetData UpdatePrices(PlaceableNetData entityData, OriginalPlacableNetData originalPlacableValues)
        {
            var newPrice = originalPlacableValues.OriginalPrice * Mod.Settings.RoadPricePercentageSlider / 100;
            Mod.log.Info($"Modifying price from {originalPlacableValues.OriginalPrice} to {newPrice}");
            entityData.m_DefaultConstructionCost = MathUtils.ClampToUInt(newPrice);
            
            var newUpkeep = originalPlacableValues.OriginalUpkeep * Mod.Settings.RoadUpkeepPercentageSlider / 100;
            Mod.log.Info($"Modifying price from {originalPlacableValues.OriginalUpkeep} to {newUpkeep}");
            entityData.m_DefaultUpkeepCost = newUpkeep;

            return entityData;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;
    }
}