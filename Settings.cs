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
        private float RoadPricePercentage { get; set; }
        private float RoadUpkeepPercentage { get; set; }

        public enum PresetsEnum
        {
            Vanilla,
            Balanced,
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
                    case PresetsEnum.Balanced:
                        RoadPricePercentage = 250;
                        RoadUpkeepPercentage = 200;
                        break;
                    case PresetsEnum.Realistic:
                        RoadPricePercentage = 1000;
                        RoadUpkeepPercentage = 500;
                        break;
                    case PresetsEnum.Custom:
                    default:
                        break;
                }
            }
        }


        [SettingsUISlider(min = 0.1f, max = 100f, step = 0.1f, scalarMultiplier = 100, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, RoadGroup)]
        [SettingsUIDisableByCondition(typeof(Settings), "isNotCustomPreset")]
        public float RoadPricePercentageSlider
        {
            get => RoadPricePercentage;
            set
            {
                RoadPricePercentage = value;
                Mod.SchedulePriceRecalculation();
            }
        }


        [SettingsUISlider(min = 0.1f, max = 100f, step = 0.1f, scalarMultiplier = 100, unit = Unit.kPercentage)]
        [SettingsUISection(MainTab, RoadGroup)]
        [SettingsUIDisableByCondition(typeof(Settings), "isNotCustomPreset")]
        public float RoadUpkeepPercentageSlider
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

        public bool IsNotCustomPreset() => PresetsDropdown != PresetsEnum.Custom;


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
                { _mSettings.GetOptionGroupLocaleID(Settings.AdvancedGroup), "Advanced" },

                { _mSettings.GetOptionLabelLocaleID(nameof(Settings.PresetsDropdown)), "Preset" },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.PresetsDropdown)),
                    "Configures the pricing with predefined preset"
                },


                { _mSettings.GetEnumValueLocaleID(Settings.PresetsEnum.Vanilla), "Vanilla - Base Game prices" },
                { _mSettings.GetEnumValueLocaleID(Settings.PresetsEnum.Balanced), "Balanced" },
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
                {
                    _mSettings.GetOptionLabelLocaleID(nameof(Settings.ForceRecalculatePrices)),
                    "Force recalculate prices"
                },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.ForceRecalculatePrices)),
                    "Forces the mod to recalculate all the modified prices; this shouldn't be necessary though."
                },
            };
        }

        public void Unload()
        {
        }
    }
}