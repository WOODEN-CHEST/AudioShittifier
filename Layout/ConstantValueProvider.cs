using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Layout;

public class ConstantValueProvider : IValueProvider
{
    // Private fields.
    private object _value;


    // Constructors.
    public ConstantValueProvider(object value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
    }


    // Inherited methods.
    public object GetValue(double intensity)
    {
        return _value;
    }
}