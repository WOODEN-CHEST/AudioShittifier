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
    public double WaveformsPerSecond
    {
        get => _waveformsPerSecond;
        set => _waveformsPerSecond = double.IsNaN(value) ? WAVEFORMS_PER_SECOND_DEFAULT
            : Math.Clamp(value, WAVEFORMS_PER_SECOND_MIN, WAVEFORMS_PER_SECOND_MAX);
    }

    [AudioModifierProperty("duration_min")]
    public TimeSpan MinDuration { get; set; }

    [AudioModifierProperty("duration_max")]
    public TimeSpan MaxDuration { get; set; }

    [AudioModifierProperty("type")]
    public WaveformType WaveType { get; set; } = WaveformType.Sine;

    [AudioModifierProperty("volume")]
    public float Volume
    {
        get => _volume;
        set
        {
            _volume = float.IsNaN(value) ? VOLUME_DEFAULT : Math.Clamp(value, VOLUME_MIN, VOLUME_MAX);
        }
    }

    [AudioModifierProperty("frequency_min")]
    public float MinFrequency
    {
        get => _frequencyMin;
        set
        {
            _frequencyMin = float.IsNaN(value) ? FREQUENCY_DEFAULT_MIN 
                : Math.Clamp(value, FREQUENCY_MIN, FREQUENCY_MAX);
            _frequencyMax = Math.Max(_frequencyMin, _frequencyMax);
        }
    }

    [AudioModifierProperty("frequency_max")]
    public float MaxFrequency
    {
        get => _frequencyMax;
        set
        {
            _frequencyMax = float.IsNaN(value) ? FREQUENCY_DEFAULT_MAX
                : Math.Clamp(value, FREQUENCY_MIN, FREQUENCY_MAX);
            _frequencyMin = Math.Min(_frequencyMax, _frequencyMin);
        }
    }


    // Private static fields.
    private const double WAVEFORMS_PER_SECOND_DEFAULT = 0d;
    private const double WAVEFORMS_PER_SECOND_MAX = 10_000d;
    private const double WAVEFORMS_PER_SECOND_MIN = 0d;
    private const float VOLUME_DEFAULT = 0.5f;
    private const float VOLUME_MAX = 10_000f;
    private const float VOLUME_MIN = 0f;
    private const float FREQUENCY_MIN = 1f;
    private const float FREQUENCY_MAX = 20_000f;
    private const float FREQUENCY_DEFAULT_MIN = 300f;
    private const float FREQUENCY_DEFAULT_MAX = 3000f;


    // Private fields.
    private double _waveformsPerSecond = WAVEFORMS_PER_SECOND_DEFAULT;
    private float _frequencyMin = FREQUENCY_DEFAULT_MIN;
    private float _frequencyMax = FREQUENCY_DEFAULT_MAX;
    private float _volume = VOLUME_DEFAULT;

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