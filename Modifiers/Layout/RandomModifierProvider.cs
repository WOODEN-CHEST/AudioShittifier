using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AudioShittifier.Modifiers.Layout;

public class RandomModifierProvider : IModifierProvider
{
    // Private static fields.
    private const string MODIFIER_RESAMPLE = "resample";
    private const string MODIFIER_PRECISION = "precision";
    private const string MODIFIER_MONO = "mono";
    private const string MODIFIER_ECHO = "echo";
    private const string MODIFIER_REPEAT = "repeat";
    private const string MODIFIER_RANDOMIZE = "randomize";
    private const string MODIFIER_MULTIPLIER = "multiplier";
    private const string MODIFIER_BIQUADFILTER = "biquadfilter";
    private const string MODIFIER_CLIPPING = "clipping";
    private const string MODIFIER_WAVEFORM = "waveform";

    private const string PROPERTY_SAMPLE_RATE = "sample_rate";
    private const string PROPERTY_SAMPLE_TYPE = "sample_type";


    // Private fields.
    private readonly ModifierLayout _layout;


    // Constructors.
    public RandomModifierProvider(ModifierLayout layout)
    {
        _layout = layout ?? throw new ArgumentNullException(nameof(layout));
    }


    // Private methods.
    private void PurchaseModifierFromGroup(ModifierGroup group, ref double funds, out IAudioModifier? modifier)
    {
        double CurrentFunds = funds;
        ModifierDefinition[] AvailableModifiers = group.Where(mod => mod.TotalCost <= funds).ToArray();
        if (AvailableModifiers.Length == 0)
        {
            modifier = null;
            return 0d;
        }
        double[] Weights = AvailableModifiers.Select(mod => mod.Weight).ToArray();

        WeightedCollection<ModifierDefinition> WeightedModifiers = new(AvailableModifiers, Weights);
        return PurchaseModifierFromDefinition(WeightedModifiers.GetRandom()!, funds, out modifier);
    }

    private double PurchaseModifierFromDefinition(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {
        return (definition.Name) switch
        {
            MODIFIER_RESAMPLE => GetResampleModifier(definition, funds, out modifier),
            MODIFIER_MONO => GetMonoModifier(definition, funds, out modifier),
            MODIFIER_CLIPPING => GetClippingModifier(definition, funds, out modifier),
            MODIFIER_PRECISION => GetPrecisionModifier(definition, funds, out modifier),
            MODIFIER_BIQUADFILTER => GetBiQuadFilterModifier(definition, funds, out modifier),
            MODIFIER_RANDOMIZE => GetRandomizeModifier(definition, funds, out modifier),
            MODIFIER_REPEAT => GetRepeatModifier(definition, funds, out modifier),
            MODIFIER_WAVEFORM => GetWaveformModifier(definition, funds, out modifier),
            MODIFIER_ECHO => GetEchoModifier(definition, funds, out modifier),
            MODIFIER_MULTIPLIER => GetMultiplierModifier(definition, funds, out modifier),
            _ => throw new ModifierLayoutException($"Unknown modifier definition: \"{definition.Name}\"")
        };
    }

    private double GetNumberFromProperty(ModifierProperty? property, double defaultValue, double funds, out double number)
    {
        if (property == null)
        {
            number = defaultValue;
            return 0d;
        }

        if (property.TryGetValue(out NumberRange? TargetRange))
        {
            if (property.Cost <= 0d)
            {
                number = TargetRange!.WorstValue;
                return 0d;
            }
            double PickedAmount = Math.Min(1d, funds / property.Cost) * Random.Shared.NextDouble();
            number = (TargetRange.BestValue) + ((TargetRange.WorstValue - TargetRange.BestValue) * PickedAmount);
            return PickedAmount * property.Cost;
        }

        if (property.TryGetValue(out double[]? NumberArray))
        {
            number = NumberArray![Random.Shared.Next(NumberArray.Length)];
            return 0d;
        }
        
        if (property.TryGetValue(out double Value))
        {
            number = Value;
            return 0d;
        }
        throw new ModifierLayoutException($"Expected number value for property \"{property.Name}\"");
    }

    private string GetStringFromProperty(ModifierProperty? property, double defaultValue)
    {
        if (property.TryGetValue(out string[]? StringArray))
        {
            return StringArray![Random.Shared.Next(StringArray.Length)];
        }

        if (property.TryGetValue(out string? Value))
        {
            return Value!;
        }

        throw new ModifierLayoutException($"Expected string value for property \"{property.Name}\"");
    }

    private double GetResampleModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {
        ResampleModifier Modifier = new();
        double NewFunds = funds;
        NewFunds -= GetNumberFromProperty(definition.TryGetProperty(PROPERTY_SAMPLE_RATE), Modifier.SampleRate, funds) 



        ModifierProperty? SampleRateProperty = definition.TryGetProperty(PROPERTY_SAMPLE_RATE);
        if (SampleRateProperty != null)
        {
            if (SampleRateProperty.TryGetValue(out NumberRange? TargetRange))
            {
                double PickedValue = 
            }
        }





        return Modifier;
    }

    private double GetPrecisionModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {

    }

    private double GetMonoModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {

    }

    private double GetMultiplierModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {

    }

    private double GetClippingModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {

    }

    private double GetEchoModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {

    }
    private double GetRepeatModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {

    }

    private double GetWaveformModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {

    }

    private double GetRandomizeModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {

    }

    private double GetBiQuadFilterModifier(ModifierDefinition definition, double funds, out IAudioModifier modifier)
    {

    }


    // Inherited methods.
    public IAudioModifier[] GetModifiers(double intensity)
    {
        double Funds = _layout.MaxCost * intensity;
        List<ModifierGroup> AvailableGroups = new(_layout.Groups);
        List<IAudioModifier> PurchasedModifiers = new();

        while (AvailableGroups.Count > 0)
        {
            int GroupIndex = Random.Shared.Next(AvailableGroups.Count);
            Funds -= PurchaseModifierFromGroup(AvailableGroups[GroupIndex],
                Funds * Random.Shared.NextDouble(), out IAudioModifier? Modifier);

            if (Modifier != null)
            {
                PurchasedModifiers.Add(Modifier);
            }
            AvailableGroups.RemoveAt(GroupIndex);
        }
        return PurchasedModifiers.ToArray();
    }
}