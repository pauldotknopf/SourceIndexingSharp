using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceIndexingSharp.Indexing;
using SourceIndexingSharp.Tools;

namespace SourceIndexingSharp
{
    public static class Context
    {
        private static readonly TinyIoCContainer Container;

        static Context()
        {
            Container = new TinyIoCContainer();
            Container.Register<Paths>().AsSingleton();
            Container.Register<ISrcTool, SrcTool>();
            Container.Register<IPdbReaderWriter, PdbReaderWriter>();
            Container.Register<IIndexer, Indexer>();
        }

        public static IPdbReaderWriter PdbReaderWriter
        {
            get { return Container.Resolve<IPdbReaderWriter>(); }
        }
    }
}
