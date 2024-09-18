using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Layout;

public class NumberRangeValueProvider : IValueProvider
{
    // Private fields.
    private double _bestValue;
    private double _worstValue;


    // Constructors.
    public NumberRangeValueProvider(double bestValue, double worstValue)
    {
        _bestValue = bestValue;
        _worstValue = worstValue;
    }


    // Inherited methods.
    public object GetValue(double intensity)
    {
        return _bestValue + ((_worstValue - _bestValue) * intensity);
    }
}