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
    public double PortionOfSamplesWithNoise { get; set; } = 0.0001d;


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