using AudioShittifier.Modifiers;
using System.Collections;


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
        VerifyLayout();
    }


    // Private fields.
    private void VerifyLayout()
    {
        ModifierBuilder Builder = new();
        try
        {
            foreach (ModifierDefinition Def in _modifierDefinitions)
            {
                Builder.GetModifier(Def); // If the layout is invalid, the build will fail.
            }
        }
        catch (ModifierBuildException e)
        {
            throw new ModifierLayoutException($"Invalid layout: {e.Message}");
        }
        
    }

    private int[] GetIndexes(int count, int unusedIndexCount)
    {
        List<int> IndexList = new(Enumerable.Range(0, count));
        for (int i = 0; i < unusedIndexCount; i++)
        {
            IndexList.RemoveAt(Random.Shared.Next(IndexList.Count));
        }
        return IndexList.ToArray();
    }


    // Methods.
    public IAudioModifier[] GetModifiers(double intensity)
    {
        int UnusedModifierCount = _modifierDefinitions.Length -
            Math.Clamp((int)Math.Round(intensity * _modifierDefinitions.Length), 0, _modifierDefinitions.Length);
        List<IAudioModifier> AudioModifiers = new();
        ModifierBuilder Builder = new();

        foreach (int Index in GetIndexes(_modifierDefinitions.Length, UnusedModifierCount))
        {
            try
            {
                AudioModifiers.Add(Builder.GetModifier(_modifierDefinitions[Index]));
            }
            catch (ModifierBuildException e)
            {
                throw new ModifierLayoutException($"Failed to build modifier for layout: {e.Message}");
            }
        }

        return AudioModifiers.ToArray();
    }
}