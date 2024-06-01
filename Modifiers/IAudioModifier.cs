using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public interface IAudioModifier
{
    void Modify(float[] samples, WaveFormat audioFormat);
}