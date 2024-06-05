using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class SampleRandomizerModifier : IAudioModifier
{
    // Fields.
    public double PortionOfSamplesRandomized { get; set; } = 0.0001d;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        int SamplesRandomized = (int)(buffer.LengthPerChannel * PortionOfSamplesRandomized);

        for (int i = 0; i < SamplesRandomized; i++)
        {
            int Index1 = Random.Shared.Next(0, buffer.LengthPerChannel);
            int Index2 = Random.Shared.Next(0, buffer.LengthPerChannel);

            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                float SampleA = buffer.GetSample(Index1, ChannelIndex);
                float SampleB = buffer.GetSample(Index1, ChannelIndex);
                buffer.SetSample(Index1, ChannelIndex, SampleB);
                buffer.SetSample(Index2, ChannelIndex, SampleA);
            }
        }
    }
}