using AudioShittifier.Modifiers;
using System.Reflection;


namespace AudioShittifier.Layout;


public class ModifierBuilder
{
    // Methods.
    public IAudioModifier GetModifier(ModifierDefinition definition, double intensity)
    {
        object Modifier = GetModifierFromTypeName(definition.Name);
        ApplyPropertiesFromDefinition(Modifier, definition, intensity);
        return (IAudioModifier)Modifier;
    }


    // Private methods.
    private IEnumerable<Type> GetModifierTypes()
    {
        return Assembly.GetAssembly(typeof(IAudioModifier))!.GetTypes()
            .Where(type => (type.GetInterface(typeof(IAudioModifier).Name) != null)
                && (Attribute.GetCustomAttribute(type, typeof(AudioModifier)) != null));
    }

    private IEnumerable<PropertyInfo> GetModifiableProperties(Type type)
    {
        return type.GetProperties()
            .Where(property => property.CanWrite
            && (Attribute.GetCustomAttribute(property, typeof(AudioModifierProperty)) != null));
    }

    private object GetModifierFromTypeName(string typeName)
    {
        Type ModifierType;
        try
        {
            ModifierType = GetModifierTypes().Where(type => ((AudioModifier)Attribute.GetCustomAttribute(
                type, typeof(AudioModifier))!).ModifierName == typeName).First();
        }
        catch (InvalidOperationException)
        {
            throw new ModifierBuildException($"Invalid modifier type: {typeName}");
        }

        ConstructorInfo? Constructor = ModifierType.GetConstructor(Array.Empty<Type>());
        if (Constructor == null)
        {
            throw new ModifierBuildException($"Couldn't create modifier \"{typeName}\", no suitable constructor found.");
        }

        return Constructor.Invoke(Array.Empty<object>());
    }

    private object CastToEnum(object value, Type targetType, string propertyName)
    {
        if (value is string StringValue)
        {
            if (Enum.TryParse(targetType, StringValue, false, out object? Result))
            {
                return Result;
            }
            throw new ModifierBuildException($"Invalid value \"{StringValue}\" for property \"{propertyName}\"");
        }
        throw new ModifierBuildException($"Expected string value for modifier \"{propertyName}\"");
    }

    private object CastToTimespan(object value, string propertyName)
    {
        try
        {
            return TimeSpan.FromSeconds(Convert.ToDouble(value));
        }
        catch (Exception e)
        {
            throw new ModifierBuildException($"Expected timespan seconds for property \"{propertyName}\"");
        }
    }

    private object? CastToNumber(object value, Type targetType, string propertyName)
    {
        if (targetType.Equals(typeof(byte)))
        {
            return Convert.ToByte(value);
        }
        if (targetType.Equals(typeof(sbyte)))
        {
            return Convert.ToSByte(value);
        }
        if (targetType.Equals(typeof(short)))
        {
            return Convert.ToInt16(value);
        }
        if (targetType.Equals(typeof(ushort)))
        {
            return Convert.ToUInt16(value);
        }
        if (targetType.Equals(typeof(int)))
        {
            return Convert.ToInt32(value);
        }
        if (targetType.Equals(typeof(uint)))
        {
            return Convert.ToUInt32(value);
        }
        if (targetType.Equals(typeof(long)))
        {
            return Convert.ToInt64(value);
        }
        if (targetType.Equals(typeof(ulong)))
        {
            return Convert.ToUInt64(value);
        }
        if (targetType.Equals(typeof(float)))
        {
            return Convert.ToSingle(value);
        }
        if (targetType.Equals(typeof(double)))
        {
            return value;
        }
        return null;
    }

    private object TryConvertValue(object value, Type targetType, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        if (targetType.IsEnum)
        {
            return CastToEnum(value, targetType, propertyName);
        }
        else if (targetType.Equals(typeof(TimeSpan)))
        {
            return CastToTimespan(value, propertyName);
        }
        else if (((value is string) && targetType.Equals(typeof(string)))
            || (targetType.Equals(typeof(bool)) && (value is bool)))
        {
            return value;
        }
        else if (value is byte or sbyte or short or ushort or int or uint or long or ulong or float or double)
        {
            object? CastedObject = CastToNumber(value, targetType, propertyName);
            if (CastedObject != null)
            {
                return CastedObject;
            }
        }
        throw new ModifierBuildException($"Invalid type for modifier property \"{propertyName}\"." +
            $"Expected {targetType.Name}, got {value.GetType().Name}");
    }

    private void ApplyPropertiesFromDefinition(object modifier, ModifierDefinition definition, double intensity)
    {
        foreach (PropertyInfo Property in GetModifiableProperties(modifier.GetType()))
        {
            AudioModifierProperty TargetAttribute = (AudioModifierProperty)Property.GetCustomAttribute(
                typeof(AudioModifierProperty))!;

            if (!definition.ContainsValue(TargetAttribute.PropertyName))
            {
                continue;
            }

            object? Value = TryConvertValue(definition.GetValue(TargetAttribute.PropertyName, intensity)!,
                Property.PropertyType, TargetAttribute.PropertyName);
            Property.SetValue(modifier, Value);
        }
    }
}