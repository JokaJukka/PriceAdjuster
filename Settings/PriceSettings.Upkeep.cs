using Game.Settings;
using Game.UI;

namespace PriceAdjuster.Settings
{
    public sealed partial class PriceSettings
    {
        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(UpkeepTab, InfrastructureTypeGroup)]
        public float RoadUpkeepMultiplier { get; set; }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(UpkeepTab, InfrastructureTypeGroup)]
        public float TrackUpkeepMultiplier { get; set; }
    }
}
