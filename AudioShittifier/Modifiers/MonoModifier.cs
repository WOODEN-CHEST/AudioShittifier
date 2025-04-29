using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("mono")]
public class MonoModifier : IAudioModifier
{
    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        float[] ChannelBuffer = new float[buffer.Format.Channels];
        for (int i = 0; i < buffer.LengthPerChannel; i++)
        {
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                ChannelBuffer[ChannelIndex] = buffer.GetSample(i, ChannelIndex);
            }

            float AverageSample = 0f;
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                AverageSample += ChannelBuffer[ChannelIndex];
            }
            AverageSample /= ChannelBuffer.Length;

            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                buffer.SetSample(i, ChannelIndex, AverageSample);
            }
        }
    }
}