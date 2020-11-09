using System;
using System.IO;

namespace GenHelper.MainLoop.Services
{
    public static class PathHelpers
    {
        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToUpperInvariant();
        }
    }
}
