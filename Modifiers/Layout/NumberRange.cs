using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers.Layout;

public class NumberRange
{
    // Fields.
    public double BestValue { get; }
    public double WorstValue { get; }


    // Constructors.
    public NumberRange(double bestValue, double worstValue)
    {
        BestValue = bestValue;
        WorstValue = worstValue;
    }
}