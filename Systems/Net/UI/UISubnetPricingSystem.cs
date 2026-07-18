using Game;
using Game.Prefabs;
using PriceAdjuster.Components;
using PriceAdjuster.Utils;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using SubNet = Game.Prefabs.SubNet;

namespace PriceAdjuster.Systems.Net.UI
{
    /// <summary>
    /// This system is an odd one out - it handles primarily recalculating prices of pre-made interchanges
    /// in the UI. Computes a length-weighted average price coefficient from the individual sub-net
    /// segments (comparing current vs original PlaceableNetData) and applies it to the original
    /// interchange cost.
    /// </summary>
    public partial class UISubnetPricingSystem : GameSystemBase
    {
        private EntityQuery _initialQuery;
        private EntityQuery _recalcQuery;
        private BufferLookup<SubNet> _subNetLookup;
        private ComponentLookup<PlaceableNetData> _placeableNetDataLookup;
        private ComponentLookup<OriginalPlaceableNetProps> _originalPropsLookup;

        protected override void OnCreate()
        {
            base.OnCreate();

            _subNetLookup = GetBufferLookup<SubNet>(true);
            _placeableNetDataLookup = GetComponentLookup<PlaceableNetData>(true);
            _originalPropsLookup = GetComponentLookup<OriginalPlaceableNetProps>(true);

            _initialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableObjectData>(),
                    ComponentType.ReadOnly<SubNet>()
                },
                None = new[]
                {
                    ComponentType.ReadOnly<OriginalPlaceableNetProps>(),
                    ComponentType.ReadOnly<BuildingData>()
                }
            });

            _recalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableObjectData>(),
                    ComponentType.ReadOnly<SubNet>(),
                    ComponentType.ReadWrite<ScheduledPriceRecalculation>()
                },
                None = new[]
                {
                    ComponentType.ReadOnly<BuildingData>()
                }
            });

            RequireAnyForUpdate(_initialQuery, _recalcQuery);
        }

        protected override void OnUpdate()
        {
            ProcessEntities(_initialQuery, initialize: true);
            ProcessEntities(_recalcQuery, initialize: false);
        }

        private void ProcessEntities(EntityQuery query, bool initialize)
        {
            var entities = query.ToEntityArray(Allocator.Temp);
            var objectDataArray = query.ToComponentDataArray<PlaceableObjectData>(Allocator.Temp);

            for (var i = 0; i < entities.Length; i++)
            {
                var entity = entities[i];
                var objectData = objectDataArray[i];

                if (initialize)
                {
                    var originalPrices = new OriginalPlaceableNetProps(objectData.m_ConstructionCost, 0);
                    EntityManager.AddComponentData(entity, originalPrices);
                }

                var originalCost = EntityManager.GetComponentData<OriginalPlaceableNetProps>(entity).OriginalPrice;
                var newCost = CalculateInterchangeCost(entity, originalCost);
                Mod.log.Debug($"Interchange price: {originalCost} -> {newCost}");

                objectData.m_ConstructionCost = MathUtils.ClampToUInt(newCost);

                if (!initialize)
                    EntityManager.RemoveComponent<ScheduledPriceRecalculation>(entity);

                EntityManager.SetComponentData(entity, objectData);
            }

            entities.Dispose();
            objectDataArray.Dispose();
        }

        private uint CalculateInterchangeCost(Entity entity, uint originalCost)
        {
            if (!_subNetLookup.TryGetBuffer(entity, out var subNetBuffer))
            {
                Mod.log.Warn("Failed to get subnet buffer!");
                return originalCost;
            }
            Mod.log.Info($"Calculating subnet price from {subNetBuffer.Length} elements");

            float weightedCoefficientSum = 0f;
            float totalLength = 0f;

            for (var i = 0; i < subNetBuffer.Length; i++)
            {
                var subNet = subNetBuffer[i];
                var length = Colossal.Mathematics.MathUtils.Length(subNet.m_Curve);

                var coefficient = GetSubNetCoefficient(subNet.m_Prefab);

                weightedCoefficientSum += length * coefficient;
                totalLength += length;
            }

            if (totalLength <= 0f)
                return originalCost;

            var weightedCoefficient = weightedCoefficientSum / totalLength;
            Mod.log.Debug($"Interchange coefficient: {weightedCoefficient:F2}");

            return MathUtils.ClampToUInt(originalCost * weightedCoefficient);
        }

        private float GetSubNetCoefficient(Entity prefabEntity)
        {
            if (!_placeableNetDataLookup.TryGetComponent(prefabEntity, out var placeableNetData))
                return 1f;

            if (!_originalPropsLookup.TryGetComponent(prefabEntity, out var originalProps))
                return 1f;

            if (originalProps.OriginalPrice == 0)
                return 1f;

            return (float)placeableNetData.m_DefaultConstructionCost / originalProps.OriginalPrice;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 256;
        }
    }
}