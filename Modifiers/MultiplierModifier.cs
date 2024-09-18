using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("multiplier")]
public class MultiplierModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("amount")]
    public float Multiplier { get; set; } = 1f;


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