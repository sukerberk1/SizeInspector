using System.IO;
using SizeInspectorExtensionMethods;

namespace SizeInspector;

internal class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Unknown location.");
            Console.WriteLine("Usage: SizeInspector <location eg. C:/>");
            return;
        }
        DirectoryInfo location = new(@args[0]);

        

        Console.WriteLine("Running...");
        double Size = GetDirSize(location);
        Console.WriteLine($"Given directory: {location.FullName}; Size: {Size} GB");

    }

    /* Gets directories size in Gigabytes */
    public static double GetDirSize(DirectoryInfo location, short precision = 2)
    {
        long totalSizeBytes = 0;
        CalculateSize(ref totalSizeBytes, location);
        double totalSizeGB = totalSizeBytes;
        totalSizeGB = Math.Round(totalSizeGB / Math.Pow(1024, 3), precision);
        return totalSizeGB;
    }


    private static void CalculateSize(ref long size, DirectoryInfo location)
    {
        size += location.TotalFilesSize();
        try
        {
            foreach (var directory in location.GetDirectories()) 
                CalculateSize(ref size, directory);
        }
        catch
        {
            Console.WriteLine($"Could not get subdirectories of {location.FullName}. Therefore, not accounting for its subdirectories size.");
        }
    }

}