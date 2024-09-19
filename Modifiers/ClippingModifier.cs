using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("clipping")]
public class ClippingModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("per_second")]
    public double ClipsPerSecond
    {
        get => _clipsPerSecond;
        set => _clipsPerSecond = double.IsNaN(value) ? CLIPS_PER_SECOND_DEFAULT
            : Math.Clamp(value, CLIPS_PER_SECOND_MIN, CLIPS_PER_SECOND_MAX);
    }

    [AudioModifierProperty("duration_min")]
    public TimeSpan ClipDurationMin { get; set; }

    [AudioModifierProperty("duration_max")]
    public TimeSpan ClipDurationMax { get; set; }

    // Private static fields.
    private const double CLIPS_PER_SECOND_MIN = 0d;
    private const double CLIPS_PER_SECOND_MAX = 10_000d;
    private const double CLIPS_PER_SECOND_DEFAULT = 0d;


    // Private fields.
    private double _clipsPerSecond = CLIPS_PER_SECOND_DEFAULT;


    // Private methods.
    private void ClipSegment(SampleBuffer buffer, int index, int count)
    {
        for (int i = index; (i < index + count) && (i < buffer.LengthPerChannel); i++)
        {
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                buffer.SetSample(i, ChannelIndex, buffer.GetSample(index, ChannelIndex));
            }
        }
    }


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        int ClipCount = (int)((double)buffer.LengthPerChannel / buffer.Format.SampleRate  * ClipsPerSecond);

        for (int i = 0; i < ClipCount; i++)
        {
            int SamplesInSegment = (int)(ClipDurationMin.TotalSeconds +
                (ClipDurationMax - ClipDurationMin).TotalSeconds * Random.Shared.NextDouble() * buffer.Format.SampleRate);
            ClipSegment(buffer, Random.Shared.Next(buffer.LengthPerChannel), SamplesInSegment);
        }
    }
}