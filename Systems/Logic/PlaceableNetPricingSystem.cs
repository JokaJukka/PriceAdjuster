using Game;
using Game.Prefabs;
using PriceAdjuster.Components;
using PriceAdjuster.Utils;
using Unity.Collections;
using Unity.Entities;

namespace PriceAdjuster.Systems.Logic
{
    /// <summary>
    /// This class modifies the pricing of existing & to-be created networks.
    /// </summary>
    public partial class PlaceableNetPricingSystem : GameSystemBase
    {
        private EntityQuery _initialQuery;
        private EntityQuery _recalcQuery;

        protected override void OnCreate()
        {
            base.OnCreate();

            _initialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[] { ComponentType.ReadWrite<PlaceableNetComposition>() },
                None = new[] { ComponentType.ReadOnly<OriginalPlacableNetProps>() }
            });

            _recalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetComposition>(),
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
            var entitiesData = _initialQuery.ToComponentDataArray<PlaceableNetComposition>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = entitiesData[i];
                var originalPrices =
                    new OriginalPlacableNetProps(entityData.m_ConstructionCost, entityData.m_UpkeepCost);
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
            var entitiesData = _recalcQuery.ToComponentDataArray<PlaceableNetComposition>(Allocator.Temp);
            var entitiesOriginalPrices = _recalcQuery.ToComponentDataArray<OriginalPlacableNetProps>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = UpdatePrices(entitiesData[i], entitiesOriginalPrices[i]);

                EntityManager.RemoveComponent<ScheduledPriceRecalculation>(entities[i]);
                EntityManager.SetComponentData(entities[i], entityData);
            }

            entities.Dispose();
            entitiesData.Dispose();
        }

        private PlaceableNetComposition UpdatePrices(PlaceableNetComposition entityData,
            OriginalPlacableNetProps originalPlacableValues)
        {
            var newPrice = originalPlacableValues.OriginalPrice * Mod.Settings.RoadPricePercentageSlider / 100;
            entityData.m_ConstructionCost = MathUtils.ClampToUInt(newPrice);

            var newUpkeep = originalPlacableValues.OriginalUpkeep * Mod.Settings.RoadUpkeepPercentageSlider / 100;
            entityData.m_UpkeepCost = newUpkeep;
            
            Mod.log.Debug(
                $"Price: {originalPlacableValues.OriginalPrice} -> {newPrice}; Upkeep: {originalPlacableValues.OriginalUpkeep} -> {newUpkeep}");

            return entityData;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;
    }
}