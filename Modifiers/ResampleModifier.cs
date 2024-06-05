using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class ResampleModifier : IAudioModifier
{
    // Fields.
    public int SampleRate
    {
        get => _sampleRate;
        set => _sampleRate = Math.Max(value, 1);
    }

    public ResampleModifierType Type { get; set; } = ResampleModifierType.Interpolate;



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

    private void InterpolateResample(SampleBuffer buffer)
    {
        SampleBuffer BufferCopy = new(buffer.GetCopyOfSamples(), buffer.Format);
        double Precision = Math.Max(1d, (double)buffer.Format.SampleRate / SampleRate);

        for (int Index = 0; Index < buffer.LengthPerChannel; Index++)
        {
            double LowerIndex = Math.Floor(Index / Precision) * Precision;
            double UpperIndex = Math.Floor((Index / Precision) + 1f) * Precision;
            float InterpolationAmount = (float)((Index - LowerIndex) / (UpperIndex - LowerIndex));
            
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                float LowerSample = BufferCopy.GetLerpedSample(LowerIndex, ChannelIndex);
                float UpperSample = BufferCopy.GetLerpedSample(LowerIndex, ChannelIndex);
                buffer.SetSample(Index, ChannelIndex, LowerSample + ((UpperSample - LowerSample) * InterpolationAmount));
            }
        }
    }

    private void ClampResample(SampleBuffer buffer)
    {

    }


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        if (SampleRate >= buffer.Format.SampleRate)
        {
            return;
        }

        switch (Type)
        {
            case ResampleModifierType.Interpolate:
                InterpolateResample(buffer);
                break;

            case ResampleModifierType.Clamp:
                ClampResample(buffer);
                break;

            case ResampleModifierType.Old:
                LegacyResample(buffer);
                break;

            default:
                throw new NotSupportedException($"Resample not supported for resample type {Type} ({(int)Type})");
        }
    }
}