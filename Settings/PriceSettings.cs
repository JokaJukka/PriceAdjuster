using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;

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

        [SettingsUIHidden] private float CustomRoadPriceMultiplier { get; set; }
        [SettingsUIHidden] private float CustomTrackPriceMultiplier { get; set; }

        public enum PresetsEnum
        {
            Vanilla,
            Balanced,
            Realistic,
            Custom
        }

        [SettingsUISection(PricesTab, PresetGroup)]
        public PresetsEnum Preset { get; set; }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(PricesTab, RoadTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float RoadPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 3f,
                    PresetsEnum.Realistic => 8f,
                    _ => CustomRoadPriceMultiplier
                };
            }
            set
            {
                if (Preset == PresetsEnum.Custom)
                {
                    CustomRoadPriceMultiplier = value;
                }

                Mod.SchedulePriceRecalculation();
            }
        }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(PricesTab, RoadTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float TrackPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 3f,
                    PresetsEnum.Realistic => 8f,
                    _ => CustomTrackPriceMultiplier
                };
            }
            set
            {
                if (Preset == PresetsEnum.Custom)
                {
                    CustomTrackPriceMultiplier = value;
                }

                Mod.SchedulePriceRecalculation();
            }
        }

        private bool IsNotCustomPreset() => Preset != PresetsEnum.Custom;

        public override void SetDefaults()
        {
            Preset = PresetsEnum.Vanilla;
            CustomRoadPriceMultiplier = 1f;
            CustomTrackPriceMultiplier = 1f;
        }
    }
}
