using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ccnet.ZiathBuild.plugin
{
    public class Utilities
    {
        /// <summary>
        /// Return teh SHA checksum oif the git checkin
        /// </summary>
        /// <param name="path">the path to he root of the git project</param>
        /// <returns>a string representing the SHA</returns>
        public static string GetGitSHA(string gitProjectPath)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = gitProjectPath,
                    FileName = "git",
                    Arguments = " rev-parse --short HEAD",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            string line = "";
            while (!proc.StandardOutput.EndOfStream)
            {
                line = proc.StandardOutput.ReadLine();
            }
            return line;
        }


    }
}
