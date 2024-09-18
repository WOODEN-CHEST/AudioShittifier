using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class NoiseModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("noise_amount")]
    public double PortionOfSamplesWithNoise { get; set; } = 0.0001d;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        int SamplesRandomized = (int)(buffer.LengthPerChannel * PortionOfSamplesWithNoise);

        for (int i = 0; i < SamplesRandomized; i++)
        {
            int Index = Random.Shared.Next(0, buffer.LengthPerChannel);
            for (int ChannelIndex = 0; ChannelIndex < buffer.LengthPerChannel; ChannelIndex++)
            {
                buffer.SetSample(Index, ChannelIndex, Random.Shared.NextSingle() * 2f - 1f);
            }
        }
    }
}