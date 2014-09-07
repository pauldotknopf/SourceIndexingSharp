using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp
{
    public static class Context
    {
        private static readonly TinyIoCContainer Container;

        static Context()
        {
            Container = new TinyIoCContainer();
            Container.Register<Tools>();
        }

        public static IPdbReaderWriter PdbReaderWriter
        {
            get { return Container.Resolve<IPdbReaderWriter>(); }
        }
    }
}
