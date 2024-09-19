using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("multiplier")]
public class MultiplierModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("amount")]
    public float Multiplier
    {
        get => _multiplier;
        set => _multiplier = float.IsNaN(value) ? MULTIPLIER_DEFAULT
            : Math.Clamp(value, MULTIPLIER_MIN, MULTIPLIER_MAX);
    }

    // Private static fields.
    private const float MULTIPLIER_DEFAULT = 1f;
    private const float MULTIPLIER_MIN = 0f;
    private const float MULTIPLIER_MAX = 10_000f;


    // Private fields.
    private float _multiplier = MULTIPLIER_DEFAULT;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        for (int i = 0; i < buffer.Samples.Length; i++)
        {
            float Sample = buffer.Samples[i] * Multiplier;
            buffer.SetSample(i, Sample);
        }
    }
}