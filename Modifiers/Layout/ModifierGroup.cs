using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers.Layout;

public class ModifierGroup : IEnumerable<ModifierDefinition>
{
    // Fields.
    public string Name { get; }
    public double EstimatedCost
    {
        get
        {
            double HighestModifierCost = 
                _modifiers.Where(modifier => !modifier.IsMandatory).Select(modifier => modifier.TotalCost).Max();
            double CostFromMandatoryModifiers = 
                _modifiers.Where(modifier => modifier.IsMandatory).Select(modifier => modifier.TotalCost).Sum();
            return HighestModifierCost + CostFromMandatoryModifiers;
        }
    }


    // Private fields.
    private ModifierDefinition[] _modifiers;


    // Constructors.
    public ModifierGroup(string name, ModifierDefinition[] modifiers)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _modifiers = modifiers?.ToArray() ?? throw new ArgumentNullException(nameof(modifiers));
    }

    public IEnumerator<ModifierDefinition> GetEnumerator()
    {
        foreach (ModifierDefinition Modifier in _modifiers)
        {
            yield return Modifier;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}