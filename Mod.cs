using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Game.UI;
using PriceAdjuster.Components;
using PriceAdjuster.Locale;
using PriceAdjuster.Settings;
using PriceAdjuster.Systems.Net.Logic;
using PriceAdjuster.Systems.Net.UI;
using PriceAdjuster.Systems.Prefab;
using Unity.Collections;
using Unity.Entities;

namespace PriceAdjuster
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(PriceAdjuster)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);

        private static EntityManager _entityManager;

        public static PriceSettings Settings { get; private set; }

        public void OnLoad(UpdateSystem updateSystem)
        {
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            _entityManager = updateSystem.EntityManager;

            Settings = new PriceSettings(this);
            Settings.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(Settings));

            AssetDatabase.global.LoadSettings(nameof(PriceAdjuster), Settings);

            // Prefab updates
            updateSystem.UpdateAt<NetObjectPricingSystem>(SystemUpdatePhase.Modification1);

            // Net Logic updates
            updateSystem.UpdateAt<RoadPricingSystem>(SystemUpdatePhase.Modification1);
            updateSystem.UpdateAt<TrackPricingSystem>(SystemUpdatePhase.Modification1);

            // Interchange pricing (must be after net logic systems to use adjusted PlaceableNetComposition)
            // updateSystem.UpdateAt<InterchangePricingSystem>(SystemUpdatePhase.Modification2);

            // Net UI updates
            updateSystem.UpdateAt<UIRoadPricingSystem>(SystemUpdatePhase.Modification1);
            updateSystem.UpdateAt<UITrackPricingSystem>(SystemUpdatePhase.Modification1);
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (Settings != null)
            {
                Settings.UnregisterInOptionsUI();
                Settings = null;
            }
        }

        public static void SchedulePriceRecalculation()
        {
            var query = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<OriginalPlaceableNetProps>());
            var entities = query.ToEntityArray(Allocator.Temp);

            if (entities.Length > 0)
                log.Info($"Scheduling price recalculation for {entities.Length} entities!");

            foreach (var entity in entities) _entityManager.AddComponent<ScheduledPriceRecalculation>(entity);

            entities.Dispose();
        }
    }
}