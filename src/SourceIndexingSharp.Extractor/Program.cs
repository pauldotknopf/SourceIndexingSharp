using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using PowerArgs;

namespace SourceIndexingSharp.Extractor
{
    class Program
    {
        static int Main(string[] args)
        {
#if EMBEDDED
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
#endif
            return Invoke(args);
        }

        private static int Invoke(string[] args)
        {
            try
            {
                Args.SearchAssemblyForRevivers(typeof(Program).Assembly);
                Args.InvokeAction<Actions>(args);
                return 0;
            }
            catch (ArgException ex)
            {
                Console.WriteLine(ex.Message);
                ArgUsage.GetStyledUsage<Actions>().Write();
            }
            catch (TargetInvocationException ex)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return -1;
        }

#if EMBEDDED
        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var resourceName = "SourceIndexingSharp.Extractor.Embedded." + new AssemblyName(args.Name).Name + ".dll";

            foreach (var resourcen in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                Console.WriteLine("Name: " + resourcen);
            }

            Console.WriteLine(resourceName);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if(stream == null)
                    throw new Exception("Can't load embedded assembly " + args.Name);

                var assemblyData = new Byte[stream.Length];

                stream.Read(assemblyData, 0, assemblyData.Length);

                return Assembly.Load(assemblyData);
            }
        }
#endif
    }
}
