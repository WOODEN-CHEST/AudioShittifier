using AudioShittifier.Layout;
using AudioShittifier.Modifiers;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public class Shittifier
{
    // Private static fields.
    private const string TARGET_FILE_EXTENSION = ".mp3";


    // Constructors.
    public Shittifier()
    {

    }


    // Methods.
    public void Shittify(string[] programArgs)
    { 
        ShittifierArguments ParsedArguments = ReadArguments(programArgs);
        if (!Path.IsPathFullyQualified(ParsedArguments.SourceFilePath))
        {
            throw new ShittifyException(
                $"Source file or directory path \"{ParsedArguments.SourceFilePath}\" is not fully qualified");
        }
        IEnumerable<string> FilesToShittify = GetFilesToShittify(ParsedArguments.SourceFilePath);
        string Destionation = GetDestinationDirecotry(ParsedArguments.DestinationDirectory, ParsedArguments.SourceFilePath);
        ShittifyFiles(FilesToShittify, Destionation, ParsedArguments.Intensity, 
            GetModifierLayout(ParsedArguments.LayoutPath));
    }


    // Private methods.
    private ShittifierArguments ReadArguments(string[] args)
    {
        try
        {
           return new(args);
        }
        catch (ShittifierArgumentException e)
        {
            throw new ShittifyException($"Failed parsing arguments for shittifier: {e.Message}");
        }
    }

    private IEnumerable<string> GetFilesToShittify(string sourcePath)
    {
        List<string> FilesToShittify = new();
        if (File.Exists(sourcePath))
        {
            FilesToShittify.Add(sourcePath);
        }
        else if (Directory.Exists(sourcePath))
        {
            FilesToShittify.AddRange(Directory.GetFiles(sourcePath, $"*{TARGET_FILE_EXTENSION}", SearchOption.AllDirectories));
        }
        else
        {
            throw new ShittifyException("Source path does not exist.");
        }

        if (FilesToShittify.Count == 0)
        {
            throw new ShittifyException("No files found to shittify with the given path.");
        }

        return FilesToShittify;
    }

    private string GetDestinationDirecotry(string? destinationDirectory, string? sourceDirectory)
    {
        string FinalDirectory;
        if (destinationDirectory == null)
        {
            FinalDirectory = Path.Combine(Path.GetDirectoryName(sourceDirectory) ?? string.Empty, "out");
        }
        else if (!Path.IsPathFullyQualified(destinationDirectory))
        {
            throw new ShittifyException($"Destination directory \"{destinationDirectory}\" is not fully qualified");
        }
        else
        {
            FinalDirectory = destinationDirectory;
        }
        Directory.CreateDirectory(FinalDirectory);
        return FinalDirectory;
    }

    private ModifierLayout GetDefaultModifierLayout()
    {
        throw new NotImplementedException();
    }

    private ModifierLayout GetModifierLayout(string? layoutPath)
    {
        if (layoutPath == null)
        {
            return GetDefaultModifierLayout();
        }

        try
        {
            IModifierLayoutParser Parser = new JSONModifierLayoutParser();
            return Parser.GetLayout(layoutPath);
        }
        catch (ModifierParseException e)
        {
            throw new ShittifyException(e.Message);
        }
    }

    private void ShittifyFiles(IEnumerable<string> filePaths, string outputDir, double intensity, ModifierLayout layout)
    {
        foreach (string FilePath in filePaths)
        {
            SampleBuffer Buffer = ReadAudioFile(FilePath);
            IAudioModifier[] Modifiers;
            try
            {
                Modifiers = layout.GetModifiers(intensity);
            }
            catch (ModifierLayoutException e)
            {
                throw new ShittifyException($"Invalid layout: {e.Message}");
            }

            foreach (IAudioModifier Modifier in Modifiers)
            {
                Modifier.Modify(Buffer);
            }

            string Destination = Path.Combine(outputDir, Path.GetFileName(FilePath));
            WriteAudioFile(Destination, Buffer);
        }
    }

    private SampleBuffer ReadAudioFile(string path)
    {
        using AudioFileReader Reader = new(path);
        float[] Buffer = new float[Reader.Length / sizeof(float)];
        Reader.Read(Buffer, 0, Buffer.Length);
        return new(Buffer, Reader.WaveFormat);
    }

    private void WriteAudioFile(string path, SampleBuffer buffer)
    {
        SampleToWaveConverter Converter = new(buffer.Format, buffer.Samples);
        MediaFoundationEncoder.EncodeToMp3(Converter, path);
    }
}