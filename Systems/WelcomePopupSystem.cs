using Colossal.Serialization.Entities;
using Game;
using Game.SceneFlow;
using Game.UI;
using Game.UI.Localization;
using PriceAdjuster.Settings;

namespace PriceAdjuster.Systems
{
    public partial class WelcomePopupSystem : GameSystemBase
    {
        public const string PopupTitleKey = "PriceAdjuster.Welcome[TITLE]";
        public const string PopupMessageKey = "PriceAdjuster.Welcome[MESSAGE]";
        public const string PopupKeepVanillaKey = "PriceAdjuster.Welcome[KEEP_VANILLA]";
        public const string PopupApplyBalancedKey = "PriceAdjuster.Welcome[APPLY_BALANCED]";
        public const string PopupApplyRealisticKey = "PriceAdjuster.Welcome[APPLY_REALISTIC]";

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            if (mode != GameMode.MainMenu || Mod.Settings.HasSeenWelcome)
                return;

            var dialog = new ConfirmationDialog(
                LocalizedString.Id(PopupTitleKey),
                LocalizedString.Id(PopupMessageKey),
                LocalizedString.Id(PopupApplyBalancedKey),
                LocalizedString.Id(PopupKeepVanillaKey),
                LocalizedString.Id(PopupApplyRealisticKey)
            );

            GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(dialog, OnDialogResult);
        }

        private void OnDialogResult(int result)
        {
            Mod.Settings.HasSeenWelcome = true;
            Mod.Settings.Preset = result switch
            {
                1 => PriceSettings.PresetsEnum.Balanced,
                2 => PriceSettings.PresetsEnum.Realistic,
                _ => Mod.Settings.Preset
            };
        }

        protected override void OnUpdate()
        {
        }
    }
}