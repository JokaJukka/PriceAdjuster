using Game.Settings;

namespace PriceAdjuster.Settings
{
    public sealed partial class PriceSettings
    {
        [SettingsUISection(AdvancedTab, DebugGroup)]
        [SettingsUIButton]
        [SettingsUIConfirmation()]
        public bool ForceRecalculatePrices
        {
            get => false;
            set
            {
                if (value) Mod.SchedulePriceRecalculation();
            }
        }
    }
}