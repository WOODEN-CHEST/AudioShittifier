using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers.Layout;

public class ModifierDefinition
{
    // Fields.
    public string Name { get; }
    public bool IsMandatory { get; }
    public double Weight { get; }
    public double TotalCost => _properties.Values.Select(property => property.Cost).Sum();


    // Private fields.
    private readonly Dictionary<string, ModifierProperty> _properties = new();


    // Constructors.
    public ModifierDefinition(string name, ModifierProperty[] properties)
        : this(name, 0, true, properties) { }

    public ModifierDefinition(string name, int weight, ModifierProperty[] properties) 
        : this(name, weight, false, properties) { }

    private ModifierDefinition(string name, int weight, bool isMandatory, ModifierProperty[] properties)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        IsMandatory = isMandatory;
        Weight = Math.Max(0, weight);
        if (properties == null)
        {
            throw new ArgumentNullException(nameof(properties));
        }
        foreach (ModifierProperty Property in properties)
        {
            _properties[Property.Name] = Property;
        }
    }


    // Methods.,
    public ModifierProperty? TryGetProperty(string name)
    {
        _properties.TryGetValue(name, out ModifierProperty? Property);
        return Property;
    }
}