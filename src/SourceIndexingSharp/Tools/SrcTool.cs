using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp.Tools
{
    public class SrcTool : ISrcTool
    {
        private readonly Paths _paths;

        public SrcTool(Paths paths)
        {
            _paths = paths;
            _paths.VerifySrcToolPath();
        }

        public string Dump(string file, SrcToolFlags flags = SrcToolFlags.RawSource, string limitSourceFiles = null, string limitSourceFileNames = null, string dumpDirectory = null)
        {
            var fileInfo = new FileInfo(file);

            if(!fileInfo.Exists)
                throw new SourceIndexException("File doesn't exist. " + file);

            var flagParts = new List<string>();

            if(flags.HasFlag(SrcToolFlags.DisplayOnlySourceFilesNotIndexed))
                flagParts.Add("-u");
            if (flags.HasFlag(SrcToolFlags.RawSource))
                flagParts.Add("-r");
            if (flags.HasFlag(SrcToolFlags.RecursiveSubDirectories))
                flagParts.Add("-s");
            if (flags.HasFlag(SrcToolFlags.ExtractFiles))
                flagParts.Add("-x");
            if (flags.HasFlag(SrcToolFlags.ExtractToFlatDirectory))
                flagParts.Add("-f");
            if (flags.HasFlag(SrcToolFlags.ShowVersionControlCommandsWhileExtracting))
                flagParts.Add("-n");
            if (flags.HasFlag(SrcToolFlags.DisplayCount))
                flagParts.Add("-c");

            if (!string.IsNullOrEmpty(limitSourceFiles))
                flagParts.Add(string.Format("-l:\"{0}\"", limitSourceFiles));
            if (!string.IsNullOrEmpty(limitSourceFileNames))
                flagParts.Add(string.Format("-lf:\"{0}\"", limitSourceFileNames));
            if(!string.IsNullOrEmpty(dumpDirectory))
                flagParts.Add(string.Format("-d:\"{0}\"", dumpDirectory));


            flagParts.Add(string.Format("\"{0}\"", file));

            var psi = new ProcessStartInfo(_paths.SrcToolPath)
            {
                WorkingDirectory = fileInfo.Directory.FullName,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                Arguments = string.Join(" ", flagParts),
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            string output;
            string errors;

            using (var p = Process.Start(psi))
            {
                output = p.StandardOutput.ReadToEnd();
                errors = p.StandardError.ReadToEnd();

                p.WaitForExit();
            }

            if(!string.IsNullOrEmpty(errors))
                throw new SourceIndexToolException("SRCTOOL", string.Format("\"{0}\" had an error. {1}", psi.Arguments, errors));

            output = output.TrimEnd(Environment.NewLine.ToCharArray());

            return output;
        }

        /// <summary>
        /// Read the indexed source files from the pdb
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public List<string> GetSourceFiles(string file)
        {
            var output = Dump(file);

            var result = new List<string>();

            foreach (var item in output.Split('\r', '\n'))
            {
                var fileName = item.Trim();

                if (string.IsNullOrEmpty(fileName))
                    continue; // We split on \r and \n

                if ((fileName.IndexOf('*') >= 0) || // C++ Compiler internal file
                    ((fileName.Length > 2) && (fileName.IndexOf(':', 2) >= 0)))
                {
                    // Some compiler internal filenames of C++ start with a * 
                    // and/or have a :123 suffix

                    continue; // Skip never existing files
                }

                fileName = NormalizePath(fileName);

                if(File.Exists(fileName))
                    result.Add(fileName);
            }

            return result;
        }

        private string NormalizePath(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            if (!Path.IsPathRooted(path) || path.Contains(".."))
                path = Path.GetFullPath(path);

            return path;
        }
    }
}
