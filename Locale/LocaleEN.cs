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
                { _settings.GetOptionGroupLocaleID(PriceSettings.RoadTypeGroup), "Road settings" },
                { _settings.GetOptionGroupLocaleID(PriceSettings.TrackTypeGroup), "Track settings" },
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
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.RoundaboutPriceMultiplier)),
                    "Roundabout price multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.RoundaboutPriceMultiplier)),
                    "Sets the multiplier of a price of building roundabouts and cul-de-sacs. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.TrainTrackPriceMultiplier)),
                    "Train track price multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.TrainTrackPriceMultiplier)),
                    "Sets the multiplier of a price of building train tracks. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.TramTrackPriceMultiplier)),
                    "Tram track price multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.TramTrackPriceMultiplier)),
                    "Sets the multiplier of a price of building tram tracks. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.SubwayTrackPriceMultiplier)),
                    "Subway track price multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.SubwayTrackPriceMultiplier)),
                    "Sets the multiplier of a price of building subway tracks. 1 equals to vanilla game settings."
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
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.HighwayUpkeepMultiplier)),
                    "Highway upkeep multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.HighwayUpkeepMultiplier)),
                    "Sets the multiplier of a price of maintaining highways. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.TrainTrackUpkeepMultiplier)),
                    "Train track upkeep multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.TrainTrackUpkeepMultiplier)),
                    "Sets the multiplier of a price of maintaining train tracks. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.TramTrackUpkeepMultiplier)),
                    "Tram track upkeep multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.TramTrackUpkeepMultiplier)),
                    "Sets the multiplier of a price of maintaining tram tracks. 1 equals to vanilla game settings."
                },
                {
                    _settings.GetOptionLabelLocaleID(nameof(PriceSettings.SubwayTrackUpkeepMultiplier)),
                    "Subway track upkeep multiplier"
                },
                {
                    _settings.GetOptionDescLocaleID(nameof(PriceSettings.SubwayTrackUpkeepMultiplier)),
                    "Sets the multiplier of a price of maintaining subway tracks. 1 equals to vanilla game settings."
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