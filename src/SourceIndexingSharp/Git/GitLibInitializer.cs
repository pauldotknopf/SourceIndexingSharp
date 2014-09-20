using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SourceIndexingSharp.Git
{
    public class GitLibInitializer
    {
        static readonly object InitLock = new object();
        static bool _isInitialized;

        public static void EnsureInitialized()
        {
            if(_isInitialized) return;

            lock (InitLock)
            {
                if(_isInitialized) return;

                _isInitialized = true;

                LoadNativeGitDll();
            }
        }

        private static string GetNativeGitDllName()
        {
            var nativeDllNameType = Type.GetType("LibGit2Sharp.Core.NativeDllName, LibGit2Sharp");

            if(nativeDllNameType == null)
                throw new Exception("Couldn't reflect on the internal LibGit2Sharp class to get the name of the lib2git dll we need.");

            var nameField = nativeDllNameType.GetField("Name");

            if(nameField == null)
                throw new Exception("Couldn't get the reference to the constant 'Name' to get the name of the lib2git dll we need.");

            return (string)nameField.GetValue(null);
        }

        private static void LoadNativeGitDll()
        {
            var gitDllName = GetNativeGitDllName();

            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            var dirName = Path.Combine(Path.GetTempPath(), assemblyName.Name + "." + Assembly.GetExecutingAssembly().GetName().Version);
            
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);

            var dllPath = Path.Combine(dirName, gitDllName + ".dll");

            var streamLocation = string.Format("SourceIndexingSharp.Git.Embedded.{0}.{1}.dll",
                IntPtr.Size == 4 ? "x86" : "x64", gitDllName);

            if (!File.Exists(dllPath))
            {
                using (var stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(streamLocation))
                {
                    if (stm == null)
                        throw new Exception("Couldn't find the embedded native dll at embedded location " +
                                            streamLocation);

                    try
                    {
                        using (Stream outFile = File.Create(dllPath))
                        {
                            const int sz = 4096;
                            var buf = new byte[sz];
                            while (true)
                            {
                                int nRead = stm.Read(buf, 0, sz);
                                if (nRead < 1)
                                    break;
                                outFile.Write(buf, 0, nRead);
                            }
                        }
                    }
                        // ReSharper disable EmptyGeneralCatchClause
                    catch
                        // ReSharper restore EmptyGeneralCatchClause
                    {
                        // This may happen if another process has already created and loaded the file.
                        // Since the directory includes the version number of this assembly we can
                        // assume that it's the same bits, so we just ignore the excecption here and
                        // load the DLL.
                    }
                }
            }

            var h = LoadLibrary(dllPath);

            if(h == IntPtr.Zero)
                throw new Exception("Unable to load library " + dllPath);
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr LoadLibrary(string lpFileName);
    }
}
