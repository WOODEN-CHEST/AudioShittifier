using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

/* This doesn't event properly re-sample anything, but the result sounds so bad that I love it. */
[AudioModifier("resample")]
public class ResampleModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("rate")]
    public int SampleRate
    {
        get => _sampleRate;
        set => _sampleRate = Math.Max(value, 1);
    }

    [AudioModifierProperty("type")]
    public ResampleModifierType ResampleType { get; set; } = ResampleModifierType.Interpolate;



    // Private fields.
    private int _sampleRate = 44100;


    // Private methods.
    private void LegacyResample(SampleBuffer buffer)
    {
        SampleBuffer BufferCopy = new(buffer.GetCopyOfSamples(), buffer.Format);
        float SampleRatio = (float)Math.Min(buffer.Format.SampleRate, SampleRate) / buffer.Format.SampleRate;

        for (int i = 0; i < buffer.Samples.Length; i++)
        {
            int SampleIndex = Math.Clamp((int)(MathF.Floor(i * SampleRatio) / SampleRatio), 0, buffer.Samples.Length - 1);
            buffer.Samples[i] = BufferCopy.Samples[SampleIndex];
        }
    }

    private void Resample(SampleBuffer buffer)
    {
        SampleBuffer BufferCopy = new(buffer.GetCopyOfSamples(), buffer.Format);
        double Precision = Math.Max(1d, (double)buffer.Format.SampleRate / SampleRate);

        for (int Index = 0; Index < buffer.LengthPerChannel; Index++)
        {
            double LowerIndex = Math.Floor(Index / Precision) * Precision;
            double UpperIndex = Math.Floor((Index / Precision) + 1f) * Precision;
            float InterpolationAmount = (float)((Index - LowerIndex) / (UpperIndex - LowerIndex));
            if (ResampleType == ResampleModifierType.Clamp)
            {
                InterpolationAmount = MathF.Round(InterpolationAmount);
            }
            
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                float LowerSample = BufferCopy.GetLerpedSample(LowerIndex, ChannelIndex);
                float UpperSample = BufferCopy.GetLerpedSample(UpperIndex, ChannelIndex);
                buffer.SetSample(Index, ChannelIndex, LowerSample + ((UpperSample - LowerSample) * InterpolationAmount));
            }
        }
    }

    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        if (SampleRate >= buffer.Format.SampleRate)
        {
            return;
        }

        switch (ResampleType)
        {
            case ResampleModifierType.Interpolate:
            case ResampleModifierType.Clamp:
                Resample(buffer);
                break;

            case ResampleModifierType.Legacy:
                LegacyResample(buffer);
                break;

            default:
                throw new NotSupportedException($"Resample not supported for resample type {ResampleType} ({(int)ResampleType})");
        }
    }
}