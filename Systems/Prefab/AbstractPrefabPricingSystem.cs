using Game;
using Game.Prefabs;
using PriceAdjuster.Components;
using PriceAdjuster.Utils;
using Unity.Collections;
using Unity.Entities;

namespace PriceAdjuster.Systems.Prefab
{
    /// <summary>
    ///     This class serves as template for modifying pricing of prefabs.
    /// </summary>
    public abstract partial class AbstractPrefabPricingSystem<T> : GameSystemBase where T : unmanaged, IComponentData
    {
        protected EntityQuery InitialQuery;
        protected EntityQuery RecalcQuery;

        protected abstract float PriceCoefficient(PlaceableObjectData objectData, T detailData);

        protected override void OnUpdate()
        {
            InitializeNewPrices();
            RecalculatePrices();
        }

        private void InitializeNewPrices()
        {
            var entities = InitialQuery.ToEntityArray(Allocator.Temp);
            var entitiesData = InitialQuery.ToComponentDataArray<PlaceableObjectData>(Allocator.Temp);
            var entitiesDetailData = InitialQuery.ToComponentDataArray<T>(Allocator.Temp);

            for (var i = 0; i < entitiesData.Length; i++)
            {
                var entityData = entitiesData[i];
                var originalPrices = new OriginalPlaceableNetProps(entityData.m_ConstructionCost, 0);
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
            var entitiesData = RecalcQuery.ToComponentDataArray<PlaceableObjectData>(Allocator.Temp);
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

        private PlaceableObjectData UpdatePrices(PlaceableObjectData entityData,
            OriginalPlaceableNetProps originalPlaceableValues, T detailData)
        {
            var newPrice = originalPlaceableValues.OriginalPrice * PriceCoefficient(entityData, detailData);
            entityData.m_ConstructionCost = MathUtils.ClampToUInt(newPrice);

            Mod.log.Debug($"Price: {originalPlaceableValues.OriginalPrice} -> {newPrice}");

            return entityData;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 256;
        }
    }
}