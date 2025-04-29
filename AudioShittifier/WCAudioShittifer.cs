using AudioShittifier.Layout;

namespace AudioShittifier;

public static class WCAudioShittifier
{
    // Static methods.
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage();
            return;
        }

        try
        {
            Shittifier AudioShittifier = new();
            AudioShittifier.FileShittify += OnShittifyFileEvent;
            AudioShittifier.Shittify(args);
        }
        catch (ShittifyException e)
        {
            Console.WriteLine($"User error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to shittify audio file due to an internal error! {e}");
        }
    }


    // Private static methods.
    private static void OnShittifyFileEvent(object? sender, ShittifyCompleteEventArgs args)
    {
        double ProgressPercent = (double)args.FileNumber / args.MaxFileNumber * 100d;
        Console.WriteLine($"Shittified \"{Path.GetFileName(args.FilePath)}\" ({ProgressPercent.ToString("0.00")}% done)");
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage: Pass arguments with <key>=<value> where <key> is the name of the argument" +
            " and <value> is its value.\n" +
            "Available arguments:\n" +
            "    source=<string> -The source directory or file path which contains the file(s) to shittify.\n" +
            "    destination=<string> -The destination directory path in which to place the shittified files, may be omitted.\n" +
            "    layout=<string> -The file path to the layout file.\n" +
            "    intensity=<real number> -The intensity of effects used from the layout, may be omitted.\n" +
            "Remember to wrap paths in double quotes \" if they have spaces in them.");
    }
}