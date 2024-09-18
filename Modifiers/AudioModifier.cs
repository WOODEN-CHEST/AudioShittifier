using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AudioModifier : Attribute
{
    // Fields.
    public string ModifierName { get; private init; }


    // Constructors.
    public AudioModifier(string modifierName)
    {
        ModifierName = modifierName ?? throw new ArgumentNullException(nameof(modifierName));
    }
}