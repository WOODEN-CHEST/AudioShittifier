using AudioShittifier.Audio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class PrecisionModifier : IAudioModifier
{
    // Fields.
    public ulong StepCount
    {
        get => _stepCount;
        set => _stepCount = Math.Max(value, 1);
    }


    // Private fields.
    private ulong _stepCount = (ulong)Math.Pow(2, 32);


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        for (int i = 0; i < buffer.Samples.Length; i++)
        {
            float ClampedSample = (MathF.Round((buffer.Samples[i] + 1f) * StepCount) / StepCount) - 1f;
            buffer.SetSample(i, ClampedSample);
        }
    }
}