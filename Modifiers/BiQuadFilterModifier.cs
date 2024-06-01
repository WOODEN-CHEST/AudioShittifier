using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

internal class BiQuadFilterModifier : IAudioModifier
{
    // Fields.
    public int Frequency { get; set; } = 2000;
    public BiQualFilterPassType PassType { get; set; } = BiQualFilterPassType.HighPass;


    // Inherited methods.
    public void Modify(float[] samples, WaveFormat audioFormat)
    {
        NAudio.Dsp.BiQuadFilter[] Filters = new NAudio.Dsp.BiQuadFilter[audioFormat.Channels];
        for (int i = 0; i < Filters.Length; i++)
        {
            Filters[i] = PassType switch
            {
                BiQualFilterPassType.HighPass => NAudio.Dsp.BiQuadFilter.HighPassFilter(
                audioFormat.SampleRate, Math.Min(Frequency, audioFormat.SampleRate / 2), 3),

                BiQualFilterPassType.LowPass => NAudio.Dsp.BiQuadFilter.LowPassFilter(
                audioFormat.SampleRate, Math.Min(Frequency, audioFormat.SampleRate / 2), 3),

                _ => throw new ArgumentException($"Invalid filter pass type \"{PassType}\" ({(int)PassType})",
                nameof(PassType))
            };
        }

        for (int SampleIndex = 0; SampleIndex < samples.Length; SampleIndex += Filters.Length)
        {
            for (int FilterIndex = 0; FilterIndex < Filters.Length; FilterIndex++)
            {
                float Sample = samples[SampleIndex + FilterIndex];
                samples[SampleIndex + FilterIndex] = Filters[FilterIndex].Transform(Sample);
            }
        }
    }
}