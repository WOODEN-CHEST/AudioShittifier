using AudioShittifier.Layout;
using AudioShittifier.Modifiers;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public class Shittifier
{
    // Fields.
    public event EventHandler<ShittifyCompleteEventArgs> FileShittify;

    // Private static fields.
    private const string TARGET_FILE_EXTENSION = ".mp3";


    // Constructors.
    public Shittifier() { }


    // Methods.
    public void Shittify(string[] programArgs)
    { 
        ShittifierArguments ParsedArguments = ReadArguments(programArgs);
        string[] FilesToShittify = GetFilesToShittify(ParsedArguments.SourceFilePath);
        Directory.CreateDirectory(ParsedArguments.DestinationDirectory);
        ModifierLayout Layout = GetModifierLayout(ParsedArguments.LayoutPath);
        ShittifyFiles(FilesToShittify, ParsedArguments.DestinationDirectory, ParsedArguments.Intensity, Layout);
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

    private string[] GetFilesToShittify(string sourcePath)
    {
        string[] FilesToShittify;
        if (File.Exists(sourcePath))
        {
            FilesToShittify = new string[] { sourcePath };
        }
        else if (Directory.Exists(sourcePath))
        {
            FilesToShittify = Directory.GetFiles(sourcePath, $"*{TARGET_FILE_EXTENSION}", SearchOption.AllDirectories);
        }
        else
        {
            throw new ShittifyException("Source path does not exist.");
        }

        if (FilesToShittify.Length == 0)
        {
            throw new ShittifyException("No files found to shittify with the given path.");
        }

        return FilesToShittify;
    }

    private ModifierLayout GetDefaultModifierLayout()
    {
        return new ModifierLayout(Array.Empty<ModifierDefinition>());
    }

    private ModifierLayout GetModifierLayout(string? layoutPath)
    {
        if (layoutPath == null)
        {
            throw new ShittifyException("No layout provided, can't shittify file.");
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

    private void ShittifyFiles(string[] filePaths, string outputDir, double intensity, ModifierLayout layout)
    {
        for (int i = 0; i < filePaths.Length; i++)
        {
            string FilePath = filePaths[i];
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
            FileShittify?.Invoke(this, new(FilePath, i + 1, filePaths.Length));
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
        File.Delete(path);
        SampleToWaveConverter Converter = new(buffer.Format, buffer.Samples);
        MediaFoundationEncoder.EncodeToMp3(Converter, path);
    }
}