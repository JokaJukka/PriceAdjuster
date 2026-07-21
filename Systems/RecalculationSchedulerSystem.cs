using Game;
using PriceAdjuster.Components;
using Unity.Collections;
using Unity.Entities;

namespace PriceAdjuster.Systems
{
    public partial class RecalculationSchedulerSystem : GameSystemBase
    {
        private float _lastRecalculationTime = -1f;

        private EntityQuery _query;

        protected override void OnCreate()
        {
            base.OnCreate();

            _query = GetEntityQuery(
                new EntityQueryDesc
                {
                    All = new[] { ComponentType.ReadOnly<OriginalPlaceableNetProps>() }
                });
        }

        protected override void OnUpdate()
        {
            var now = UnityEngine.Time.unscaledTime;
            if (Mod.ScheduledRecalculationTime > now || Mod.ScheduledRecalculationTime <= _lastRecalculationTime) return;
            _lastRecalculationTime = now;

            var entities = _query.ToEntityArray(Allocator.Temp);

            if (entities.Length > 0)
                Mod.log.Info($"Recalculating prices for {entities.Length} entities!");

            foreach (var entity in entities) EntityManager.AddComponent<ScheduledPriceRecalculation>(entity);

            entities.Dispose();
        }

        public override int GetUpdateInterval(SystemUpdatePhase phase)
        {
            return 2048;
        }
    }
}