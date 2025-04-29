using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier.Modifiers.Layout;

public class ModifierLayout
{
    // Fields.
    public ModifierGroup[] Groups => _groups.Values.ToArray();
    public double MaxCost
    {
        get
        {
            double TotalCost = 0d;
            foreach (ModifierGroup Group in _groups.Values)
            {
                TotalCost += Group.EstimatedCost;
            }
            return TotalCost;
        }
    }


    // Private fields.
    private readonly Dictionary<string, ModifierGroup> _groups = new();



    // Constructors.
    public ModifierLayout(ModifierGroup[] groups)
    {
        if (groups == null)
        {
            throw new ArgumentNullException(nameof(groups));
        }

        foreach (ModifierGroup Group in groups)
        {
            _groups[Group.Name] = Group;
        }
    }


    // Methods.
    public ModifierGroup? GetGroup(string name)
    {
        _groups.TryGetValue(name, out ModifierGroup? Group);
        return Group;
    }
}