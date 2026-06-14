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
                    continue;
                }
                var newPrice = road.m_DefaultConstructionCost + 1000;
                Mod.log.Info($"Modifying price of {road} from {road.m_DefaultConstructionCost} to {newPrice}");
                road.m_DefaultConstructionCost = newPrice;

                EntityManager.AddComponent<PriceAdjusted>(entities[i]);
                EntityManager.SetComponentData(entities[i], road);
            }

            // _query.CopyFromComponentDataArray(roads);
            entities.Dispose();
            roads.Dispose();
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;
    }
}