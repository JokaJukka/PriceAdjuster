using System.Collections.Generic;
using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using Game.UI;

namespace PriceAdjuster
{
    [FileLocation("ModsSettings/PriceAdjuster/PriceAdjuster")]
    [SettingsUIGroupOrder(PresetGroup, RoadGroup, TrackGroup, AdvancedGroup)]
    [SettingsUIShowGroupName(PresetGroup, RoadGroup, TrackGroup, AdvancedGroup)]
    public sealed class Settings : ModSetting
    {
        public const string MainTab = "Main";
        public const string PresetGroup = "Presets";
        public const string RoadGroup = "Roads";
        public const string TrackGroup = "Trains & Trams";
        public const string AdvancedGroup = "Advanced";

        public Settings(IMod mod) : base(mod)
        {
        }

        private PresetsEnum Preset { get; set; }
        private float RoadPriceMultiplier { get; set; }
        private float RoadUpkeepMultiplier { get; set; }
        private float TrackPriceMultiplier { get; set; }
        private float TrackUpkeepMultiplier { get; set; }

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
                        RoadPriceMultiplier = 1;
                        RoadUpkeepMultiplier = 1;
                        TrackPriceMultiplier = 1;
                        TrackUpkeepMultiplier = 1;
                        break;
                    case PresetsEnum.Balanced:
                        RoadPriceMultiplier = 3;
                        RoadUpkeepMultiplier = 1.1f;
                        TrackPriceMultiplier = 3;
                        TrackUpkeepMultiplier = 1.1f;
                        break;
                    case PresetsEnum.Realistic:
                        RoadPriceMultiplier = 8;
                        RoadUpkeepMultiplier = 1.2f;
                        TrackPriceMultiplier = 8;
                        TrackUpkeepMultiplier = 1.2f;
                        break;
                    case PresetsEnum.Custom:
                    default:
                        break;
                }
            }
        }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, RoadGroup)]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(IsNotCustomPreset))]
        public float RoadPriceMultiplierSlider
        {
            get => RoadPriceMultiplier;
            set
            {
                RoadPriceMultiplier = value;
                Mod.SchedulePriceRecalculation();
            }
        }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, RoadGroup)]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(IsNotCustomPreset))]
        public float RoadUpkeepMultiplierSlider
        {
            get => RoadUpkeepMultiplier;
            set
            {
                RoadUpkeepMultiplier = value;
                Mod.SchedulePriceRecalculation();
            }
        }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, TrackGroup)]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(IsNotCustomPreset))]
        public float TrackPriceMultiplierSlider
        {
            get => TrackPriceMultiplier;
            set
            {
                TrackPriceMultiplier = value;
                Mod.SchedulePriceRecalculation();
            }
        }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(MainTab, TrackGroup)]
        [SettingsUIDisableByCondition(typeof(Settings), nameof(IsNotCustomPreset))]
        public float TrackUpkeepMultiplierSlider
        {
            get => TrackUpkeepMultiplier;
            set
            {
                TrackUpkeepMultiplier = value;
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
                { _mSettings.GetOptionGroupLocaleID(Settings.TrackGroup), "Trains & Trams" },
                { _mSettings.GetOptionGroupLocaleID(Settings.AdvancedGroup), "Advanced" },

                // Preset section
                { _mSettings.GetOptionLabelLocaleID(nameof(Settings.PresetsDropdown)), "Preset" },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.PresetsDropdown)),
                    "Configures the pricing with predefined preset"
                },
                { _mSettings.GetEnumValueLocaleID(Settings.PresetsEnum.Vanilla), "Vanilla - Base Game prices" },
                { _mSettings.GetEnumValueLocaleID(Settings.PresetsEnum.Balanced), "Balanced" },
                { _mSettings.GetEnumValueLocaleID(Settings.PresetsEnum.Realistic), "Realistic" },
                { _mSettings.GetEnumValueLocaleID(Settings.PresetsEnum.Custom), "Custom" },

                // Road section
                {
                    _mSettings.GetOptionLabelLocaleID(nameof(Settings.RoadPriceMultiplierSlider)),
                    "Road price multiplier"
                },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.RoadPriceMultiplierSlider)),
                    "Sets the multiplier of a price of building roads. 1 equals to vanilla game settings."
                },
                {
                    _mSettings.GetOptionLabelLocaleID(nameof(Settings.RoadUpkeepMultiplierSlider)),
                    "Road upkeep multiplier"
                },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.RoadUpkeepMultiplierSlider)),
                    "Sets the multiplier of a price of maintaining roads. 1 equals to vanilla game settings."
                },

                // Track section
                {
                    _mSettings.GetOptionLabelLocaleID(nameof(Settings.TrackPriceMultiplierSlider)),
                    "Track price multiplier"
                },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.TrackPriceMultiplierSlider)),
                    "Sets the multiplier of a price of building tracks for trains & trams. 1 equals to vanilla game settings."
                },
                {
                    _mSettings.GetOptionLabelLocaleID(nameof(Settings.TrackUpkeepMultiplierSlider)),
                    "Track upkeep multiplier"
                },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.TrackUpkeepMultiplierSlider)),
                    "Sets the multiplier of a price of maintaining tracks for trains & trams. 1 equals to vanilla game settings."
                },

                // Advanced section
                {
                    _mSettings.GetOptionLabelLocaleID(nameof(Settings.ForceRecalculatePrices)),
                    "Force recalculate prices"
                },
                {
                    _mSettings.GetOptionDescLocaleID(nameof(Settings.ForceRecalculatePrices)),
                    "Forces the mod to recalculate all the modified prices; this shouldn't be necessary though."
                },
                {
                    _mSettings.GetOptionWarningLocaleID(nameof(Settings.ForceRecalculatePrices)),
                    "Do you want to force recalculate all prices?"
                },
            };
        }

        public void Unload()
        {
        }
    }
}