using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("biquadfilter")]
public class BiQuadFilterModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("frequency")]
    public int Frequency { get; set; } = 2000;

    [AudioModifierProperty("type")]
    public BiQualFilterPassType PassType { get; set; } = BiQualFilterPassType.HighPass;

    [AudioModifierProperty("order")]
    public int FilterOrder { get; set; } = 3;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        BiQuadFilter[] Filters = new BiQuadFilter[buffer.Format.Channels];
        for (int i = 0; i < Filters.Length; i++)
        {
            Filters[i] = PassType switch
            {
                BiQualFilterPassType.HighPass => BiQuadFilter.HighPassFilter(
                buffer.Format.SampleRate, Math.Min(Frequency, buffer.Format.SampleRate / 2), FilterOrder),

                BiQualFilterPassType.LowPass => BiQuadFilter.LowPassFilter(
                buffer.Format.SampleRate, Math.Min(Frequency, buffer.Format.SampleRate / 2), FilterOrder),

                _ => throw new ArgumentException($"Invalid filter pass type \"{PassType}\" ({(int)PassType})",
                nameof(PassType))
            };
        }

        for (int SampleIndex = 0; SampleIndex < buffer.LengthPerChannel; SampleIndex++)
        {
            for (int ChannelIndex = 0; ChannelIndex < Filters.Length; ChannelIndex++)
            {
                float Sample = buffer.GetSample(SampleIndex, ChannelIndex);
                buffer.SetSample(SampleIndex, ChannelIndex, Filters[ChannelIndex].Transform(Sample));
            }
        }
    }
}