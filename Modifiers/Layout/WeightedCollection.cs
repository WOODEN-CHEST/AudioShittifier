using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers.Layout;

public class WeightedCollection<T> : IEnumerable<T?>
{
    // Private fields.
    private readonly RangedEntry<T>[] _entries;

    // Constructors.
    public WeightedCollection(T[] values, double[] weights)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }
        if (weights == null)
        {
            throw new ArgumentNullException(nameof(weights));
        }
        if (values.Length != weights.Length)
        {
            throw new ArgumentException("length of value array not the same as length of weight array", nameof(values));
        }

        if (values.Length == 0)
        {
            _entries = Array.Empty<RangedEntry<T>>();
        }
        else
        {
            _entries = new RangedEntry<T>[values.Length];
            double CurrentRange = 0d;
            for (int i = 0; i < values.Length; i++)
            {
                if (double.IsInfinity(weights[i]) || double.IsNaN(weights[i]) || (weights[i] < 0d))
                {
                    throw new ArgumentException("Weight array contains invalid weight value.");
                }
                _entries[i] = new RangedEntry<T>(values[i], CurrentRange, CurrentRange + weights[i]);
                CurrentRange += weights[i];
            }
        }
    }


    // Methods.
    public T? GetRandom()
    {
        if (_entries.Length == 0)
        {
            return default;
        }

        double Point = _entries[^1].RangeMax * Random.Shared.NextDouble();
        RangedEntry<T>? Entry = GetEntry(Point);
        if (Entry == null)
        {
            return _entries[0].Value;
        }
        return Entry.Value;
    }


    // Private methods.
    private RangedEntry<T>? GetEntry(double pointInRange)
    {
        int Low = 0;
        int High = _entries.Length - 1;
        
        while (Low <= High)
        {
            int Middle = Low + ((High - Low) / 2);
            if ((_entries[Middle].RangeMin <= pointInRange) && (pointInRange <= _entries[Middle].RangeMax))
            {
                return _entries[Middle];
            }

            if (pointInRange < _entries[Middle].RangeMin)
            {
                High = Middle - 1;
            }
            else
            {
                Low = Middle + 1;
            }
        }

        return null;
    }


    // Inherited methods.
    public IEnumerator<T?> GetEnumerator()
    {
        foreach (RangedEntry<T> Entry in _entries)
        {
            yield return Entry.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }


    // Types.
    private class RangedEntry<V>
    {
        // Fields.
        public double RangeMin { get; }
        public double RangeMax { get; }
        public V Value { get; }


        // Constructors.
        public RangedEntry(V value, double rangeMin, double rangeMax)
        {
            RangeMin = rangeMin;
            RangeMax = rangeMax;
            Value = value;
        }
    }
}