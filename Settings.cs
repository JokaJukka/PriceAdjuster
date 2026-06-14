using System.Collections.Generic;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;

namespace PriceAdjuster
{
    [FileLocation("ModsSettings/PriceAdjuster/PriceAdjuster")]
    [SettingsUIGroupOrder(PresetGroup, RoadGroup, AdvancedGroup)]
    [SettingsUIShowGroupName(PresetGroup, RoadGroup, AdvancedGroup)]
    public sealed class Settings : ModSetting
    {
        public const string MainTab = "Main";
        public const string PresetGroup = "Presets";
        public const string RoadGroup = "Roads";
        public const string AdvancedGroup = "Advanced";

        public Settings(IMod mod) : base(mod)
        {
        }

        private PresetsEnum Preset { get; set; }
        private int RoadPricePercentage { get; set; }
        private int RoadUpkeepPercentage { get; set; }

        public enum PresetsEnum
        {
            Vanilla,
            Realistic,
            Custom
        }

        [SettingsUISection(MainTab, PresetGroup)]
        public PresetsEnum PresetsDropdown
        {
            get => Preset;
            set
            {
                Mod.log.Info($"Switched preset from {Preset} to {value}");
                Preset = value;
                switch (value)
                {
                    case PresetsEnum.Vanilla:
                        RoadPricePercentage = 100;
                        RoadUpkeepPercentage = 100;
                        break;
                    case PresetsEnum.Realistic:
                        RoadPricePercentage = 200;
                        RoadUpkeepPercentage = 200;
                        break;
                    case PresetsEnum.Custom:
                    default:
                        break;
                }
            }
        }


        [SettingsUISlider(min = 10, max = 1000, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, RoadGroup)]
        [SettingsUIDisableByCondition(typeof(Settings), "isNotCustomPreset")]
        public int RoadPricePercentageSlider
        {
            get => RoadPricePercentage;
            set
            {
                RoadPricePercentage = value;
                Mod.SchedulePriceRecalculation();
            }
        }


        [SettingsUISlider(min = 10, max = 1000, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, RoadGroup)]
        [SettingsUIDisableByCondition(typeof(Settings), "isNotCustomPreset")]
        public int RoadUpkeepPercentageSlider
        {
            get => RoadUpkeepPercentage;
            set
            {
                RoadUpkeepPercentage = value;
                Mod.SchedulePriceRecalculation();
            }
        }

        [SettingsUISection(MainTab, AdvancedGroup)]
        [SettingsUIButton]
        [SettingsUIConfirmation(null, null)]
        public bool ForceRecalculatePrices
        {
            get => false;
            set
            {
                if (value)
                {
                    Mod.SchedulePriceRecalculation();
                }
            }
        }

        public bool IsNotCustomPreset() => Preset != PresetsEnum.Custom;


        public override void SetDefaults()
        {
            PresetsDropdown = PresetsEnum.Vanilla;
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Settings _mSettings;

        public LocaleEN(Settings settings)
        {
            _mSettings = settings;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { _mSettings.GetSettingsLocaleID(), "PriceAdjuster" },
                { _mSettings.GetOptionTabLocaleID(Settings.MainTab), "Main" },
                { _mSettings.GetOptionGroupLocaleID(Settings.PresetGroup), "Presets" },
                { _mSettings.GetOptionGroupLocaleID(Settings.RoadGroup), "Roads" },

                { _mSettings.GetOptionLabelLocaleID(nameof(Settings.PresetsDropdown)), "Preset" },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.PresetsDropdown)),
                    "Configures the pricing with predefined preset"
                },


                { _mSettings.GetEnumValueLocaleID(Settings.PresetsEnum.Vanilla), "Vanilla" },
                { _mSettings.GetEnumValueLocaleID(Settings.PresetsEnum.Realistic), "Realistic" },
                { _mSettings.GetEnumValueLocaleID(Settings.PresetsEnum.Custom), "Custom" },

                {
                    _mSettings.GetOptionLabelLocaleID(nameof(Settings.RoadPricePercentageSlider)),
                    "Road price multiplier"
                },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.RoadPricePercentageSlider)),
                    "Sets the percentage of a price of building roads. 100% equals to vanilla game settings."
                },
                {
                    _mSettings.GetOptionLabelLocaleID(nameof(Settings.RoadUpkeepPercentageSlider)),
                    "Road upkeep multiplier"
                },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.RoadUpkeepPercentageSlider)),
                    "Sets the percentage of a price of maintaining roads. 100% equals to vanilla game settings."
                },
            };
        }

        public void Unload()
        {
        }
    }
}