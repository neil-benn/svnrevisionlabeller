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
    [ReflectorType("ZiathDeleteFiles")]
    public class ZiathDeleteFiles : ITask
    {
        public void Run(IIntegrationResult result)
        {
            try
            {
                Utilities.LogTaskStart(result, "ZiathDeleteFiles");
                result.BuildProgressInformation.SignalStartRunTask("Processing delete task");
                Utilities.LogConsoleAndTask(result, "----------------ZIATH DELETE FILES START-------------");
                Utilities.LogConsoleAndTask(result, "----------------DELETE DIRECTORIES START-------------");
                if (Directories != null)
                {
                    foreach (String d in Directories)
                    {

                        String sDir = d.Trim();
                        if (!String.IsNullOrEmpty(sDir))
                        {
                            if (Directory.Exists(sDir))
                            {
                                try
                                {
                                    Directory.Delete(sDir, true);
                                    Utilities.LogConsoleAndTask(result, string.Format("Deleted directory {0}", sDir));
                                }
                                catch (Exception)
                                {
                                    Utilities.LogConsoleAndTask(result, string.Format("Failed to delete directory {0}", sDir));
                                }
                            }
                            else
                            {
                                Utilities.LogConsoleAndTask(result, string.Format("Failed to delete directory {0}, it doesn't exist", sDir));
                            }
                        }
                        else
                        {
                            Utilities.LogConsoleAndTask(result, string.Format("Empty dir in delete directory", sDir));
                        }
                    }
                }
                Utilities.LogConsoleAndTask(result, "----------------DELETE DIRECTORIES END-------------");
                Utilities.LogConsoleAndTask(result, "----------------DELETE FILES START-------------");
                if (Files != null)
                {

                    foreach (string s in Files)
                    {
                        String sFile = s.Trim();
                        if (!String.IsNullOrEmpty(sFile))
                        {
                            string directory = Path.GetDirectoryName(sFile);
                            Utilities.LogConsoleAndTask(result, "directory is " + directory);
                            string filename = Path.GetFileName(sFile);
                            Utilities.LogConsoleAndTask(result, "filename is " + filename);
                            foreach (string sf in Directory.GetFiles(directory, filename))
                            {
                                Utilities.LogConsoleAndTask(result, "processing " + sf);
                                if (File.Exists(sf))
                                {
                                    try
                                    {
                                        File.Delete(sf);
                                        Utilities.LogConsoleAndTask(result, string.Format("Deleted file {0}", sf));
                                    }
                                    catch (Exception)
                                    {
                                        Utilities.LogConsoleAndTask(result, string.Format("Failed to delete file {0}", sf));
                                    }
                                }
                                else
                                {
                                    Utilities.LogConsoleAndTask(result, string.Format("Failed to delete file {0}, it doesn't exist", sf));
                                }
                            }
                        }
                        else
                        {
                            Utilities.LogConsoleAndTask(result, string.Format("Empty filename in delete", sFile));
                        }
                    }
                }
                Utilities.LogConsoleAndTask(result, "----------------DELETE FILES END-------------");
                result.Status = IntegrationStatus.Success;
            }
            finally
            {
                Utilities.LogTaskEnd(result);
            }
        }

        #region Properties

        [ReflectorArray("files", Required = false)]
        public string[] Files { get; set; }
        [ReflectorArray("directories", Required = false)]
        public string[] Directories { get; set; }
        #endregion

    }
}
