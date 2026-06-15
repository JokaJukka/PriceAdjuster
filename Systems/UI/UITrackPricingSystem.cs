using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.UI
{
    public partial class UITrackPricingSystem : AbstractUINetPricingSystem
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            InitialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetData>(),
                    ComponentType.ReadOnly<TrackData>()
                },
                None = new[] { ComponentType.ReadOnly<OriginalPlaceableNetProps>() }
            });

            RecalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetData>(),
                    ComponentType.ReadOnly<TrackData>(),
                    ComponentType.ReadWrite<ScheduledPriceRecalculation>()
                },
            });

            RequireAnyForUpdate(InitialQuery, RecalcQuery);
        }

        protected override float PriceCoefficient() => Mod.Settings.TrackPriceMultiplierSlider;

        protected override float UpkeepCoefficient() => Mod.Settings.TrackUpkeepMultiplierSlider;
    }
}