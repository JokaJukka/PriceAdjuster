using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using PriceAdjuster.Locale;
using PriceAdjuster.Settings;
using PriceAdjuster.Systems;
using PriceAdjuster.Systems.Net.Logic;
using PriceAdjuster.Systems.Net.UI;
using PriceAdjuster.Systems.Prefab;
using UnityEngine;

namespace PriceAdjuster
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(PriceAdjuster)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);

        public static float ScheduledRecalculationTime { get; private set; } = -1f;

        public static PriceSettings Settings { get; private set; }

        public void OnLoad(UpdateSystem updateSystem)
        {
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            Settings = new PriceSettings(this);
            Settings.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(Settings));

            AssetDatabase.global.LoadSettings(nameof(PriceAdjuster), Settings);

            // Welcome popup
            updateSystem.UpdateAt<WelcomePopupSystem>(SystemUpdatePhase.Modification1);

            // Prefab updates
            updateSystem.UpdateAt<NetObjectPricingSystem>(SystemUpdatePhase.Modification1);

            // Net Logic updates
            updateSystem.UpdateAt<RoadPricingSystem>(SystemUpdatePhase.Modification1);
            updateSystem.UpdateAt<TrackPricingSystem>(SystemUpdatePhase.Modification1);

            // Net UI updates
            updateSystem.UpdateAt<UIRoadPricingSystem>(SystemUpdatePhase.Modification1);
            updateSystem.UpdateAt<UITrackPricingSystem>(SystemUpdatePhase.Modification1);

            // Interchange UI updates (must be after net logic systems to use adjusted PlaceableNetComposition)
            updateSystem.UpdateAt<UISubnetPricingSystem>(SystemUpdatePhase.Modification2);

            // Scheduler for debounced price recalculations
            updateSystem.UpdateAt<RecalculationSchedulerSystem>(SystemUpdatePhase.Modification1);
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
            ScheduledRecalculationTime = Time.unscaledTime + 1f;
            log.Debug($"Scheduled recalc at time {ScheduledRecalculationTime}");
        }
    }
}