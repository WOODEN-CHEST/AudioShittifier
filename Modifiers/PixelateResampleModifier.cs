using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class PixelateResampleModifier : IAudioModifier
{
    // Fields.
    public int SampleRate
    {
        get => _sampleRate;
        set => _sampleRate = Math.Clamp(value, 1, 192000);
    }



    // Private fields.
    private int _sampleRate = 44100;


    // Inherited methods.
    // Doesn't even work correctly, but that makes it better!
    public void Modify(float[] samples, WaveFormat audioFormat)
    {

        float[] OriginalSamples = new float[samples.Length];
        Buffer.BlockCopy(samples, 0, OriginalSamples, 0, samples.Length * sizeof(float));

        float SampleRatio = (float)Math.Min(audioFormat.SampleRate, SampleRate) / audioFormat.SampleRate;

        for (int i = 0; i < samples.Length; i++)
        {
            int SampleIndex = Math.Clamp((int)(MathF.Floor(i * SampleRatio) / SampleRatio), 0, samples.Length - 1);
            samples[i] = OriginalSamples[SampleIndex];
        }
    }
}