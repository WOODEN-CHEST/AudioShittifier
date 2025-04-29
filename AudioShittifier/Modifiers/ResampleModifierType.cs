using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

public enum ResampleModifierType
{
    Interpolate,
    Clamp,
    Legacy
}