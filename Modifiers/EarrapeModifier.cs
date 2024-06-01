using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class EarrapeModifier : IAudioModifier
{
    // Fields.
    public float Multiplier { get; set; } = 1f;
    public float ClippingThreshold { get; set; } = 4f;


    // Inherited methods.
    public void Modify(float[] samples, WaveFormat audioFormat)
    {
        for (int i = 0; i < samples.Length; i++)
        {
            float ModifiedSample = Math.Abs(samples[i] * Multiplier) > ClippingThreshold ? 0f
                : Math.Clamp(samples[i] * Multiplier, -1f, 1f);
            samples[i] = ModifiedSample;
        }
    }
}