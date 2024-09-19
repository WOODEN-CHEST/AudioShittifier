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
    [AudioModifierProperty("per_second")]
    public double RepeatsPerSecond
    {
        get => _repeatsPerSecond;
        set
        {
            _repeatsPerSecond = double.IsNaN(value) ? REPEATS_PER_SECOND_DEFAULT
                : Math.Clamp(value, REPEATS_PER_SECOND_MIN, REPEATS_PER_SECOND_MAX);
        }
    }

    [AudioModifierProperty("duration_min")]
    public TimeSpan RepeatDurationMin { get; set; }

    [AudioModifierProperty("duration_max")]
    public TimeSpan RepeatDurationMax { get; set; }

    [AudioModifierProperty("count_min")]
    public int CopyCountMin
    {
        get => _copyCountMin;
        set
        {
            _copyCountMin = Math.Clamp(value, COPY_COUNT_MIN, COPY_COUNT_MAX);
            _copyCountMax = Math.Max(_copyCountMin, _copyCountMax);
        }
    }

    [AudioModifierProperty("count_max")]
    public int CopyCountMax
    {
        get => _copyCountMax;
        set
        {
            _copyCountMax = Math.Clamp(value, COPY_COUNT_MIN, COPY_COUNT_MAX);
            _copyCountMin = Math.Min(_copyCountMin, _copyCountMax);
        }
    }


    // Private static fields.
    private const double REPEATS_PER_SECOND_DEFAULT = 0d;
    private const double REPEATS_PER_SECOND_MAX = 10_000;
    private const double REPEATS_PER_SECOND_MIN = 0;
    private const int COPY_COUNT_MIN = 0;
    private const int COPY_COUNT_MAX = 10_000;
    private const int COPY_COUNT_DEFAULT = 0;


    // Private fields.
    private double _repeatsPerSecond = REPEATS_PER_SECOND_DEFAULT;
    private int _copyCountMin = COPY_COUNT_MIN;
    private int _copyCountMax = COPY_COUNT_MAX;


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
            int SamplesInSegment = (int)(RepeatDurationMin.TotalSeconds + 
                (RepeatDurationMax - RepeatDurationMin).TotalSeconds * Random.Shared.NextDouble() * buffer.Format.SampleRate);
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