using AudioShittifier.Modifiers;


namespace AudioShittifier.Layout;


public class ModifierLayout
{
    // Private fields.
    private ModifierDefinition[] _modifierDefinitions;


    // Constructors.
    public ModifierLayout(ModifierDefinition[] modifiers)
    {
        ArgumentNullException.ThrowIfNull(modifiers, nameof(modifiers));
        if (modifiers.Contains(null))
        {
            throw new ArgumentException("Modifiers array contains null", nameof(modifiers));
        }
        _modifierDefinitions = modifiers.ToArray();
    }


    // Methods.
    public IAudioModifier[] GetModifiers(double intensity)
    {
        int MaxModifiers = Math.Clamp((int)Math.Round(intensity * _modifierDefinitions.Length), 0, _modifierDefinitions.Length);
        List<ModifierDefinition> AvailableModifiers = new(_modifierDefinitions);
        List<IAudioModifier> AudioModifiers = new();
        ModifierBuilder Builder = new();

        for (int i = 0; i < MaxModifiers; i++)
        {
            int UsedIndex = Random.Shared.Next(AvailableModifiers.Count);
            try
            {
                AudioModifiers.Add(Builder.GetModifier(AvailableModifiers[UsedIndex], intensity));
            }
            catch (ModifierBuildException e)
            {
                throw new ModifierLayoutException($"Failed to build modifier for layout: {e.Message}");
            }
            AvailableModifiers.RemoveAt(UsedIndex);
        }

        return AudioModifiers.ToArray();
    }
}