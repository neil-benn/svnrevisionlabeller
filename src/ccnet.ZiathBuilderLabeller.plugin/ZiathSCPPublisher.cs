using System;
using Exortech.NetReflector;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core;
using WinSCP;
using ThoughtWorks.CruiseControl.Remote;
using ccnet.ZiathBuild.plugin;
using System.IO;
using ThoughtWorks.CruiseControl.Core.Tasks;

namespace ccnet.ZiathBuildLabeller.plugin
{
    [ReflectorType("ZiathSCPPublisher")]
    public class ZiathSCPPublisher : TaskBase
    {
        protected override bool Execute(IIntegrationResult result)
        {
            Utilities.LogTaskStart(result, "ZiathSCPPublisher");
            Utilities.LogConsoleAndTask(result, ("-------SCP Publisher Start-----"));
            // Setup session options
            SessionOptions sessionOptions = new SessionOptions();

            sessionOptions.Protocol = Protocol.Scp;
            sessionOptions.HostName = Server;
            sessionOptions.UserName = Username;
            sessionOptions.Password = Password;
            sessionOptions.SshHostKeyFingerprint = SSHKeygen;

            Session session = new Session();
            session.Open(sessionOptions);
            // Upload files
            TransferOptions transferOptions = new TransferOptions();
            transferOptions.TransferMode = TransferMode.Binary;
            Utilities.LogConsoleAndTask(result, "Hostname : " + Server);
            Utilities.LogConsoleAndTask(result, "Username : " + Username);
            Utilities.LogConsoleAndTask(result, "LocalFile : " + LocalFile);
            Utilities.LogConsoleAndTask(result, "RemoteDirectory : " + RemoteDirectory);
            
            string directory = Path.GetDirectoryName(LocalFile);
            Console.WriteLine("directory is " + directory);
            string filename = Path.GetFileName(LocalFile);
            Console.WriteLine("filename is " + filename);
            Boolean failed = false;
            foreach (string sf in Directory.GetFiles(directory, filename))
            {
                TransferOperationResult transferResult;
                Utilities.LogConsoleAndTask(result, "processing " + sf);
                transferResult = session.PutFiles(sf, RemoteDirectory + "/", false, transferOptions);

                // Throw on any error
                transferResult.Check();
                if (!transferResult.IsSuccess)
                {
                    foreach (Exception e in transferResult.Failures)
                    {
                        Utilities.LogConsoleAndTask(result, e.Message);
                    }
                    result.Status = IntegrationStatus.Failure;
                    failed = true;
                }
                else
                {
                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Utilities.LogConsoleAndTask(result, string.Format("Upload of {0} succeeded", transfer.FileName));
                    }
                }
            }

            Utilities.LogTaskEnd(result);
            return !failed;
        }

        #region Properties

        [ReflectorProperty("ssh-keygen", Required = true)]
        public string SSHKeygen { get; set; }

        [ReflectorProperty("localfile", Required = true)]
        public string LocalFile { get; set; }

        [ReflectorProperty("remotedirectory", Required = true)]
        public string RemoteDirectory { get; set; }

        [ReflectorProperty("server", Required = true)]
        public string Server { get; set; }

        [ReflectorProperty("username", Required = true)]
        public string Username { get; set; }

        [ReflectorProperty("password", Required = true)]
        public string Password { get; set; }

        #endregion

    }
}
