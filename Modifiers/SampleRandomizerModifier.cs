using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class SampleRandomizerModifier : IAudioModifier
{
    // Fields.
    public double PortionOfSamplesRandomized { get; set; } = 0.0001d;


    // Inherited methods.
    public void Modify(float[] samples, WaveFormat audioFormat)
    {
        int SamplesRandomized = (int)(samples.Length * PortionOfSamplesRandomized);

        for (int i = 0; i < SamplesRandomized; i++)
        {
            int Index1 = Random.Shared.Next(0, samples.Length);
            int Index2 = Random.Shared.Next(0, samples.Length);

            (samples[Index1], samples[Index2]) = (samples[Index2], samples[Index1]);
        }
    }
}