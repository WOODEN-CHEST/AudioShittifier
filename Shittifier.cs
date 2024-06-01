using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioShittifier.Modifiers;
using NAudio.Wave;

namespace AudioShittifier;

public class Shittifier
{
    // Private fields.
    private readonly IAudioModifier[] _modifers;


    // Constructors.
    public Shittifier(IAudioModifier[] modifiers)
    {
        _modifers = modifiers ?? throw new ArgumentNullException(nameof(modifiers));
    }


    // Methods.
    public void Shittify(float[] samples, WaveFormat audioFormat)
    {
        foreach (IAudioModifier Modifier in _modifers)
        {
            Modifier.Modify(samples, audioFormat);
        }
    }
}