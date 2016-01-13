using System;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Remote;
using System.Net;
using ccnet.ZiathBuild.plugin;

namespace ccnet.ZiathBuildLabeller.plugin
{

	[ReflectorType("ZiathBuildLabeller")]
	public class ZiathBuildLabeller : ILabeller, ITask
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SvnRevisionLabeller"/> class.
		/// </summary>
		public ZiathBuildLabeller()
		{
		}


		#endregion

		#region Properties

		[ReflectorProperty("component", Required = true)]
		public string Component { get; set; }

        [ReflectorProperty("password", Required = true)]
        public string Password { get; set; }

        #endregion

        #region Methods

        public void Run(IIntegrationResult result)
		{
            Utilities.LogTaskStart(result, "ZiathBuildLabeller");
			result.Label = Generate(result);
            result.Status = IntegrationStatus.Success;
            Utilities.LogTaskEnd(result);
        }
		
		public string Generate(IIntegrationResult resultFromLastBuild)
		{
            string callURL = "http://www.ziath.com/number/serve?component=" + Component + "&password=" + Password;
            string buildnumber = new WebClient().DownloadString(callURL);
            Utilities.LogConsoleAndTask(resultFromLastBuild, "------------START--ZIATH BUILD LABELLER--------------");
            Utilities.LogConsoleAndTask(resultFromLastBuild, "Getting build number from " + callURL);
            Utilities.LogConsoleAndTask(resultFromLastBuild, "Build number is " + buildnumber);
            Utilities.LogConsoleAndTask(resultFromLastBuild, "-------------END---ZIATH BUILD LABELLER--------------");
            return buildnumber;

        }
		#endregion
	}
}