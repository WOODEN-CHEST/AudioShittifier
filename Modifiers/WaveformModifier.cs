using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("waveform")]
public class WaveformModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("per_second")]
    public double WaveformsPerSecond { get; set; }

    [AudioModifierProperty("duration_min")]
    public TimeSpan MinDuration { get; set; }

    [AudioModifierProperty("duration_max")]
    public TimeSpan MaxDuration { get; set; }

    [AudioModifierProperty("type")]
    public WaveformType WaveType { get; set; } = WaveformType.Sine;

    [AudioModifierProperty("volume")]
    public float Volume { get; set; } = 0.5f;

    [AudioModifierProperty("frequency_min")]
    public float MinFrequency { get; set; } = 300f;

    [AudioModifierProperty("frequency_max")]
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
        int WaveformCount = (int)(buffer.LengthPerChannel / (double)buffer.Format.SampleRate * WaveformsPerSecond);
        for (int i = 0; i < WaveformCount; i++)
        {
            int SourceIndex = Random.Shared.Next(buffer.LengthPerChannel);

            int SampleCount = (int)(MinDuration.TotalSeconds + ((MaxDuration - MinDuration).TotalSeconds 
                * Random.Shared.NextDouble()) * buffer.Format.SampleRate);
            float Frequency = MinFrequency + ((MaxFrequency - MinFrequency) * Random.Shared.NextSingle());
            CreateWaveform(buffer, SourceIndex, SampleCount, Frequency);
        }
    }
}