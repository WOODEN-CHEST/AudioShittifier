using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("swap")]
public class SwapModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("per_second")]
    public double SwapsPerSecond
    {
        get => _swapsPerSecond;
        set => _swapsPerSecond = double.IsNaN(value) ? SWAPS_PER_SECOND_DEFAULT
            : Math.Clamp(value, SWAPS_PER_SECOND_MIN, SWAPS_PER_SECOND_MAX);
    }

    [AudioModifierProperty("duration_min")]
    public TimeSpan SwapDurationMin { get; set; } = TimeSpan.FromSeconds(0d);

    [AudioModifierProperty("duration_max")]
    public TimeSpan SwapDurationMax { get; set; } = TimeSpan.FromSeconds(0d);


    // Private static fields.
    private const double SWAPS_PER_SECOND_DEFAULT = 0d;
    private const double SWAPS_PER_SECOND_MIN = 0d;
    private const double SWAPS_PER_SECOND_MAX = 10_000d;


    // Private fields.
    private double _swapsPerSecond = SWAPS_PER_SECOND_DEFAULT;

    // Private methods.
    private void Swap(SampleBuffer buffer, int sourceIndex, int destIndex, int sampleCount)
    {
        for (int i = 0; (i < sampleCount); i++)
        {
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                float SampleSource = buffer.GetSample(sourceIndex + i, ChannelIndex);
                float SampleDest = buffer.GetSample(destIndex + i, ChannelIndex);
                buffer.SetSample(destIndex + i, ChannelIndex, SampleSource);
                buffer.SetSample(sourceIndex + i, ChannelIndex, SampleDest);
            }
        }
    }


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        int SwapCount = (int)(buffer.LengthPerChannel / (double)buffer.Format.SampleRate * SwapsPerSecond);

        for (int i = 0; i < SwapCount; i++)
        {
            double SwapDurationSeconds = SwapDurationMin.TotalSeconds + ((SwapDurationMax - SwapDurationMin).TotalSeconds
                * Random.Shared.NextDouble());
            int SampleCount = (int)(buffer.Format.SampleRate * SwapDurationSeconds);
            int StartIndex1 = Random.Shared.Next(buffer.LengthPerChannel);
            int StartIndex2 = Random.Shared.Next(buffer.LengthPerChannel);
            Swap(buffer, StartIndex1, StartIndex2, SampleCount);
        }
    }
}