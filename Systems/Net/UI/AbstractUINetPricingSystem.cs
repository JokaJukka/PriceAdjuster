using Game;
using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace PriceAdjuster.Systems.Net.UI
{
    /// <summary>
    ///     This class serves as template for modifying the pricing of networks only in UI.
    /// </summary>
    public abstract partial class AbstractUINetPricingSystem<T> : GameSystemBase where T : unmanaged, IComponentData
    {
        protected EntityQuery InitialQuery;
        protected EntityQuery RecalcQuery;

        protected abstract float PriceCoefficient(T detailData);

        protected abstract float UpkeepCoefficient(T detailData);

        protected override void OnUpdate()
        {
            InitializeNewPrices();
            RecalculatePrices();
        }

        private void InitializeNewPrices()
        {
            var entities = InitialQuery.ToEntityArray(Allocator.Temp);
            var entitiesData = InitialQuery.ToComponentDataArray<PlaceableNetData>(Allocator.Temp);
            var entitiesDetailData = InitialQuery.ToComponentDataArray<T>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = entitiesData[i];
                var originalPrices = new OriginalPlaceableNetProps(entityData.m_DefaultConstructionCost,
                    entityData.m_DefaultUpkeepCost);
                entityData = UpdatePrices(entitiesData[i], originalPrices, entitiesDetailData[i]);

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
            var entitiesDetailData = RecalcQuery.ToComponentDataArray<T>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = UpdatePrices(entitiesData[i], entitiesOriginalPrices[i], entitiesDetailData[i]);

                EntityManager.RemoveComponent<ScheduledPriceRecalculation>(entities[i]);
                EntityManager.SetComponentData(entities[i], entityData);
            }

            entities.Dispose();
            entitiesData.Dispose();
        }

        private PlaceableNetData UpdatePrices(PlaceableNetData entityData,
            OriginalPlaceableNetProps originalPlaceableValues, T detailData)
        {
            var newPrice = originalPlaceableValues.OriginalPrice * PriceCoefficient(detailData);
            entityData.m_DefaultConstructionCost = (uint)Mathf.Clamp(newPrice, 0, uint.MaxValue);

            var newUpkeep = originalPlaceableValues.OriginalUpkeep * UpkeepCoefficient(detailData);
            entityData.m_DefaultUpkeepCost = newUpkeep;

            Mod.log.Debug(
                $"Price: {originalPlaceableValues.OriginalPrice} -> {newPrice}; Upkeep: {originalPlaceableValues.OriginalUpkeep} -> {newUpkeep}");

            return entityData;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 2048;
        }
    }
}