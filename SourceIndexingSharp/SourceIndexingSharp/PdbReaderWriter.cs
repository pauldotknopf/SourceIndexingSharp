using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp
{
    public class PdbReaderWriter : IPdbReaderWriter
    {
        private readonly Tools _tools;

        public PdbReaderWriter(Tools tools)
        {
            _tools = tools;
            _tools.VerifyPdbStrPath();
        }

        public void WritePdb(string pdbFile, Action<StreamWriter> writerAction, string streamName = "srcsrv")
        {
            var tmpFile = Path.GetFullPath(Path.GetTempFileName());
            try
            {
                using (var sw = File.CreateText(tmpFile))
                    writerAction(sw);

                var psi = new ProcessStartInfo(_tools.PdbStrPath)
                {
                    Arguments = string.Format("-w -s:{0} -p:\"{1}\" -i:\"{2}\"", streamName, pdbFile, tmpFile),
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
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
                File.Delete(tmpFile);
            }
        }

        public string ReadPdb(string pdbFile, string streamName = "srcsrv")
        {
            var psi = new ProcessStartInfo(_tools.PdbStrPath)
            {
                WorkingDirectory = new FileInfo(pdbFile).Directory.FullName,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                Arguments = string.Format("-r -s:{0} -p:\"{1}\"", streamName, pdbFile)
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
