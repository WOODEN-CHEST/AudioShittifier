using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("power")]
public class PowerModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("amount")]
    public float Power
    {
        get => _power;
        set => _power = float.IsNaN(value) ? POWER_DEFAULT
            : Math.Clamp(value, POWER_MIN, POWER_MAX);
    }

    // Private static fields.
    private const float POWER_DEFAULT = 1f;
    private const float POWER_MAX = 100_000f;
    private const float POWER_MIN = float.Epsilon;


    // Private fields.
    private float _power = POWER_DEFAULT;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        for (int i = 0; i < buffer.Samples.Length; i++)
        {
            float Sample = buffer.Samples[i];
            buffer.Samples[i] = MathF.Pow(Math.Abs(Sample), Power) * Math.Sign(Sample);
        }
    }
}