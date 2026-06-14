using System;

namespace PriceAdjuster.Utils
{
    public class MathUtils
    {
        public static uint ClampToUInt(float value)
        {
            switch (value)
            {
                case > uint.MaxValue:
                    Mod.log.Warn($"Clamping {value} to {uint.MaxValue}");
                    return uint.MaxValue;
                case < uint.MinValue:
                    Mod.log.Warn($"Clamping {value} to {uint.MinValue}");
                    return uint.MinValue;
                default:
                    return Convert.ToUInt32(value);
            }
        }
    }
}