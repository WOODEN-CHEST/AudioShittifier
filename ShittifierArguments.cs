using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioShittifier;

public record class ShittifierArguments
{
    // Static fields.
    public const string ARG_SOURCE = "source";
    public const string ARG_DEST = "destination";
    public const string ARG_INTENSITY = "intensity";
    public const string ARG_LAYOUT = "layout";


    // Fields.
    public string SourceFilePath { get; private set; }
    public string DestinationFilePath { get; private set; }
    public float Intensity { get; private set; } = 0.2f;
    public string? LayoutPath { get; private set; } = null;


    // Constructors.
    public ShittifierArguments(string[] args)
    {
        if (args == null)
        {
            throw new ArgumentNullException("args");
        }

        ParseArguments(args);
        if (SourceFilePath == null)
        {
            throw new ShittifierArgumentException($"Missing source directory.");
        }
        if (!Path.IsPathFullyQualified(SourceFilePath))
        {
            throw new ShittifierArgumentException($"Source directory path is not fully qualified.");
        }
        if (!Directory.Exists(SourceFilePath))
        {
            throw new ShittifierArgumentException($"Source directory does not exist.");
        }
        if (DestinationFilePath == null)
        {
            DestinationFilePath = Path.Combine(Path.GetDirectoryName(SourceFilePath) ?? string.Empty, "out");
        }
        if (!Path.IsPathFullyQualified(DestinationFilePath))
        {
            throw new ShittifierArgumentException($"'Destination file path is not fully qualified.");
        }
        Directory.CreateDirectory(DestinationFilePath);
    }


    // Private methods.
    private void ParseArguments(string[] args)
    {
        HashSet<string> ParsedValues = new();
        foreach (string Argument in args)
        {
            string[] KeyValuePair = Argument.Split('=');
            if (KeyValuePair.Length != 2)
            {
                throw new ShittifierArgumentException($"Invalid argument: \"{Argument}\"");
            }
            if (ParsedValues.Contains(KeyValuePair[0]))
            {
                throw new ShittifierArgumentException($"Duplicate argument \"{KeyValuePair[0]}\"");
            }

            ParseSingleArgument(KeyValuePair[0], KeyValuePair[1]);
            ParsedValues.Add(KeyValuePair[0]);
        }
    }

    private void ParseSingleArgument(string key, string value)
    {
        switch (key)
        {
            case ARG_SOURCE:
                SourceFilePath = value;
                break;

            case ARG_DEST:
                DestinationFilePath = value;
                break;

            case ARG_INTENSITY:
                if (float.TryParse(key, CultureInfo.InvariantCulture, out float Result))
                {
                    Intensity = Result;
                }
                else
                {
                    throw new ShittifierArgumentException($"Expected number for intensity: \"{value}\"");
                }
                break;

            case ARG_LAYOUT:
                LayoutPath = value;
                break;
        }
    }
}