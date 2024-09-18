using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Layout;

public class MultiSelectionValueProvider : IValueProvider
{
    // Private fields.
    private object[] _choices;


    // Constructors.
    public MultiSelectionValueProvider(object[] choices)
    {
        _choices = choices ?? throw new ArgumentNullException(nameof(choices));
    }


    // Inherited methods.
    public object GetValue(double intensity)
    {
        return _choices[Random.Shared.Next(_choices.Length)];
    }
}