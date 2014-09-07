using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp.Tools
{
    /// <summary>
    /// Flags to pass to srctool.exe
    /// </summary>
    [Flags]
    public enum SrcToolFlags
    {
        /// <summary>
        /// No flags.
        /// </summary>
        None = 0,
        /// <summary>
        /// -u
        /// Displays only source files that are not indexed.
        /// </summary>
        DisplayOnlySourceFilesNotIndexed = 1,
        /// <summary>
        /// -r
        /// Dumps raw source data from the pdb.
        /// </summary>
        RawSource = 1 << 1,
        /// <summary>
        /// -s
        /// Recursive subdirectories.
        /// </summary>
        RecursiveSubDirectories = 1 << 2,
        /// <summary>
        /// -x
        /// Extracts the files, instead of simply listing them.
        /// </summary>
        ExtractFiles = 1 << 3,
        /// <summary>
        /// -f
        /// Extracts the files to a flag directory.
        /// </summary>
        ExtractToFlatDirectory = 1 << 4,
        /// <summary>
        /// -n,
        /// Show version control commands and output while extracting.
        /// </summary>
        ShowVersionControlCommandsWhileExtracting = 1 << 5,
        /// <summary>
        /// -c
        /// Displays only the count of indexed files - no detail.
        /// </summary>
        DisplayCount = 1 << 6
    }
}
