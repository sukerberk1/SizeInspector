using System.IO;

namespace SizeInspector;

public class Program
{
    enum Mode
    {
        Normal,
        Analyze
    }
    public enum OutputOption
    {
        Bytes,
        Kilobytes,
        Megabytes,
        Gigabytes
    }
    public enum LogOption
    { 
        NoLogs,
        ConsoleLogs,
        FileLogs
    }

    // Specify options for the program class;
    Mode RunMode;
    OutputOption output;
    LogOption logOption;
    readonly DirectoryInfo location;


    public Program(DirectoryInfo location)
        => this.location = location;

    static void DisplayHelp()
    {
        Console.WriteLine("Usage: SizeInspector <options> <location eg. \"C:/\">");
        Console.WriteLine("Available options:");
        Console.WriteLine("-log-console OR -log-file    - specifies what to do with logs. File log output currently not supported.");
        Console.WriteLine("-b OR -kb OR -gb             - changes output format to bytes, kilobytes or gigabytes");
        Console.WriteLine("--analyze                    - runs progam in analyzer mode, showing the size of all subdirectories.");
    }

    static void Main(string[] args)
    {

        // Check if required args are provided
        if (args.Length == 0)
        {
            Console.WriteLine("No location specified.");
            DisplayHelp();
            return;
        }

        // Check if given location is valid
        DirectoryInfo location = new(@args.Last());
        if (!location.Exists)
        {
            Console.WriteLine("This location does not seem to exist.");
            DisplayHelp();
            return;
        }

        // Create a program instance from given location and assign default settings
        Program program = new(location)
        {
            RunMode = Mode.Normal,
            logOption = LogOption.NoLogs
        };

        // Customize run mode
        if (args.Contains("--analyze")) program.RunMode = Mode.Analyze; 

        // Register logging options
        if (args.Contains("-log-console")) program.logOption = LogOption.ConsoleLogs;
        else if (args.Contains("-log-file")) //program.logOption = LogOption.File;
        {
            Console.WriteLine("File logging currently not supported. Program execution ommited.");
            DisplayHelp();
            return;
        }

        // Register output options
        program.output = OutputOption.Megabytes;
        if (args.Contains("-gb")) program.output = OutputOption.Gigabytes;
        else if (args.Contains("-kb")) program.output = OutputOption.Kilobytes;
        else if (args.Contains("-b")) program.output = OutputOption.Bytes;

        program.Run();

    }

    public void Run()
    {

        Console.WriteLine("Running...");

        // Run mode - Analyze
        if (RunMode == Mode.Analyze)
        {
            Dictionary<DirectoryInfo, double> data = new();
            foreach (DirectoryInfo dir in location.GetDirectories())
            {
                data.Add(dir, GetDirSize(dir, 2));
            }
            foreach (var dir in data)
            {
                Console.WriteLine($"Directory: {dir.Key}; Size: {dir.Value} " + Enum.GetName(typeof(OutputOption), output));
            }
            Console.WriteLine("Total files size in the initial directory: "
                + $"{location.TotalFilesSize(logOption, output)} " 
                + Enum.GetName(typeof(OutputOption), output));
            return;
        }

        // Run mode - Normal
        double Size = GetDirSize(location);
        Console.WriteLine($"\nGiven directory: {location.FullName}; Size: {Size} " + Enum.GetName(typeof(OutputOption), output));
    }

    /* Gets directories size in Gigabytes */
    public double GetDirSize(DirectoryInfo location,short precision = 2)
    {
        if ((int)output == 0) precision = 0;
        long totalSizeBytes = 0;
        CalculateSize(ref totalSizeBytes, location);
        double totalSizeGB = totalSizeBytes;
        totalSizeGB = Math.Round(totalSizeGB / Math.Pow(1024, (int)output), precision);
        return totalSizeGB;
    }


    private void CalculateSize(ref long size, DirectoryInfo location)
    {
        size += location.TotalFilesSize(logOption);
        try
        {
            foreach (var directory in location.GetDirectories())
                CalculateSize(ref size, directory);
        }
        catch
        {
            if(logOption == LogOption.ConsoleLogs) Console.WriteLine($"Could not get subdirectories of {location.FullName}. Therefore, not accounting for its subdirectories size.");
        }
    }
}