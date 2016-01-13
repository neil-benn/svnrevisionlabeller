using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core;

namespace ccnet.ZiathBuild.plugin
{
    public class Utilities
    {
        public static void LogTaskStart(IIntegrationResult result, String taskName)
        {
            result.AddTaskResult("<buildresults>\r\n");
            result.AddTaskResult(string.Format("\t<task name=\"{0}\"/>\r\n", taskName));
        }

        public static void LogTaskEnd(IIntegrationResult result)
        {
            result.AddTaskResult("\t<\task>\r\n");
            result.AddTaskResult("</buildresults>\r\n");
        }
        /// <summary>
        /// Prints a message both teh cruisecontrol server and the consol
        /// </summary>
        /// <param name="result">a reference to the cruise control server to print to</param>
        /// <param name="message">the message to write</param>
        public static void LogConsoleAndTask(IIntegrationResult result, String message)
        {
            result.AddTaskResult("\t\t<message>" + message + "</message>\r\n");
            Console.WriteLine(message);
        }
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
