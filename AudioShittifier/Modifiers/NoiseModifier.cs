using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("noise")]
public class NoiseModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("amount")]
    public double PortionOfSamplesWithNoise
    {
        get => _portionOfSamplesWithNoise;
        set => _portionOfSamplesWithNoise = double.IsNaN(value) ? NOISE_PORTION_DEFAULT
            : Math.Clamp(value, NOISE_PORTION_MIN, NOISE_PORTION_MAX);
    }


    // Private static fields.
    private const double NOISE_PORTION_DEFAULT = 0.0001d;
    private const double NOISE_PORTION_MIN = 0f;
    private const double NOISE_PORTION_MAX = 1d;


    // Private fields.
    private double _portionOfSamplesWithNoise = NOISE_PORTION_DEFAULT;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        int SamplesRandomized = (int)(buffer.LengthPerChannel * PortionOfSamplesWithNoise);

        for (int i = 0; i < SamplesRandomized; i++)
        {
            int Index = Random.Shared.Next(0, buffer.LengthPerChannel);
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                buffer.SetSample(Index, ChannelIndex, Random.Shared.NextSingle() * 2f - 1f);
            }
        }
    }
}