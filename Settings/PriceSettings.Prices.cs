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

        [SettingsUIHidden] public float CustomRoadPriceMultiplier { get; set; } = 1f;
        [SettingsUIHidden] public float CustomHighwayPriceMultiplier { get; set; } = 1f;
        [SettingsUIHidden] public float CustomTrainTrackPriceMultiplier { get; set; } = 1f;
        [SettingsUIHidden] public float CustomTramTrackPriceMultiplier { get; set; } = 1f;
        [SettingsUIHidden] public float CustomSubwayTrackPriceMultiplier { get; set; } = 1f;

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
        public float HighwayPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 3f,
                    PresetsEnum.Realistic => 8f,
                    _ => CustomHighwayPriceMultiplier
                };
            }
            set
            {
                if (Preset == PresetsEnum.Custom)
                {
                    CustomHighwayPriceMultiplier = value;
                }

                Mod.SchedulePriceRecalculation();
            }
        }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(PricesTab, TrackTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float TrainTrackPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 3f,
                    PresetsEnum.Realistic => 8f,
                    _ => CustomTrainTrackPriceMultiplier
                };
            }
            set
            {
                if (Preset == PresetsEnum.Custom)
                {
                    CustomTrainTrackPriceMultiplier = value;
                }

                Mod.SchedulePriceRecalculation();
            }
        }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(PricesTab, TrackTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float TramTrackPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 3f,
                    PresetsEnum.Realistic => 8f,
                    _ => CustomTramTrackPriceMultiplier
                };
            }
            set
            {
                if (Preset == PresetsEnum.Custom)
                {
                    CustomTramTrackPriceMultiplier = value;
                }

                Mod.SchedulePriceRecalculation();
            }
        }

        [SettingsUISlider(min = 0.1f, max = 10f, step = 0.1f, scalarMultiplier = 1, unit = Unit.kFloatSingleFraction)]
        [SettingsUISection(PricesTab, TrackTypeGroup)]
        [SettingsUIDisableByCondition(typeof(PriceSettings), nameof(IsNotCustomPreset))]
        public float SubwayTrackPriceMultiplier
        {
            get
            {
                return Preset switch
                {
                    PresetsEnum.Vanilla => 1f,
                    PresetsEnum.Balanced => 3f,
                    PresetsEnum.Realistic => 8f,
                    _ => CustomSubwayTrackPriceMultiplier
                };
            }
            set
            {
                if (Preset == PresetsEnum.Custom)
                {
                    CustomSubwayTrackPriceMultiplier = value;
                }

                Mod.SchedulePriceRecalculation();
            }
        }

        private bool IsNotCustomPreset() => Preset != PresetsEnum.Custom;

        private void ResetPrices()
        {
            Preset = PresetsEnum.Vanilla;
            CustomRoadPriceMultiplier = 1f;
            CustomHighwayPriceMultiplier = 1f;
            CustomTrainTrackPriceMultiplier = 1f;
            CustomTramTrackPriceMultiplier = 1f;
            CustomSubwayTrackPriceMultiplier = 1f;
        }
    }
}