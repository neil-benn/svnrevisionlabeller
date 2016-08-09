using System;
using Exortech.NetReflector;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core;
using WinSCP;
using System.IO;
using ThoughtWorks.CruiseControl.Remote;
using ccnet.ZiathBuild.plugin;
using ThoughtWorks.CruiseControl.Core.Tasks;

namespace ccnet.ZiathBuildLabeller.plugin
{
    [ReflectorType("ZiathCopyFiles")]
    public class ZiathCopyFiles : TaskBase
    {
        protected override bool Execute(IIntegrationResult result)
        {
            Utilities.LogTaskStart(result, "CopyFiles");
            result.BuildProgressInformation.SignalStartRunTask("Processing copy task");
            Utilities.LogConsoleAndTask(result, "----------------ZIATH COPY FILES START-------------");
            if (!File.Exists(Source))
            {
                Utilities.LogConsoleAndTask(result, "source file " + Source + " does not exist");
                if (!IgnoreNoSource)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            if (File.Exists(Dest) && !Overwrite)
            {
                Utilities.LogConsoleAndTask(result, "dest file " + Dest + " exists and overwrite is set to false");
                return false;
            }
            
            Directory.CreateDirectory(Path.GetDirectoryName(Dest));
            
            File.Copy(Source, Dest, Overwrite);
            Utilities.LogConsoleAndTask(result, "Copied " + Source + " to " + Dest);
            Utilities.LogConsoleAndTask(result, "----------------ZIATH COPY FILES END-------------");
            Utilities.LogTaskEnd(result);
            return true;
            
        }

        #region Properties

        [ReflectorProperty("source", Required = true)]
        public string Source { get; set; }
        [ReflectorProperty("dest", Required = true)]
        public string Dest { get; set; }
        [ReflectorProperty("overwrite", Required = false)]
        public bool Overwrite { get; set; }
        [ReflectorProperty("ignorenosource", Required = false)]
        public bool IgnoreNoSource { get; set; }
        #endregion

    }
}
