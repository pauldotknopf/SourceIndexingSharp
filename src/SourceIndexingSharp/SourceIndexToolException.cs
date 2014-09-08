using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SourceIndexingSharp
{
    public class SourceIndexToolException : SourceIndexException
    {
        public SourceIndexToolException()
        {
        }

        public SourceIndexToolException(string toolname, string message)
            : base(string.Format("{0} failed: {1}", toolname, message))
        {
        }

        public SourceIndexToolException(string toolname, string message, Exception inner)
            : base(string.Format("{0} failed: {1}", toolname, message), inner)
        {
        }
    }
}
