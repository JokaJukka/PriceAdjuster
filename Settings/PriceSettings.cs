using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;

namespace PriceAdjuster.Settings
{
    [FileLocation("ModsSettings/PriceAdjuster/PriceAdjuster")]
    [SettingsUITabOrder(PricesTab, UpkeepTab, AdvancedTab)]
    [SettingsUIGroupOrder(PresetGroup, RoadTypeGroup, DebugGroup)]
    [SettingsUIShowGroupName]
    public sealed partial class PriceSettings : ModSetting
    {
        public const string PricesTab = "Prices";
        public const string UpkeepTab = "Upkeep";
        public const string AdvancedTab = "Advanced";

        public const string PresetGroup = "Presets";
        public const string RoadTypeGroup = "Road settings";
        public const string TrackTypeGroup = "Track settings";
        public const string DebugGroup = "Debug";

        [SettingsUIHidden] public bool HasSeenWelcome { get; set; } = false;

        public PriceSettings(IMod mod) : base(mod)
        {
            onSettingsApplied += setting =>
            {
                Mod.log.Warn(setting);
            };
        }

        public override void Apply()
        {
            base.Apply();
            Mod.SchedulePriceRecalculation();
        }

        public override void SetDefaults()
        {
            ResetPrices();
            ResetUpkeep();
        }
    }
}