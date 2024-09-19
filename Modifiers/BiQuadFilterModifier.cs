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
    public int Frequency
    {
        get => _frequency;
        set => _frequency = Math.Clamp(value, FREQUENCY_MIN, FREQUENCY_MAX);
    }

    [AudioModifierProperty("type")]
    public BiQualFilterPassType PassType { get; set; } = BiQualFilterPassType.HighPass;

    [AudioModifierProperty("order")]
    public int FilterOrder
    {
        get => _filterOrder;
        set => _filterOrder = Math.Clamp(value, FILTER_ORDER_MIN, FILTER_ORDER_MAX);
    }

    // Private static fields.
    private const int FREQUENCY_MIN = 0;
    private const int FREQUENCY_MAX = 192000;
    private const int FREQUENCY_DEFAULT = 22050;
    private const int FILTER_ORDER_MIN = 0;
    private const int FILTER_ORDER_MAX = 20;
    private const int FILTER_ORDER_DEFAULT = 3;

    // Private fields.
    private int _frequency = FREQUENCY_DEFAULT;
    private int _filterOrder = FILTER_ORDER_DEFAULT;


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