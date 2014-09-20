using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp
{
    /// <summary>
    /// Interface used to resolve paths (..\test.file, test.file, C:\test.file, etc)
    /// </summary>
    public interface IPathResolver
    {
        /// <summary>
        /// Resolve a path with the specified relative directory
        /// </summary>
        /// <param name="path"></param>
        /// <param name="relativeDirectory"></param>
        /// <returns></returns>
        string ResolvePath(string path, string relativeDirectory);
    }
}
