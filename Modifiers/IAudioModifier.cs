using NAudio.Wave;


namespace AudioShittifier.Modifiers;


public interface IAudioModifier
{
    void Modify(SampleBuffer buffer);
}