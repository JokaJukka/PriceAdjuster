using Game.Settings;

namespace PriceAdjuster.Settings
{
    public sealed partial class PriceSettings
    {
        [SettingsUISection(AdvancedTab, MiscGroup)]
        [SettingsUIButton]
        [SettingsUIConfirmation]
        public bool ForceRecalculatePricesButton
        {
            get => false;
            set
            {
                if (value) Mod.SchedulePriceRecalculation();
            }
        }
    }
}