using System;
using Exortech.NetReflector;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core;
using WinSCP;
using ThoughtWorks.CruiseControl.Remote;

namespace ccnet.ZiathBuildLabeller.plugin
{
    [ReflectorType("ZiathSCPPublisher")]
    public class ZiathSCPPublisher : ITask
    {
        public void Run(IIntegrationResult result)
        {
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

            TransferOperationResult transferResult;
            transferResult = session.PutFiles(LocalFile, RemoteDirectory + "/", false, transferOptions);

            // Throw on any error
            transferResult.Check();

            // Print results
            foreach (TransferEventArgs transfer in transferResult.Transfers)
            {
                Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
            }
            result.Status = IntegrationStatus.Success;
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
