
using AudioShittifier.Audio;
using AudioShittifier.Audio.Modifiers;
using AudioShittifier.Modifiers;
using NAudio.MediaFoundation;
using NAudio.Wave;
using System.Collections.Concurrent;
using System.Reflection.PortableExecutable;

namespace AudioShittifier;

public static class WCAudioShittifier
{
    // Static methods.
    public static void Main(string[] args)
    {
        try
        {
            ShittifierArguments Arguments = new(args);
            ShittifyFiles(Arguments);
        }
        catch (ShittifierArgumentException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to shittify audio file due to an internal error! {e}");
        }
    }

    // Private static methods.
    private static void ShittifyFiles(ShittifierArguments arguments)
    {
        string[] AllFilePaths = Directory.GetFiles(arguments.SourceFilePath, "*.mp3");
        Console.WriteLine($"Shittifying {AllFilePaths.Length} file{(AllFilePaths.Length == 1 ? string.Empty : "s")}.");
        for (int i = 0; i < AllFilePaths.Length; i++)
        {
            string Destination = Path.Combine(arguments.DestinationFilePath, Path.GetFileName(AllFilePaths[i]));
            ShittifySingleFile(AllFilePaths[i], Destination, arguments.Intensity);
            Console.WriteLine($"Shittified file \"{AllFilePaths[i]}\". " +
                $"({((float)(i + 1) / AllFilePaths.Length * 100f).ToString("0.00")}% done)");
        }
        Console.WriteLine("Finished shittifying files.");
    }

    private static void ShittifySingleFile(string filePath, string destPath, float intensity)
    {
        Shittifier AudioShittifier = GetRandomShittifier(intensity);
        float[] Data = ReadAudioFile(filePath, out WaveFormat format);
        AudioShittifier.Shittify(Data, format);
        WriteAudioFile(destPath, Data, format);
        
    }

    private static Shittifier GetRandomShittifier(float intensity)
    {
        // Lots of magic numbers for chances here.
        List<IAudioModifier> Modifiers = new();

        if (ShittifierRandom.TryWithChance(0.6, intensity))
        {
            Modifiers.Add(new BiQuadFilterModifier()
            {
                PassType = BiQualFilterPassType.HighPass,
                Frequency = (int)ShittifierRandom.RandomNumberWithIntensity(1000, 8000, intensity),
            });
        }
        if (ShittifierRandom.TryWithChance(0.3, intensity))
        {
            Modifiers.Add(new BiQuadFilterModifier()
            {
                PassType = BiQualFilterPassType.LowPass,
                Frequency = (int)ShittifierRandom.RandomNumberWithIntensity(3000, 200, intensity),
            });
        }
        if (ShittifierRandom.TryWithChance(0.1d, intensity))
        {
            Modifiers.Add(new EchoModifier()
            {
                Offset = TimeSpan.FromMilliseconds((int)ShittifierRandom.RandomNumberWithIntensity(50, 2000, intensity)),
            });
        }
        if (ShittifierRandom.TryWithChance(0.2, intensity))
        {
            Modifiers.Add(new RepeatModifier()
            {
                AverageRepeatCount = (int)ShittifierRandom.RandomNumberWithIntensity(1, 30, intensity),
                AverageRepeatSectionDuration = TimeSpan.FromMilliseconds(Random.Shared.Next(0, 400)),
                RepeatChancePerSecond = ShittifierRandom.RandomNumberWithIntensity(0.001, 0.4, intensity)
            });
        }
        if (ShittifierRandom.TryWithChance(0.2d, intensity))
        {
            Modifiers.Add(new WaveformModifier()
            {
                ChancePerSecond = ShittifierRandom.RandomNumberWithIntensity(0.001, 0.4, intensity),
                AverageSectionDuration = TimeSpan.FromMilliseconds(Random.Shared.Next(0, 2000)),
            });
        }
        if (ShittifierRandom.TryWithChance(8d, intensity))
        {
            Modifiers.Add(new SampleMultiplier()
            {
                Multiplier = (int)ShittifierRandom.RandomNumberWithIntensity(1, 20, intensity),
                ClippingThreshold = (float)ShittifierRandom.RandomNumberWithIntensity(5, 1.5, intensity)
            });
        }
        if (ShittifierRandom.TryWithChance(0.5d, intensity))
        {
            Modifiers.Add(new SampleRandomizerModifier()
            { PortionOfSamplesRandomized = (int)ShittifierRandom.RandomNumberWithIntensity(0.0001d, 0.05, intensity) });
        }
        if (ShittifierRandom.TryWithChance(4d, intensity))
        {
            Modifiers.Add(new PrecisionModifier()
            { BitDepth = (int)ShittifierRandom.RandomNumberWithIntensity(32, 2, intensity) });
        }
        if (ShittifierRandom.TryWithChance(8d, intensity))
        {
            Modifiers.Add(new ResampleModifier()
            {
                SampleRate = (int)ShittifierRandom.RandomNumberWithIntensity(44100, 200, intensity),
                Type = ResampleModifierType.Old
            });
        }

        return new Shittifier(Modifiers.ToArray());
    }

    private static float[] ReadAudioFile(string path, out WaveFormat format)
    {
        using AudioFileReader Reader = new(path);
        float[] Buffer = new float[Reader.Length / sizeof(float)];
        Reader.Read(Buffer, 0, Buffer.Length);
        format = Reader.WaveFormat;
        return Buffer;
    }

    private static void WriteAudioFile(string path, float[] sampleBuffer, WaveFormat format)
    {
        SampleToWaveConverter Converter = new(format, sampleBuffer);
        MediaFoundationEncoder.EncodeToMp3(Converter, path);
    }
}