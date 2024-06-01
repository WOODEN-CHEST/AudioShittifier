using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public static class ShittifierRandom
{
    public static bool TryWithChance(double chance, double intensity)
    {
        return Random.Shared.NextDouble() < Math.Pow(intensity, 1f / chance);
    }

    public static double RandomNumberWithIntensity(double bestValue, double worstValue, double intensity)
    {
        double InterpolationAmount = Random.Shared.NextDouble() * intensity;
        return bestValue + ((worstValue - bestValue) * InterpolationAmount);
    }

    public static double RandomOffsetValue(double averageValue)
    {
        return ((Random.Shared.NextSingle() - 0.5f) * averageValue * 0.85f) + averageValue;
    }
}