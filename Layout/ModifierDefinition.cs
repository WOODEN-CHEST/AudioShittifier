using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Layout;

public class ModifierDefinition
{
    // Fields.
    public string Name { get; private init; }


    // Private fields.
    private readonly Dictionary<string, IValueProvider> _values = new();

    // Constructors.
    public ModifierDefinition(string name, IDictionary<string, IValueProvider> values)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ArgumentNullException.ThrowIfNull(values, nameof(values));

        foreach (KeyValuePair<string, IValueProvider> Entry in values)
        {
            _values[Entry.Key] = Entry.Value;
        }
    }


    // Methods.
    public bool ContainsValue(string name)
    {
        return _values.ContainsKey(name ?? throw new ArgumentNullException(nameof(name)));
    }

    public object? GetValue(string name, double intensity)
    {
        ArgumentNullException.ThrowIfNull(name, nameof(name));
        if (_values.TryGetValue(name, out IValueProvider? ValueProvider))
        {
            return ValueProvider.GetValue(intensity);
        }
        return null;
    }
}