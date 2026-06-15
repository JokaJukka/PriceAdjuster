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
    public abstract partial class AbstractUINetPricingSystem : GameSystemBase
    {
        protected EntityQuery InitialQuery;
        protected EntityQuery RecalcQuery;

        protected abstract float PriceCoefficient();

        protected abstract float UpkeepCoefficient();


        protected override void OnUpdate()
        {
            InitializeNewPrices();
            RecalculatePrices();
        }

        private void InitializeNewPrices()
        {
            var entities = InitialQuery.ToEntityArray(Allocator.Temp);
            var entitiesData = InitialQuery.ToComponentDataArray<PlaceableNetData>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = entitiesData[i];
                var originalPrices = new OriginalPlaceableNetProps(entityData.m_DefaultConstructionCost,
                    entityData.m_DefaultUpkeepCost);
                entityData = UpdatePrices(entitiesData[i], originalPrices);

                EntityManager.AddComponentData(entities[i], originalPrices);
                EntityManager.SetComponentData(entities[i], entityData);
            }

            entities.Dispose();
            entitiesData.Dispose();
        }

        private void RecalculatePrices()
        {
            var entities = RecalcQuery.ToEntityArray(Allocator.Temp);
            var entitiesData = RecalcQuery.ToComponentDataArray<PlaceableNetData>(Allocator.Temp);
            var entitiesOriginalPrices = RecalcQuery.ToComponentDataArray<OriginalPlaceableNetProps>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = UpdatePrices(entitiesData[i], entitiesOriginalPrices[i]);

                EntityManager.RemoveComponent<ScheduledPriceRecalculation>(entities[i]);
                EntityManager.SetComponentData(entities[i], entityData);
            }

            entities.Dispose();
            entitiesData.Dispose();
        }

        private PlaceableNetData UpdatePrices(PlaceableNetData entityData,
            OriginalPlaceableNetProps originalPlaceableValues)
        {
            var newPrice = originalPlaceableValues.OriginalPrice * PriceCoefficient();
            entityData.m_DefaultConstructionCost = MathUtils.ClampToUInt(newPrice);

            var newUpkeep = originalPlaceableValues.OriginalUpkeep * UpkeepCoefficient();
            entityData.m_DefaultUpkeepCost = newUpkeep;

            Mod.log.Debug(
                $"Price: {originalPlaceableValues.OriginalPrice} -> {newPrice}; Upkeep: {originalPlaceableValues.OriginalUpkeep} -> {newUpkeep}");

            return entityData;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;
    }
}