using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("precision")]
public class PrecisionModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("steps")]
    public ulong StepCount
    {
        get => _stepCount;
        set => _stepCount = Math.Max(value, STEP_COUNT_MIN);
    }

    // Private static fields.
    private const ulong STEP_COUNT_DEFAULT = 4_294_967_296uL;
    private const ulong STEP_COUNT_MIN = 1uL;


    // Private fields.
    private ulong _stepCount = STEP_COUNT_DEFAULT;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        for (int i = 0; i < buffer.Samples.Length; i++)
        {
            float ClampedSample = (float)(Math.Round((double)buffer.Samples[i] * StepCount) / (double)StepCount);
            buffer.SetSample(i, ClampedSample);
        }
    }
}