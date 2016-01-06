using Exortech.NetReflector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Tasks;
using ThoughtWorks.CruiseControl.Remote;

namespace ccnet.ZiathBuild.plugin
{
    [ReflectorType("ZiathExecuteNSIS")]
    public class ZiathExecuteNSIS : BaseExecutableTask
    {

        protected override string GetProcessFilename()
        {
            string makensisExe = NSISExeFilepath;
            if (!makensisExe.EndsWith("makensis.exe"))
            {
                makensisExe = Path.Combine(NSISExeFilepath, "makensis.exe");
            }
            
            return makensisExe;
        }

        protected override ProcessPriorityClass GetProcessPriorityClass()
        {
            return ProcessPriorityClass.Normal;
        }

        protected override int GetProcessTimeout()
        {
            return 300;
        }

        protected override string GetProcessArguments(IIntegrationResult result)
        {
            string gitsha = Utilities.GetGitSHA(result.WorkingDirectory);
            string args = string.Format("/DBUILDNUMBER={1} /DGITSHA={2}", NSIFile, result.Label, gitsha);
            foreach (string dprop in NSISDProps)
            {
                args += " " + dprop;
                Console.WriteLine("D-PROP is " + dprop);
            }
            args += " \"" + NSIFile + "\"";
            return args;
        }

        protected override string GetProcessBaseDirectory(IIntegrationResult result)
        {
            return BaseDirectory;
        }


        protected override bool Execute(IIntegrationResult result)
        {
            Utilities.LogConsoleAndTask(result, "Process filename : " + GetProcessFilename());
            Utilities.LogConsoleAndTask(result, "Process Arguments : " + GetProcessArguments(result));
            Utilities.LogConsoleAndTask(result, "Process WD : " + GetProcessBaseDirectory(result));
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = GetProcessBaseDirectory(result),
                    FileName = GetProcessFilename(),
                    Arguments = GetProcessArguments(result),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream)
            {
                Console.WriteLine(proc.StandardOutput.ReadLine() + "\n");
            }
            return proc.ExitCode == 0;
        }

        #region Properties

        [ReflectorProperty("nsi-file", Required = true)]
        public string NSIFile { get; set; }

        [ReflectorProperty("nsis-exe-filepath", Required = true)]
        public string NSISExeFilepath { get; set; }

        [ReflectorProperty("base-directory", Required = true)]
        public string BaseDirectory { get; set; }

        [ReflectorArray("nsis-d-props", Required = false)]
        public string[] NSISDProps { get; set; }
        #endregion Properties
    }
}
