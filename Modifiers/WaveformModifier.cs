using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public class WaveformModifier : IAudioModifier
{
    // Fields.
    public double ChancePerSecond { get; set; }
    public TimeSpan MinDuration { get; set; }
    public TimeSpan MaxDuration { get; set; }
    public WaveformType WaveType { get; set; } = WaveformType.Sine;
    public float Volume { get; set; } = 0.5f;
    public float MinFrequency { get; set; } = 300f;
    public float MaxFrequency { get; set; } = 3000f;


    // Private methods.
    private void CreateWaveform(SampleBuffer buffer, int index, int count, float frequency)
    {
        for (int Index = index; (Index < buffer.LengthPerChannel) && (Index < index + count); Index++)
        {
            float Sample = WaveType switch
            {
                WaveformType.Sine => MathF.Sin((Index * MathF.PI) / buffer.Format.SampleRate * frequency),

                WaveformType.Square => MathF.Round((Index % (buffer.Format.SampleRate / frequency))
                    / (buffer.Format.SampleRate / frequency)) * 2f - 1f,

                WaveformType.Triangle => Math.Abs((Index % (buffer.Format.SampleRate / frequency))
                    / (buffer.Format.SampleRate / frequency) - 0.5f) * 4f - 1,

                WaveformType.Saw => (Index % (buffer.Format.SampleRate / frequency) / (buffer.Format.SampleRate / frequency)
                    - 0.5f) * 2f,

                _ => throw new NotSupportedException($"Waveform type \"{WaveType}\" ({(int)WaveType}) is not supported")
            };
            Sample *= Volume;

            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                buffer.SetSample(Index, ChannelIndex, Sample);
            }
        }
    }


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        for (int Index = 0; Index < buffer.LengthPerChannel; Index += buffer.Format.SampleRate)
        {
            if (Random.Shared.NextDouble() >= ChancePerSecond)
            {
                continue;
            }

            int SampleCount = (int)(MinDuration.TotalSeconds
                + ((MaxDuration - MinDuration).TotalSeconds * Random.Shared.NextDouble()));
            float Frequency = MinFrequency + ((MaxFrequency - MinFrequency) * Random.Shared.NextSingle());
            CreateWaveform(buffer, Index, SampleCount, Frequency);
            Index += SampleCount;
        }
    }
}