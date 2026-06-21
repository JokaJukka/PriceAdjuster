using System.Collections.Generic;
using Colossal;
using PriceAdjuster.Settings;

namespace PriceAdjuster.Locale
{
    public class LocaleEN : IDictionarySource
    {
        private readonly PriceSettings _settings;

        public LocaleEN(PriceSettings settings)
        {
            _settings = settings;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                // Tabs & Groups
                { _settings.GetSettingsLocaleID(), "PriceAdjuster" },
                { _settings.GetOptionTabLocaleID(PriceSettings.PricesTab), "Prices" },
                { _settings.GetOptionTabLocaleID(PriceSettings.UpkeepTab), "Upkeep" },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.UpkeepNotes)),
                    "Unlike the prices, upkeep seems to be quite balanced in the base game and is not generally recommended to change it."
                },
                { _settings.GetOptionTabLocaleID(PriceSettings.AdvancedTab), "Advanced" },
                { _settings.GetOptionGroupLocaleID(PriceSettings.PresetGroup), "Presets" },
                { _settings.GetOptionGroupLocaleID(PriceSettings.RoadTypeGroup), "Road Type" },
                { _settings.GetOptionGroupLocaleID(PriceSettings.DebugGroup), "Debug" },

                // Prices tab
                { _settings.GetOptionLabelLocaleID(nameof(PriceSettings.Preset)), "Preset" },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.Preset)),
                    "Configures the pricing with predefined preset"
                },
                {
                    _settings.GetEnumValueLocaleID(PriceSettings.PresetsEnum.Vanilla),
                    "Vanilla - Base Game prices"
                },
                { _settings.GetEnumValueLocaleID(PriceSettings.PresetsEnum.Balanced), "Balanced" },
                { _settings.GetEnumValueLocaleID(PriceSettings.PresetsEnum.Realistic), "Realistic" },
                { _settings.GetEnumValueLocaleID(PriceSettings.PresetsEnum.Custom), "Custom" },

                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.RoadPriceMultiplier)),
                    "Road price multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.RoadPriceMultiplier)),
                    "Sets the multiplier of a price of building roads. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.HighwayPriceMultiplier)),
                    "Highway price multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.HighwayPriceMultiplier)),
                    "Sets the multiplier of a price of building highways. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.TrackPriceMultiplier)),
                    "Track price multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.TrackPriceMultiplier)),
                    "Sets the multiplier of a price of building tracks for trains & trams. 1 equals to vanilla game settings."
                },

                // Upkeep tab
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.RoadUpkeepMultiplier)),
                    "Road upkeep multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.RoadUpkeepMultiplier)),
                    "Sets the multiplier of a price of maintaining roads. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.RoadUpkeepMultiplier)),
                    "Highway upkeep multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.RoadUpkeepMultiplier)),
                    "Sets the multiplier of a price of maintaining highways. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.TrackUpkeepMultiplier)),
                    "Track upkeep multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.TrackUpkeepMultiplier)),
                    "Sets the multiplier of a price of maintaining tracks for trains & trams. 1 equals to vanilla game settings."
                },

                // Advanced tab
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.ForceRecalculatePrices)),
                    "Force recalculate all prices"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.ForceRecalculatePrices)),
                    "Forces the mod to recalculate all the modified prices; this shouldn't be necessary though."
                },
                {
                    _settings.GetOptionWarningLocaleID(nameof(PriceSettings.ForceRecalculatePrices)),
                    "Do you want to force recalculate all prices?"
                }
            };
        }

        public void Unload()
        {
        }
    }
}