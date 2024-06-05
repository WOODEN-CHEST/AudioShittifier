using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class EchoModifier : IAudioModifier
{
    // Fields.
    public TimeSpan Offset { get; set; }
    public float EchoVolume { get; set; } = 1f;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        SampleBuffer CopyBuffer = new(buffer.GetCopyOfSamples(), buffer.Format);

        int SampleOffset = (int)(Offset.TotalSeconds * buffer.Format.SampleRate);
        for (int i = Math.Max(0, SampleOffset); Math.Max(i, i - SampleOffset) < buffer.LengthPerChannel; i++)
        {
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                buffer.SetSample(i, ChannelIndex, CopyBuffer.GetSample(i, ChannelIndex) 
                    + CopyBuffer.GetSample(i - SampleOffset, ChannelIndex) * EchoVolume);
            }
        }
    }
}