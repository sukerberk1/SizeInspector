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

        long totalSizeBytes = 0;

        try
        {
            Console.WriteLine("Running...");
            CalculateSize(ref totalSizeBytes, location);
            double totalSizeGB = totalSizeBytes;
            totalSizeGB /= Math.Pow(1024, 3);
            Console.WriteLine($"Given directory: {location.FullName}; Its Size: {Math.Round(totalSizeGB, 2)} GB");
        }
        catch(UnauthorizedAccessException)
        {
            Console.WriteLine("One of analyzed files denied access. Try to run the program as an administrator.");
        }

    }


    static void CalculateSize(ref long size, DirectoryInfo location)
    {
        size += location.TotalFilesSize();
        foreach (var directory in location.GetDirectories())
        {
            CalculateSize(ref size, directory);
        }
    }

}