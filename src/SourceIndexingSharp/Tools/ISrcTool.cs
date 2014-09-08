using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp.Tools
{
    public interface ISrcTool
    {
        /// <summary>
        /// Dumps source information froma pdb or srcsrv stream file.
        /// You must specify a pdb, executable, or srcsrv stream file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="flags">The flags to pass to the command.</param>
        /// <param name="limitSourceFiles">Limits the output to only the source files that match this wildcard.</param>
        /// <param name="limitSourceFileNames">Same as 'limitSourceFiles' except that the mask is appied only to the filename.
        /// Directory paths are ignored and all are matched.</param>
        /// <param name="dumpDirectory">The dump directory.</param>
        /// <returns></returns>
        string Dump(string file,
            SrcToolFlags flags = SrcToolFlags.RawSource,
            string limitSourceFiles = null,
            string limitSourceFileNames = null,
            string dumpDirectory = null);

        /// <summary>
        /// Read the indexed source files from the pdb
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        List<string> GetSourceFiles(string file);
    }
}
