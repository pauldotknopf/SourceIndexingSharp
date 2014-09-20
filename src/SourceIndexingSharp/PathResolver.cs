using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp
{
    /// <summary>
    /// Service used to resolve paths (..\test.file, test.file, C:\test.file, etc)
    /// </summary>
    public class PathResolver : IPathResolver
    {
        /// <summary>
        /// Resolve a path with the specified relative directory
        /// </summary>
        /// <param name="path"></param>
        /// <param name="relativeDirectory"></param>
        /// <returns></returns>
        public string ResolvePath(string path, string relativeDirectory)
        {
            if (Path.IsPathRooted(path))
                return path;

            if (string.IsNullOrEmpty(relativeDirectory))
                relativeDirectory = AppDomain.CurrentDomain.BaseDirectory;

            return Path.GetFullPath(Path.Combine(relativeDirectory, path));
        }
    }
}
