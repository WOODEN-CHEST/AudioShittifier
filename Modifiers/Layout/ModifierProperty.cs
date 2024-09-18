using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers.Layout;

public class  ModifierProperty
{
    // Fields.
    public string Name { get; }
    public double Cost { get; }


    // Private fields.
    private readonly object _value;


    // Constructors.
    public ModifierProperty(string name, double cost, object value)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _value = value;

        if (double.IsNaN(cost) || double.IsInfinity(cost) || (cost < 0d))
        {
            throw new ModifierLayoutException($"Invalid property \"{name}\" cost: {cost}");
        }
        Cost = cost;
    }


    // Methods.
    public bool TryGetValue<T>(out T? value)
    {
        if (_value is T)
        {
            value = (T)_value;
            return true;
        }

        value = default;
        return false;
    }
}