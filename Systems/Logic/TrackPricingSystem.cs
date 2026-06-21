using System;
using Game.Net;
using Game.Prefabs;
using PriceAdjuster.Components;
using Unity.Entities;

namespace PriceAdjuster.Systems.Logic
{
    public partial class TrackPricingSystem : AbstractNetPricingSystem<TrackComposition>
    {
        protected override void OnCreate()
        {
            base.OnCreate();

            InitialQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetComposition>(),
                    ComponentType.ReadOnly<TrackComposition>()
                },
                None = new[] { ComponentType.ReadOnly<OriginalPlaceableNetProps>() }
            });

            RecalcQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new[]
                {
                    ComponentType.ReadWrite<PlaceableNetComposition>(),
                    ComponentType.ReadOnly<TrackComposition>(),
                    ComponentType.ReadWrite<ScheduledPriceRecalculation>()
                }
            });

            RequireAnyForUpdate(InitialQuery, RecalcQuery);
        }

        protected override float PriceCoefficient(TrackComposition detailData)
        {
            return detailData.m_TrackType switch
            {
                TrackTypes.Train => Mod.Settings.TrainTrackPriceMultiplier,
                TrackTypes.Tram => Mod.Settings.TramTrackPriceMultiplier,
                TrackTypes.Subway => Mod.Settings.SubwayTrackPriceMultiplier,
                _ => 1f
            };
        }

        protected override float UpkeepCoefficient(TrackComposition detailData)
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