using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SizeInspector.Program;

namespace SizeInspector
{
    public static class DirectoryInfoExtensions
    {
        /* Function which returns total size of files in a directory in bytes. */
        public static long TotalFilesSize(this DirectoryInfo folder, LogOption logs)
        {
            try
            {
                return folder.EnumerateFiles().Sum(f => f.Length);
            }
            catch
            {
                if (logs == LogOption.ConsoleLogs) Console.WriteLine($"Could not proceed counting file size for directory {folder.FullName}.");
                return 0;
            }
        }

        // When specified output option, return double and different format instead.
        public static double TotalFilesSize(this DirectoryInfo folder, LogOption logs, OutputOption output)
        {
            try
            {
                return Math.Round(folder.EnumerateFiles().Sum(f => f.Length) / Math.Pow(1024, (int)output), 2);
            }
            catch { return 0; }
        }

    }
}
