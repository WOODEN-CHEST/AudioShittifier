using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers.Layout;

public interface IModifierProvider
{
    IAudioModifier[] GetModifiers(double intensity);
}