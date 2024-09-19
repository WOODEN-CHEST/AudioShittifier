using System.Globalization;


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
    public string DestinationDirectory { get; private set; }
    public double Intensity { get; private set; } = 1d;
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
        DestinationDirectory ??= Path.Combine(Path.GetDirectoryName(SourceFilePath)!, "out");
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

    private void SetSourceFilePath(string value)
    {
        SourceFilePath = value.Replace("\"", string.Empty);
        if (!Path.IsPathFullyQualified(SourceFilePath))
        {
            throw new ShittifierArgumentException(
                $"Source file or directory path \"{SourceFilePath}\" is not fully qualified");
        }
    }

    private void SetDestinationFilePath(string value)
    {
        DestinationDirectory = value.Replace("\"", string.Empty);
        if (!Path.IsPathFullyQualified(DestinationDirectory))
        {
            throw new ShittifierArgumentException(
                $"Destination directory path \"{value}\" is not fully qualified");
        }
    }

    private void SetLayoutPath(string value)
    {
        LayoutPath = value.Replace("\"", string.Empty);
        if (!Path.IsPathFullyQualified(LayoutPath))
        {
            throw new ShittifierArgumentException(
                $"Layout path \"{value}\" is not fully qualified");
        }
    }

    private void SetIntensity(string value)
    {
        if (double.TryParse(value, CultureInfo.InvariantCulture, out double Result))
        {
            Intensity = double.IsNaN(Result) ? Intensity : Math.Clamp(Result, 0d, 1d);
        }
        else
        {
            throw new ShittifierArgumentException($"Expected number for intensity: \"{value}\"");
        }
    }

    private void ParseSingleArgument(string key, string value)
    {
        switch (key)
        {
            case ARG_SOURCE:
                SetSourceFilePath(value);
                break;

            case ARG_DEST:
                SetDestinationFilePath(value);
                break;

            case ARG_INTENSITY:
                SetIntensity(value);
                break;

            case ARG_LAYOUT:
                LayoutPath = value;
                break;
        }
    }
}