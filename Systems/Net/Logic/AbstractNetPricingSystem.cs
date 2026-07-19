using Game;
using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace PriceAdjuster.Systems.Net.Logic
{
    /// <summary>
    ///     This class serves as template for modifying the pricing of existing & to-be created networks.
    /// </summary>
    public abstract partial class AbstractNetPricingSystem<T> : GameSystemBase where T : unmanaged, IComponentData
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
            var entitiesData = InitialQuery.ToComponentDataArray<PlaceableNetComposition>(Allocator.Temp);
            var entitiesDetailData = InitialQuery.ToComponentDataArray<T>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = entitiesData[i];
                var originalPrices =
                    new OriginalPlaceableNetProps(entityData.m_ConstructionCost, entityData.m_UpkeepCost);
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
            var entitiesData = RecalcQuery.ToComponentDataArray<PlaceableNetComposition>(Allocator.Temp);
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

        private PlaceableNetComposition UpdatePrices(PlaceableNetComposition entityData,
            OriginalPlaceableNetProps originalPlaceableValues, T detailData)
        {
            var newPrice = originalPlaceableValues.OriginalPrice * PriceCoefficient(detailData);
            entityData.m_ConstructionCost = (uint)Mathf.Clamp(newPrice, 0, uint.MaxValue);

            var newUpkeep = originalPlaceableValues.OriginalUpkeep * UpkeepCoefficient(detailData);
            entityData.m_UpkeepCost = newUpkeep;

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