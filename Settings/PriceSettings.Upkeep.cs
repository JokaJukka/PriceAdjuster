using Game.Settings;
using Game.UI;

namespace PriceAdjuster.Settings
{
    public sealed partial class PriceSettings
    {
        [SettingsUIMultilineText]
        [SettingsUISection(UpkeepTab, RoadTypeGroup)]
        public string UpkeepNotes => string.Empty;

        [SettingsUISlider(min = 0.5f, max = 5f, step = 0.25f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(UpkeepTab, RoadTypeGroup)]
        public float RoadUpkeepMultiplier { get; set; } = 1f;

        [SettingsUISlider(min = 0.5f, max = 5f, step = 0.25f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(UpkeepTab, RoadTypeGroup)]
        public float HighwayUpkeepMultiplier { get; set; } = 1f;

        [SettingsUISlider(min = 0.5f, max = 5f, step = 0.25f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(UpkeepTab, TrackTypeGroup)]
        public float TrainTrackUpkeepMultiplier { get; set; } = 1f;

        [SettingsUISlider(min = 0.5f, max = 5f, step = 0.25f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(UpkeepTab, TrackTypeGroup)]
        public float TramTrackUpkeepMultiplier { get; set; } = 1f;

        [SettingsUISlider(min = 0.5f, max = 5f, step = 0.25f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(UpkeepTab, TrackTypeGroup)]
        public float SubwayTrackUpkeepMultiplier { get; set; } = 1f;

        private void ResetUpkeep()
        {
            RoadUpkeepMultiplier = 1f;
            HighwayUpkeepMultiplier = 1f;
            TrainTrackUpkeepMultiplier = 1f;
            TramTrackUpkeepMultiplier = 1f;
            SubwayTrackUpkeepMultiplier = 1f;
        }
    }
}