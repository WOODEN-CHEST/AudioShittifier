using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class EchoModifier : IAudioModifier
{
    // Fields.
    public TimeSpan Offset { get; set; }



    // Inherited methods.
    public void Modify(float[] samples, WaveFormat audioFormat)
    {
        float[] SampleCopies = new float[samples.Length];
        Buffer.BlockCopy(samples, 0, SampleCopies, 0, samples.Length * sizeof(float));

        int SampleOffset = (int)(Offset.TotalSeconds * audioFormat.SampleRate * audioFormat.Channels);
        for (int i = Math.Max(0, -SampleOffset); (i < samples.Length) && (i + SampleOffset) < samples.Length; i++)
        {
            samples[i] = Math.Clamp(samples[i] + SampleCopies[i + SampleOffset], -1f, 1f);
        }
    }
}