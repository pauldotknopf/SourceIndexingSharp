using System;
using System.Diagnostics;
using System.IO;

namespace SourceIndexingSharp.Tools
{
    public class PdbReaderWriter : IPdbReaderWriter
    {
        private readonly Paths _paths;

        public PdbReaderWriter(Paths paths)
        {
            _paths = paths;
            _paths.VerifyPdbStrPath();
        }

        public void WritePdb(string pdbFile, Action<StreamWriter> writerAction, string streamName = "srcsrv")
        {
            if(!File.Exists(pdbFile))
                throw new SourceIndexException("File doesn't exist. " + pdbFile);

            var tmpFile = Path.GetFullPath(Path.GetTempFileName());

            try
            {
                using (var sw = File.CreateText(tmpFile))
                    writerAction(sw);

                var psi = new ProcessStartInfo(_paths.PdbStrPath)
                {
                    Arguments = string.Format("-w -s:{0} -p:\"{1}\" -i:\"{2}\"", streamName, pdbFile, tmpFile),
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                using (var p = Process.Start(psi))
                {
                    p.StandardOutput.ReadToEnd();
                    p.StandardError.ReadToEnd();

                    p.WaitForExit();
                }
            }
            finally
            {
                if(File.Exists(tmpFile))
                    File.Delete(tmpFile);
            }
        }

        public string ReadPdb(string pdbFile, string streamName = "srcsrv")
        {
            var fileInfo = new FileInfo(pdbFile);

            if (!fileInfo.Exists)
                throw new SourceIndexException("File doesn't exist. " + pdbFile);

            var psi = new ProcessStartInfo(_paths.PdbStrPath)
            {
                WorkingDirectory = fileInfo.Directory.FullName,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                Arguments = string.Format("-r -s:{0} -p:\"{1}\"", streamName, pdbFile),
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

            if (!string.IsNullOrEmpty(errors))
                throw new SourceIndexToolException("SRCTOOL", errors.Trim());

            output = output.TrimEnd(Environment.NewLine.ToCharArray());

            if (output == "pdbstr -r/w -p:PdbFileName -i:StreamFileName -s:StreamName")
                throw new SourceIndexToolException("PDBSTR", "Invalid arguements. " + psi.Arguments);

            return output;
        }
    }
}
