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


    [ReflectorType("DeleteDirectory")]
    public class DeleteDirectory
    {
        [ReflectorProperty("path", Required = true)]
        public String Path { get; set; }
        [ReflectorProperty("recursive", Required = false)]
        public bool Recursive { get; set; }
        [ReflectorProperty("exclude", Required = false)]
        public string Exclude { get; set; }
    }



    [ReflectorType("ZiathDeleteFiles")]
    public class ZiathDeleteFiles : ITask
    {
        private bool IsExcluded(string path)
        {
            if (Exclusions == null) return false;
            foreach (string testDir in Exclusions)
            {
                if (path.StartsWith(testDir, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        private string CombineWithBaseDir(string d)
        {
            if (!d.Equals(Path.GetFullPath(d)))
            {
                if (!String.IsNullOrEmpty(BaseDir))
                {
                    d = Path.Combine(BaseDir, d);
                }
            }
            return d;
        }

        private void PrintExclusions(IIntegrationResult result)
        {
            if (Exclusions != null)
            {
                foreach (string exclusion in Exclusions)
                {
                    Utilities.LogConsoleAndTask(result, "Exclusion - " + exclusion);
                }
            }
        }
        public void Run(IIntegrationResult result)
        {
            try
            {
                Utilities.LogTaskStart(result, "ZiathDeleteFiles");
                result.BuildProgressInformation.SignalStartRunTask("Processing delete task");
                PrintExclusions(result);
                Utilities.LogConsoleAndTask(result, "----------------DELETE DIRECTORIES START-------------");
                if (Directories != null)
                {
                    foreach (DeleteDirectory dd in Directories)
                    {
                        String d = CombineWithBaseDir(dd.Path);

                        String sDir = d.Trim();
                        String workingBaseDir = BaseDir != null ? BaseDir : ".";
                        string[] delDirs = null;

                        if (dd.Recursive)
                        {
                            Utilities.LogConsoleAndTask(result, string.Format("Processing recursive from base dir {0} looking for {1}", workingBaseDir, dd.Path));
                            delDirs = Directory.GetDirectories(workingBaseDir, dd.Path, SearchOption.AllDirectories);
                        }
                        else
                        {
                            Utilities.LogConsoleAndTask(result, "Not Processing recursive");
                            delDirs = new string[] { Path.Combine(BaseDir, dd.Path) };
                        }
                        foreach (string processDir in delDirs)
                        {
                            if (IsExcluded(processDir))
                            {
                                Utilities.LogConsoleAndTask(result, string.Format("Excluding {0}", processDir));
                                continue;
                            }
                            if (!String.IsNullOrEmpty(processDir))
                            {
                                if (Directory.Exists(processDir))
                                {
                                    try
                                    {
                                        Directory.Delete(processDir, true);
                                        Utilities.LogConsoleAndTask(result, string.Format("Deleted directory {0}", processDir));
                                    }
                                    catch (Exception)
                                    {
                                        Utilities.LogConsoleAndTask(result, string.Format("Failed to delete directory {0}", processDir));
                                    }
                                }
                                else
                                {
                                    Utilities.LogConsoleAndTask(result, string.Format("Failed to delete directory {0}, it doesn't exist", processDir));
                                }
                            }
                            else
                            {
                                Utilities.LogConsoleAndTask(result, string.Format("Empty dir in delete directory", processDir));
                            }
                        }
                    }
                }
                Utilities.LogConsoleAndTask(result, "----------------DELETE DIRECTORIES END-------------");
                Utilities.LogConsoleAndTask(result, "----------------DELETE FILES START-------------");
                if (Files != null)
                {

                    foreach (string s in Files)
                    {
                        String sFile = CombineWithBaseDir(s.Trim());
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
            catch (Exception e)
            {
                Console.Error.WriteLine(e.ToString());
                result.Status = IntegrationStatus.Exception;
            }
            finally
            {
                Utilities.LogTaskEnd(result);
            }
        }

        #region Properties

        [ReflectorProperty("BaseDir", Required=false)]
        public string BaseDir { get; set; }
        [ReflectorArray("files", Required = false)]
        public string[] Files { get; set; }
        [ReflectorArray("directories", Required = false)]
        public DeleteDirectory[] Directories { get; set; }
        [ReflectorArray("exclusions", Required = false)]
        public string[] Exclusions { get; set; }
        #endregion

    }
}
