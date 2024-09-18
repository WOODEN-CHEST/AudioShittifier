using GHEngine.IO.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AudioShittifier.Layout;

public class JSONModifierLayoutParser : IModifierLayoutParser
{
    // Private static fields.
    private const string KEY_MODIFIER_NAME = "name";
    private const string KEY_RANGE_BEST = "best";
    private const string KEY_RANGE_WORST = "worst";


    // Private methods.
    private ModifierLayout DeconstructLayout(object? parsedJsonObject)
    {
        if (parsedJsonObject is JSONCompound Compound)
        {
            return new ModifierLayout(new ModifierDefinition[] { ParseSingleModifier(Compound) });
            
        }
        if (parsedJsonObject is JsonArray ModifierArray)
        {
            return new ModifierLayout(ParseModifierArray(ModifierArray));
        }
        throw new ModifierParseException("Expected compound as modifier root");
    }

    private ModifierDefinition[] ParseModifierArray(JsonArray array)
    {
        List<ModifierDefinition> Definitions = new();
        foreach (object? Item in array)
        {
            if (Item is not JSONCompound Compound)
            {
                throw new ModifierParseException("Got non-compound entry in modifier array");
            }

            Definitions.Add(ParseSingleModifier(Compound));
        }
        return Definitions.ToArray();
    }

    private ModifierDefinition ParseSingleModifier(JSONCompound compound)
    {
        Dictionary<string, IValueProvider> Values = new();
        string Name = compound.GetVerified<string>(KEY_MODIFIER_NAME);

        foreach (KeyValuePair<string, object?> Entry in compound)
        {
            if (Entry.Key == KEY_MODIFIER_NAME)
            {
                continue;
            }
            if (Entry.Value == null)
            {
                throw new ModifierParseException("Invalid value of entry (null)");
            }

            Values.Add(Entry.Key, GetValueProvider(Entry.Key, Entry.Value));
        }

        return new ModifierDefinition(Name, Values);
    }

    private IValueProvider GetValueProvider(string key, object value)
    {
        if (value is double or long or string)
        {
            return new ConstantValueProvider(value);
        }
        if (value is JSONCompound Compound)
        {
            return GetRangeProvider(Compound);
        }
        if (value is JsonArray SelectionArray)
        {
            return GetSelectionProvider(key, SelectionArray);
        }
        throw new ModifierParseException($"Invalid value for property \"{key}\"," +
            " expected constant value, range or selection array.");
    }

    private IValueProvider GetRangeProvider(JSONCompound compound)
    {
        double Best = compound.GetVerified<double>(KEY_RANGE_BEST);
        double Worst = compound.GetVerified<double>(KEY_RANGE_WORST);
        return new NumberRangeValueProvider(Best, Worst);
    }

    private IValueProvider GetSelectionProvider(string key, JsonArray array)
    {
        object?[] ObjArray = array.ToArray();
        if (ObjArray.Length == 0)
        {
            throw new ModifierParseException($"Empty selection array for property \"{key}\", expected values.");
        }
        if (ObjArray[0] == null)
        {
            throw new ModifierParseException($"Invalid selection (null) for property \"{key}\"");
        }
        Type TargetType = ObjArray[0]!.GetType();

        if (ObjArray.Where(element => element!.GetType() != TargetType).Any())
        {
            throw new ModifierParseException($"Mismatched element types in array for property \"{key}\"");
        }
        return new MultiSelectionValueProvider(ObjArray!);
    }


    // Inherited methods.
    public ModifierLayout GetLayout(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new ModifierParseException($"Layout at path \"{filePath}\" does not exist.");
        }

        try
        {
            return DeconstructLayout(new JSONDeserializer().Deserialize(File.ReadAllText(filePath)));
        }
        catch (JSONDeserializeException e)
        {
            throw new ModifierParseException($"Failed to parse modifier due to malformed JSON. {e.Message}");
        }
        catch (JSONEntryException e)
        {
            throw new ModifierParseException($"Failed to parse modifier due to an invalid layout. {e.Message}");
        }
    }
}