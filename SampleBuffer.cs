using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public class SampleBuffer
{
    // Fields.
    public float[] Samples { get; private init; }
    public WaveFormat Format { get; private init; }
    public int LengthPerChannel => Samples.Length / Format.Channels;


    // Constructors.
    public SampleBuffer(float[] samples, WaveFormat format)
    {
        Samples = samples ?? throw new ArgumentNullException(nameof(samples));
        Format = format ?? throw new ArgumentNullException(nameof(format));

        if (Samples.Length % format.Channels != 0)
        {
            throw new ArgumentException("Sample length must be multiple of format channel count", nameof(samples));
        }
    }


    // Methods.
    public float[] GetCopyOfSamples()
    {
        return Samples.ToArray();
    }

    public float GetSample(int index, int channelIndex)
    {
        if ((index < 0) || (index >= LengthPerChannel))
        {
            return 0.0f;
        }
        return Samples[index * Format.Channels + channelIndex];
    }

    public float GetLerpedSample(double sampleIndex, int channelIndex)
    {
        double LowerIndex = Math.Floor(sampleIndex);
        double UpperIndex = Math.Ceiling(sampleIndex);
        float LowerSample = GetSample((int)LowerIndex, channelIndex);
        float UpperSample = GetSample((int)UpperIndex, channelIndex);
        float InterpolationAmount = (float)(sampleIndex - LowerIndex);
        return LowerSample + ((UpperSample - LowerSample) * InterpolationAmount);
    }

    public void SetSample(int sampleIndex, int channelIndex, float value)
    {
        if ((sampleIndex < 0) || (sampleIndex >= LengthPerChannel))
        {
            return;
        }

        Samples[sampleIndex * Format.Channels + channelIndex] = Math.Clamp(value, -1f, 1f);
    }

    public void SetSample(int index, float value)
    {
        if ((index < 0) || (index >= Samples.Length))
        {
            return;
        }

        Samples[index] = Math.Clamp(value, -1f, 1f);
    }
}