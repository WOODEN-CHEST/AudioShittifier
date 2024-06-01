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


    // Fields.
    public string SourceFilePath { get; private set; }
    public string DestinationFilePath { get; private set; }
    public float Intensity { get; private set; } = 0.2f;


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
        foreach (string Argument in args)
        {
            string[] KeyValuePair = Argument.Split('=');
            if (KeyValuePair.Length != 2)
            {
                throw new ShittifierArgumentException($"Invalid argument: \"{Argument}\"");
            }

            if (KeyValuePair[0] == ARG_SOURCE)
            {
                SourceFilePath = KeyValuePair[1];
            }
            else if (KeyValuePair[0] == ARG_DEST)
            {
                DestinationFilePath = KeyValuePair[1];
            }
            else if (KeyValuePair[0] == ARG_INTENSITY)
            {
                if (float.TryParse(KeyValuePair[1], CultureInfo.InvariantCulture, out float Result))
                {
                    Intensity = Result;
                }
                else
                {
                    throw new ShittifierArgumentException($"Expected number for intensity: \"{Argument}\"");
                }
            }
        }
    }
}