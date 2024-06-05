using AudioShittifier.Modifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public interface IModifierProvider
{
    IAudioModifier[] GetRandomModifiers();
}