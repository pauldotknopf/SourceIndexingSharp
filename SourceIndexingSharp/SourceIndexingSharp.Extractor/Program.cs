using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using PowerArgs;

namespace SourceIndexingSharp.Extractor
{
    class Program
    {
        static int Main(string[] args)
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
    }
}
