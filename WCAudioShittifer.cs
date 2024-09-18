using AudioShittifier.Layout;

namespace AudioShittifier;

public static class WCAudioShittifier
{
    // Static methods.
    public static void Main(string[] args)
    {
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
}