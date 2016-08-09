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
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Core.Tasks;

namespace ccnet.ZiathBuildLabeller.plugin
{
    [ReflectorType("ZiathMoveFiles")]
    public class ZiathMoveFiles : TaskBase
    {
        protected override bool Execute(IIntegrationResult result)
        {
            Utilities.LogTaskStart(result, "MoveFiles");
            result.BuildProgressInformation.SignalStartRunTask("Processing move task");
            Utilities.LogConsoleAndTask(result, "----------------ZIATH MOVE FILES START-------------");
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
            
            if (File.Exists(Dest))
            {
                Utilities.LogConsoleAndTask(result, "Deleted " + Dest);
                File.Delete(Dest);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(Dest));
            File.Move(Source, Dest);
            Utilities.LogConsoleAndTask(result, "Moved " + Source + " to " + Dest);
            Utilities.LogConsoleAndTask(result, "----------------ZIATH MOVE FILES END-------------");
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
