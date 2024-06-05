using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class SampleMultiplier : IAudioModifier
{
    // Fields.
    public float Multiplier { get; set; } = 1f;
    public float ClippingThreshold { get; set; } = 4f;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        for (int i = 0; i < buffer.Samples.Length; i++)
        {
            float ModifiedSample = Math.Abs(buffer.Samples[i] * Multiplier) > ClippingThreshold ? 0f
                : Math.Clamp(buffer.Samples[i] * Multiplier, -1f, 1f);
            buffer.Samples[i] = ModifiedSample;
        }
    }
}