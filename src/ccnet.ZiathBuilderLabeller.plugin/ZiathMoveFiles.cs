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

namespace ccnet.ZiathBuildLabeller.plugin
{
    [ReflectorType("ZiathMoveFiles")]
    public class ZiathMoveFiles : ITask
    {
        public void Run(IIntegrationResult result)
        {
            Utilities.LogTaskStart(result, "MoveFiles");
            result.BuildProgressInformation.SignalStartRunTask("Processing delete task");
            Utilities.LogConsoleAndTask(result, "----------------ZIATH MOVE FILES START-------------");
            if (!File.Exists(Source))
            {
                Utilities.LogConsoleAndTask(result, "source file " + Source + " does not exist");
                if (!IgnoreNoSource)
                {
                    result.Status = IntegrationStatus.Failure;
                }
                else
                {
                    result.Status = IntegrationStatus.Success;
                }
                
                return;
            }
            if (File.Exists(Dest) && !Overwrite)
            {
                Utilities.LogConsoleAndTask(result, "dest file " + Dest + " exists and overwrite is set to false");
                result.Status = IntegrationStatus.Failure;
                return;
            }
            
            if (File.Exists(Dest))
            {
                Utilities.LogConsoleAndTask(result, "Deleted " + Dest);
                File.Delete(Dest);
            }
            File.Move(Source, Dest);
            Utilities.LogConsoleAndTask(result, "Moved " + Source + " to " + Dest);
            Utilities.LogConsoleAndTask(result, "----------------ZIATH MOVE FILES END-------------");
            result.Status = IntegrationStatus.Success;
            Utilities.LogTaskEnd(result);
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
