using Game.Settings;
using Game.UI;

namespace PriceAdjuster.Settings
{
    public sealed partial class PriceSettings
    {
        [SettingsUIMultilineText]
        [SettingsUISection(UpkeepTab, RoadTypeGroup)]
        public string UpkeepNotes => string.Empty;

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(UpkeepTab, RoadTypeGroup)]
        public float RoadUpkeepMultiplier { get; set; } = 1f;

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(UpkeepTab, RoadTypeGroup)]
        public float TrackUpkeepMultiplier { get; set; } = 1f;

        private void ResetUpkeep()
        {
            RoadUpkeepMultiplier = 1f;
            TrackUpkeepMultiplier = 1f;
        }
    }
}