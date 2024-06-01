using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class SineWaveSoundModifier : IAudioModifier
{
    // Fields.
    public double ChancePerSecond { get; set; }
    public TimeSpan AverageSectionDuration { get; set; }



    // Private static fields.
    private const float MAX_FREQUENCY = 3000f;


    // Private methods.
    private void CreateNoise(float[] samples, int index, int count, WaveFormat audioFormat, float frequency)
    {
        for (int i = index; (i < count + index) && (i < samples.Length - (audioFormat.Channels - 1)); i += 2)
        {
            float Sample = MathF.Sin((i * MathF.PI) / audioFormat.SampleRate * frequency);
            samples[i] = Sample;
            samples[i + 1] = Sample;
        }
    }


    // Inherited methods.
    public void Modify(float[] samples, WaveFormat audioFormat)
    {
        for (int Index = 0; Index < samples.Length; Index += audioFormat.SampleRate * audioFormat.Channels)
        {
            if (Random.Shared.NextDouble() >= ChancePerSecond)
            {
                continue;
            }

            int SampleCount = (int)(ShittifierRandom.RandomOffsetValue(AverageSectionDuration.TotalSeconds)
                * audioFormat.SampleRate * audioFormat.Channels);
            CreateNoise(samples, Index, SampleCount, audioFormat, 300f + (Random.Shared.NextSingle() * MAX_FREQUENCY));
            Index += SampleCount;
        }
    }
}