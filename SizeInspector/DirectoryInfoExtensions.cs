using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SizeInspectorExtensionMethods;

public static class DirectoryInfoExtensions
{
    /* Function meant to be called when approaching a folder without subdirectories - folder that contains only files.*/
    public static long TotalFilesSize(this DirectoryInfo folder) => folder.EnumerateFiles().Sum(f => f.Length);
}
