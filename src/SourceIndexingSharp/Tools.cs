using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp
{
    public class Paths
    {
        public string SrcToolPath { get; set; }

        public void VerifySrcToolPath()
        {
            if(!File.Exists(SrcToolPath))
                throw new SourceIndexToolException("SRCTOOL", string.Format("File doesn't exist at \"{0}\".", SrcToolPath));
        }

        public string PdbStrPath { get; set; }

        public void VerifyPdbStrPath()
        {
            if (!File.Exists(PdbStrPath))
                throw new SourceIndexToolException("PDBSTR", string.Format("File doesn't exist at \"{0}\".", PdbStrPath));
        }
    }
}
