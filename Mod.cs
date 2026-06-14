using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Colossal.IO.AssetDatabase;

namespace PriceAdjuster
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(PriceAdjuster)}.{nameof(Mod)}")
            .SetShowsErrorsInUI(false);

        private Settings _mSettings;

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            _mSettings = new Settings(this);
            _mSettings.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(_mSettings));


            AssetDatabase.global.LoadSettings(nameof(PriceAdjuster), _mSettings, new Settings(this));
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
            if (_mSettings != null)
            {
                _mSettings.UnregisterInOptionsUI();
                _mSettings = null;
            }
        }
    }
}