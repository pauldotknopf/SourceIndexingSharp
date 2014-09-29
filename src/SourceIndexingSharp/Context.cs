using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceIndexingSharp.Indexing;
using SourceIndexingSharp.Indexing.Stash;
using SourceIndexingSharp.Tools;

namespace SourceIndexingSharp
{
    public static class Context
    {
        internal static readonly TinyIoCContainer Container;

        static Context()
        {
#if EMBEDDED
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
#endif
            Container = new TinyIoCContainer();
            Container.Register<Paths>().AsSingleton();
            Container.Register<ISrcTool, SrcTool>();
            Container.Register<IPdbReaderWriter, PdbReaderWriter>();
            Container.Register<IIndexer, Indexer>();
            Container.Register<IStashApi, StashApi>();
            Container.Register<IStringExpander, StringExpander>();
            Container.Register<IPdbCommandProcessor, PdbCommandProcessor>();
            Container.Register<IPathResolver, PathResolver>();
            Container.Register<ICommandProcessor, CommandProcessor>();
        }

        public static IPdbReaderWriter PdbReaderWriter
        {
            get { return Container.Resolve<IPdbReaderWriter>(); }
        }

        public static IStashApi StashApi
        {
            get { return Container.Resolve<IStashApi>(); }
        }

        public static IPdbCommandProcessor PdbCommandProcessor
        {
            get { return Container.Resolve<IPdbCommandProcessor>(); }
        }

        public static IPathResolver PathResolver
        {
            get { return Container.Resolve<IPathResolver>(); }
        }

        public static IIndexer Indexer
        {
            get { return Container.Resolve<IIndexer>(); }
        }

        public static ISrcTool SrcTool
        {
            get { return Container.Resolve<ISrcTool>(); }
        }

        public static IStringExpander StringExpander
        {
            get { return Container.Resolve<IStringExpander>(); }
        }

        public static ICommandProcessor CommandProcessor
        {
            get { return Container.Resolve<ICommandProcessor>(); }
        }

        public static Func<string> ConsoleExePath = () => "SourceIndexingSharpCons.exe";

#if EMBEDDED
        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var resourceName = "SourceIndexingSharp.Embedded." + new AssemblyName(args.Name).Name + ".dll";

            foreach (var resourcen in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                Console.WriteLine("Name: " + resourcen);
            }

            Console.WriteLine(resourceName);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if(stream == null)
                    return null;

                var assemblyData = new Byte[stream.Length];

                stream.Read(assemblyData, 0, assemblyData.Length);

                return Assembly.Load(assemblyData);
            }
        }
#endif
    }
}
