using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class AudioModifierProperty : Attribute
{
    // Fields.
    public string PropertyName { get; private init; }


    // Constructors.
    public AudioModifierProperty(string propertyName)
    {
        PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
    }
}