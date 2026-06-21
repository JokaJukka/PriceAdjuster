using Game.Net;
using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.UI
{
    public partial class UITrackPricingSystem : AbstractUINetPricingSystem<TrackData>
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
                }
            });

            RequireAnyForUpdate(InitialQuery, RecalcQuery);
        }

        protected override float PriceCoefficient(TrackData detailData)
        {
            return detailData.m_TrackType switch
            {
                TrackTypes.Train => Mod.Settings.TrainTrackPriceMultiplier,
                TrackTypes.Tram => Mod.Settings.TramTrackPriceMultiplier,
                TrackTypes.Subway => Mod.Settings.SubwayTrackPriceMultiplier,
                _ => 1f
            };
        }

        protected override float UpkeepCoefficient(TrackData detailData)
        {
            return detailData.m_TrackType switch
            {
                TrackTypes.Train => Mod.Settings.TrainTrackUpkeepMultiplier,
                TrackTypes.Tram => Mod.Settings.TramTrackUpkeepMultiplier,
                TrackTypes.Subway => Mod.Settings.SubwayTrackUpkeepMultiplier,
                _ => 1f
            };
        }
    }
}