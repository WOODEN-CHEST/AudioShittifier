using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("repeat")]
public class RepeatModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("repeats_per_second")]
    public double RepeatsPerSecond { get; set; }

    [AudioModifierProperty("length_min")]
    public TimeSpan RepeatLengthMin { get; set; }

    [AudioModifierProperty("length_max")]
    public TimeSpan RepeatLengthMax { get; set; }

    [AudioModifierProperty("copies_min")]
    public int CopyCountMin { get; set; }

    [AudioModifierProperty("copies_max")]
    public int CopyCountMax { get; set; }


    // Private methods.
    private void CopySamples(SampleBuffer buffer, int sourceIndex, int destinationIndex, int count)
    {
        int Offset = destinationIndex - sourceIndex;
        if (destinationIndex - Offset < 0)
        {
            throw new ArgumentException("destination index and source index create offset which is out of bounds.",
                nameof(destinationIndex));
        }
        for (int i = destinationIndex; (i < destinationIndex + count) && (i < buffer.LengthPerChannel); i++)
        {
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                buffer.SetSample(i, ChannelIndex, buffer.GetSample(i - Offset, ChannelIndex));
            }
        }
    }


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        /* Technically a buffer copy should be made here to avoid issues,
         * but the issues make the sound even worse so that's a win. */
        int TotalRepeatCount = (int)(buffer.LengthPerChannel / (double)buffer.Format.SampleRate * RepeatsPerSecond);
        for (int i = 0; i < TotalRepeatCount; i++)
        {
            int CopyCount = Random.Shared.Next(CopyCountMin, CopyCountMax + 1);
            int SamplesInSegment = (int)(RepeatLengthMin.TotalSeconds + 
                (RepeatLengthMax - RepeatLengthMin).TotalSeconds * Random.Shared.NextDouble() * buffer.Format.SampleRate);
            int SourceIndex = Random.Shared.Next(buffer.LengthPerChannel);
            int DestinationIndex = SourceIndex + CopyCount;

            for (int RepeatIndex = 0; RepeatIndex < CopyCount; RepeatIndex++)
            {
                CopySamples(buffer, SourceIndex, DestinationIndex, SamplesInSegment);
                DestinationIndex += SamplesInSegment;
            }
        }
    }
}