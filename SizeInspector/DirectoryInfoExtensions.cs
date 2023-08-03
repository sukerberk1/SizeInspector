using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeInspectorExtensionMethods;

public static class DirectoryInfoExtensions
{
    /* Function which returns total size of files in a directory. */
    public static long TotalFilesSize(this DirectoryInfo folder) 
    {
        try
        {
            return folder.EnumerateFiles().Sum(f => f.Length);
        }
        catch
        {
            Console.WriteLine($"Could not proceed counting file size for directory {folder.FullName}.");
            return 0;
        }
    } 
}
