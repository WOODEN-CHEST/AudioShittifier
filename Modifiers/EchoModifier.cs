using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AudioModifier("echo")]
public class EchoModifier : IAudioModifier
{
    // Fields.
    [AudioModifierProperty("offset")]
    public TimeSpan Offset { get; set; }

    [AudioModifierProperty("volume")]
    public float EchoVolume
    {
        get => _echoVolume;
        set => _echoVolume = float.IsNaN(value) ? ECHO_VOLUME_DEFAULT
            : Math.Clamp(value, ECHO_VOLUME_MIN, ECHO_VOLUME_MAX);
    }


    // Private static fields.
    private const float ECHO_VOLUME_DEFAULT = 1f;
    private const float ECHO_VOLUME_MAX = 10_000f;
    private const float ECHO_VOLUME_MIN = 0f;


    // Private fields.
    private float _echoVolume = ECHO_VOLUME_DEFAULT;


    // Inherited methods.
    public void Modify(SampleBuffer buffer)
    {
        SampleBuffer CopyBuffer = new(buffer.GetCopyOfSamples(), buffer.Format);

        int SampleOffset = (int)(Offset.TotalSeconds * buffer.Format.SampleRate);
        for (int i = Math.Max(0, SampleOffset); Math.Max(i, i - SampleOffset) < buffer.LengthPerChannel; i++)
        {
            for (int ChannelIndex = 0; ChannelIndex < buffer.Format.Channels; ChannelIndex++)
            {
                buffer.SetSample(i, ChannelIndex, CopyBuffer.GetSample(i, ChannelIndex) 
                    + CopyBuffer.GetSample(i - SampleOffset, ChannelIndex) * EchoVolume);
            }
        }
    }
}