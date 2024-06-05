using AudioShittifier.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class RepeatModifier : IAudioModifier
{
    // Fields.
    public double RepeatChancePerSecond { get; set; }
    public TimeSpan RepeatLengthMin { get; set; }
    public TimeSpan RepeatLengthMax { get; set; }
    public int RepeatCountMin { get; set; }
    public int RepeatCountMax { get; set; }


    // Private methods.
    private void CopySamples(SampleBuffer buffer, int sourceIndex, int destinationIndex, int count)
    {
        int Offset = destinationIndex - sourceIndex;
        if (destinationIndex - Offset < 0)
        {
            throw new ArgumentException("destination index and source index create offset which is out of bounds.",
                nameof(destinationIndex));
        }
        for (int i = destinationIndex; (i < destinationIndex + count) && (i < buffer.Samples.Length); i++)
        {
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                buffer.SetSample(i, ChannelIndex, buffer.Samples[i - Offset]);
            }
        }
    }


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        for (int Index = 0; Index < buffer.LengthPerChannel; Index += buffer.Format.SampleRate)
        {
            if (Random.Shared.NextDouble() >= RepeatChancePerSecond)
            {
                continue;
            }

            int RepeatCount = Random.Shared.Next(RepeatCountMin, RepeatCountMax + 1);
            int SamplesInSegment = (int)(RepeatLengthMin.TotalSeconds + 
                (RepeatLengthMax - RepeatLengthMin).TotalSeconds * Random.Shared.NextDouble() * buffer.Format.SampleRate);
            int SourceIndex = Index;
            Index += SamplesInSegment;

            for (int RepeatIndex = 0; RepeatIndex < RepeatCount; RepeatIndex++)
            {
                CopySamples(buffer, SourceIndex, Index, SamplesInSegment);
                Index += SamplesInSegment;
            }
        }
    }
}