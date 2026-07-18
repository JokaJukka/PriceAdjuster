using Game;
using Game.Net;
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
    /// in the UI. 
    /// </summary>
    public partial class UISubnetPricingSystem : GameSystemBase
    {
        private EntityQuery _initialQuery;
        private EntityQuery _recalcQuery;
        private BufferLookup<SubNet> _subNetLookup;
        private ComponentLookup<PlaceableNetComposition> _compositionLookup;

        protected override void OnCreate()
        {
            base.OnCreate();

            _subNetLookup = GetBufferLookup<SubNet>(true);
            _compositionLookup = GetComponentLookup<PlaceableNetComposition>(true);

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

                var newCost = CalculateInterchangeCost(entity);
                Mod.log.Debug($"Interchange price: {objectData.m_ConstructionCost} -> {newCost}");

                objectData.m_ConstructionCost = MathUtils.ClampToUInt(newCost);

                if (!initialize)
                    EntityManager.RemoveComponent<ScheduledPriceRecalculation>(entity);

                EntityManager.SetComponentData(entity, objectData);
            }

            entities.Dispose();
            objectDataArray.Dispose();
        }

        private uint CalculateInterchangeCost(Entity entity)
        {
            if (!_subNetLookup.TryGetBuffer(entity, out var subNetBuffer))
                return 0;

            uint totalCost = 0;

            for (var i = 0; i < subNetBuffer.Length; i++)
            {
                var subNet = subNetBuffer[i];

                if (!_compositionLookup.TryGetComponent(subNet.m_Prefab, out var composition))
                    continue;

                var length = Colossal.Mathematics.MathUtils.Length(subNet.m_Curve);
                var curve = new Curve
                {
                    m_Bezier = subNet.m_Curve,
                    m_Length = length
                };

                var startElevation = new Elevation(new float2(subNet.m_Curve.a.y, subNet.m_Curve.a.y));
                var endElevation = new Elevation(new float2(subNet.m_Curve.d.y, subNet.m_Curve.d.y));

                totalCost += (uint)NetUtils.GetConstructionCost(curve, startElevation, endElevation, composition);
            }

            return totalCost;
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 256;
        }
    }
}