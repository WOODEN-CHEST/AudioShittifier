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
    [AudioModifierProperty("clips_per_second")]
    public double ClipsPerSecond { get; set; }

    [AudioModifierProperty("clip_length_min")]
    public TimeSpan ClipLengthMin { get; set; }

    [AudioModifierProperty("clip_length_max")]
    public TimeSpan ClipLengthMax { get; set; }


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
            int SamplesInSegment = (int)(ClipLengthMin.TotalSeconds +
                (ClipLengthMax - ClipLengthMin).TotalSeconds * Random.Shared.NextDouble() * buffer.Format.SampleRate);
            ClipSegment(buffer, Random.Shared.Next(buffer.LengthPerChannel), SamplesInSegment);
        }
    }
}