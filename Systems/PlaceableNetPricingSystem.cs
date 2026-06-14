using Game;
using Game.Prefabs;
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

            _query = GetEntityQuery(
                ComponentType.ReadWrite<PlaceableNetData>()
            );

            RequireForUpdate(_query);
        }

        protected override void OnUpdate()
        {
            var roads = _query.ToComponentDataArray<PlaceableNetData>(Allocator.Temp);

            for (int i = 0; i < roads.Length; i++)
            {
                var road = roads[i];
                if (road.m_DefaultConstructionCost == 0)
                {
                    continue;
                }

                var newPrice = road.m_DefaultConstructionCost + 4;
                Mod.log.Info(
                    $"Modifying price of {road.ToString()} from {road.m_DefaultConstructionCost} to {newPrice}");
                road.m_DefaultConstructionCost = newPrice;
                roads[i] = road;
            }

            _query.CopyFromComponentDataArray(roads);
        }
    }
}