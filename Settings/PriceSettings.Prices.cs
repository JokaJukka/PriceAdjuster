using Game.Settings;
using Game.UI;

namespace PriceAdjuster.Settings
{
    public sealed partial class PriceSettings
    {
        public enum PresetsEnum
        {
            Vanilla,
            Balanced,
            Realistic,
            Custom
        }

        [SettingsUIHidden] public float CustomRoadPriceMultiplier = 1f;
        [SettingsUIHidden] public float CustomHighwayPriceMultiplier = 1f;
        [SettingsUIHidden] public float CustomRoundaboutPriceMultiplier = 1f;
        [SettingsUIHidden] public float CustomTrainTrackPriceMultiplier = 1f;
        [SettingsUIHidden] public float CustomTramTrackPriceMultiplier = 1f;
        [SettingsUIHidden] public float CustomSubwayTrackPriceMultiplier = 1f;

        [SettingsUISection(PricesTab, PresetGroup)]
        public PresetsEnum Preset { get; set; }

        [SettingsUISlider(min = 0.5f, max = 25f, step = 0.5f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(PricesTab, RoadTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float RoadPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 2.5f,
                    PresetsEnum.Realistic => 6f,
                    _ => CustomRoadPriceMultiplier
                };
            }
            set => CustomRoadPriceMultiplier = value;
        }

        [SettingsUISlider(min = 0.5f, max = 25f, step = 0.5f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(PricesTab, RoadTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float HighwayPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 8f,
                    PresetsEnum.Realistic => 20f,
                    _ => CustomHighwayPriceMultiplier
                };
            }
            set => CustomHighwayPriceMultiplier = value;
        }

        [SettingsUISlider(min = 0.5f, max = 25f, step = 0.5f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(PricesTab, RoadTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float RoundaboutPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 2f,
                    PresetsEnum.Realistic => 5f,
                    _ => CustomRoundaboutPriceMultiplier
                };
            }
            set => CustomRoundaboutPriceMultiplier = value;
        }

        [SettingsUISlider(min = 0.5f, max = 25f, step = 0.5f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(PricesTab, TrackTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float TrainTrackPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 5f,
                    PresetsEnum.Realistic => 12f,
                    _ => CustomTrainTrackPriceMultiplier
                };
            }
            set => CustomTrainTrackPriceMultiplier = value;
        }

        [SettingsUISlider(min = 0.5f, max = 25f, step = 0.5f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(PricesTab, TrackTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float TramTrackPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 8f,
                    PresetsEnum.Realistic => 20f,
                    _ => CustomTramTrackPriceMultiplier
                };
            }
            set => CustomTramTrackPriceMultiplier = value;
        }

        [SettingsUISlider(min = 0.5f, max = 25f, step = 0.5f, unit = Unit.kFloatTwoFractions)]
        [SettingsUISection(PricesTab, TrackTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float SubwayTrackPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 8f,
                    PresetsEnum.Realistic => 20f,
                    _ => CustomSubwayTrackPriceMultiplier
                };
            }
            set => CustomSubwayTrackPriceMultiplier = value;
        }
        
        [SettingsUISection(PricesTab, MiscGroup)]
        [SettingsUIButton]
        [SettingsUIConfirmation]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public bool ResetCustomPricesButton
        {
            get => false;
            set
            {
                if (value) ResetCustomPrices();
            }
        }

        private bool IsNotCustomPreset()
        {
            return Preset != PresetsEnum.Custom;
        }

        private void ResetPrices()
        {
            Preset = PresetsEnum.Vanilla;
        }

        private void ResetCustomPrices()
        {
            CustomRoadPriceMultiplier = 1f;
            CustomHighwayPriceMultiplier = 1f;
            CustomRoundaboutPriceMultiplier = 1f;
            CustomTrainTrackPriceMultiplier = 1f;
            CustomTramTrackPriceMultiplier = 1f;
            CustomSubwayTrackPriceMultiplier = 1f;
        }
    }
}