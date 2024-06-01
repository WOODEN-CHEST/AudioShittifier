using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class BitDepthModifier : IAudioModifier
{
    // Fields.
    public int BitDepth
    {
        get => _bitDepth;
        set => _bitDepth = Math.Clamp(value, 1, 32);
    }


    // Private fields.
    private int _bitDepth = 32;


    // Inherited methods.
    public void Modify(float[] samples, WaveFormat audioFormat)
    {
        ulong MaxValue = (ulong)Math.Pow(2, BitDepth - 1) - 1;
        for (int i = 0; i < samples.Length; i++)
        {
            long IntSample = (long)(samples[i] * MaxValue);
            float ClampedSample = (float)IntSample / MaxValue;
            samples[i] = ClampedSample;
        }
    }
}