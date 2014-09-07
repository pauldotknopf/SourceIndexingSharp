using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SourceIndexingSharp.Tests
{
    public class TestBase
    {
        // ReSharper disable InconsistentNaming
        protected Tools _tools;
        // ReSharper restore InconsistentNaming

        [SetUp]
        public virtual void Setup()
        {
            _tools = new Tools
            {
                SrcToolPath = @"C:\Program Files (x86)\Windows Kits\8.1\Debuggers\x86\srcsrv\srctool.exe",
                PdbStrPath = @"C:\Program Files (x86)\Windows Kits\8.1\Debuggers\x86\srcsrv\pdbstr.exe"
            };
        }
    }
}
