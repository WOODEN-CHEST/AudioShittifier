using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers.Layout;

public class ConstantModifierProvider : IModifierProvider
{
    // Private fields.
    private readonly IAudioModifier[] _modifiers;


    // Constructors.
    public ConstantModifierProvider(params IAudioModifier[] modifiers)
    {
        _modifiers = modifiers ?? throw new ArgumentNullException(nameof(modifiers));
    }


    // Inherited methods.
    public IAudioModifier[] GetModifiers(double intensity)
    {
        return _modifiers.ToArray();
    }
}