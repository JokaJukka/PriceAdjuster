using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;

namespace PriceAdjuster.Settings
{
    [FileLocation("ModsSettings/PriceAdjuster/PriceAdjuster")]
    [SettingsUIGroupOrder(PresetGroup, RoadTypeGroup, InfrastructureTypeGroup, DebugGroup)]
    [SettingsUIShowGroupName(PresetGroup, RoadTypeGroup, InfrastructureTypeGroup, DebugGroup)]
    [SettingsUITabOrder(PricesTab, UpkeepTab, AdvancedTab)]
    public sealed partial class PriceSettings : ModSetting
    {
        public const string PricesTab = "Prices";
        public const string UpkeepTab = "Upkeep";
        public const string AdvancedTab = "Advanced";
        
        public const string PresetGroup = "Presets";
        public const string RoadTypeGroup = "Road Type";
        public const string InfrastructureTypeGroup = "Infrastructure Type";
        public const string DebugGroup = "Debug";

        public PriceSettings(IMod mod) : base(mod)
        {
        }

        public override void SetDefaults()
        {
            Preset = PresetsEnum.Vanilla;
            CustomRoadPriceMultiplier = 1f;
            CustomTrackPriceMultiplier = 1f;
            RoadUpkeepMultiplier = 1f;
            TrackUpkeepMultiplier = 1f;
        }
    }
}
