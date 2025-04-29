using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public class SampleToWaveConverter : IWaveProvider
{
    // Fields.
    public WaveFormat WaveFormat { get; private init; }


    // Private fields.
    private float[] _samples;
    private int _sampleIndex = 0;


    // Constructors.
    public SampleToWaveConverter(WaveFormat waveFormat, float[] samples)
    {
        _samples = samples ?? throw new ArgumentNullException(nameof(samples));
        WaveFormat = waveFormat ?? throw new ArgumentNullException(nameof(waveFormat));
    }


    // Private methods.
    private int Read8BitAudio(byte[] buffer, int offset, int count)
    {
        int Index;
        for (Index = offset; (Index < count + offset) && (_sampleIndex < _samples.Length); Index++, _sampleIndex++)
        {
            buffer[Index] = (byte)(_samples[_sampleIndex] * byte.MaxValue);
        }
        return Index - offset;
    }

    private int Read16BitAudio(byte[] buffer, int offset, int count)
    {
        int Index;
        for (Index = offset; (Index < count + offset) && (_sampleIndex < _samples.Length); Index += 2, _sampleIndex++)
        {
            ushort Value = (ushort)(_samples[_sampleIndex] * ushort.MaxValue);
            buffer[Index] = (byte)(Value & 0xff);
            buffer[Index + 1] = (byte)(Value >> 8);
        }
        return Index - offset;
    }

    private int Read24BitAudio(byte[] buffer, int offset, int count)
    {
        int Index;
        int MaxValue = (int)Math.Pow(2, 24) - 1;
        for (Index = offset; (Index < count + offset) && (_sampleIndex < _samples.Length); Index += 3, _sampleIndex++)
        {
            int Value = (int)(_samples[_sampleIndex] * MaxValue);
            buffer[Index] = (byte)(Value & 0xff);
            buffer[Index + 1] = (byte)((Value >> 8) & 0xff);
            buffer[Index + 2] = (byte)(Value >> 16);
        }
        return Index - offset;
    }

    private int Read32BitAudio(byte[] buffer, int offset, int count)
    {
        int Index;
        for (Index = offset; (Index < count + offset) && (_sampleIndex < _samples.Length); Index += 4, _sampleIndex++)
        {
            Buffer.BlockCopy(BitConverter.GetBytes(_samples[_sampleIndex]), 0, buffer, Index, sizeof(float));
        }
        return Index - offset;
    }


    // Methods.
    public int Read(byte[] buffer, int offset, int count)
    {
        return (WaveFormat.BitsPerSample) switch
        {
            8 => Read8BitAudio(buffer, offset, count),
            16 => Read16BitAudio(buffer, offset, count),
            24 => Read24BitAudio(buffer, offset, count),
            32 => Read32BitAudio(buffer, offset, count),
            _ => throw new NotSupportedException($"{WaveFormat.BitsPerSample} bits per sample are not supported.")
        };
    }
}