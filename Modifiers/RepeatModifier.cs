using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class RepeatModifier : IAudioModifier
{
    // Fields.
    public double RepeatChancePerSecond { get; set; }
    public TimeSpan AverageRepeatSectionDuration { get; set; }
    public int AverageRepeatCount { get; set; }


    // Private methods.
    private void CopySamples(float[] samples, int sourceIndex, int destinationIndex, int count)
    {
        int Offset = destinationIndex - sourceIndex;
        if (destinationIndex - Offset < 0)
        {
            throw new ArgumentException("destination index and source index create offset which is out of bounds.",
                nameof(destinationIndex));
        }
        for (int i = destinationIndex; (i < destinationIndex + count) && (i < samples.Length); i++)
        {
            samples[i] = samples[i - Offset];
        }
    }


    // Inherited methods.
    public void Modify(float[] samples, WaveFormat audioFormat)
    {
        for (int Index = 0; Index < samples.Length; Index += audioFormat.SampleRate * audioFormat.Channels)
        {
            if (Random.Shared.NextDouble() >= RepeatChancePerSecond)
            {
                continue;
            }
            
            int RepeatCount = (int)ShittifierRandom.RandomOffsetValue(AverageRepeatCount);
            int RepeatedSamples = (int)(ShittifierRandom.RandomOffsetValue(AverageRepeatSectionDuration.TotalSeconds)
                * audioFormat.SampleRate * audioFormat.Channels);
            int SourceIndex = Index;
            Index += RepeatedSamples;
            for (int RepeatIndex = 0; RepeatIndex < RepeatCount; RepeatIndex++)
            {
                CopySamples(samples, SourceIndex, Index, RepeatedSamples);
                Index += RepeatedSamples;
            }
        }
    }
}