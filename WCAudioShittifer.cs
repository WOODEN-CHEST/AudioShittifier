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
}