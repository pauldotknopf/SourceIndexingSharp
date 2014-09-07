using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PowerArgs;

namespace SourceIndexingSharp.Extractor
{
    public class Actions
    {
        [ArgActionMethod]
        public void Test(TestArgs args)
        {
            if(File.Exists(args.OutputFileLocation))
                File.Delete(args.OutputFileLocation);
            File.WriteAllText(args.OutputFileLocation, "TESTFILE: " + args.OutputFileLocation.ToLower());
        }

        public class TestArgs
        {
            public string OutputFileLocation { get; set; }
        }
    }
}
