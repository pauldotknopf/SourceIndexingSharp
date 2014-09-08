using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SourceIndexingSharp
{
    public class SourceIndexException : ApplicationException
    {
        public SourceIndexException()
        {
        }

        public SourceIndexException(string message)
            : base(message)
        {
        }

        public SourceIndexException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
