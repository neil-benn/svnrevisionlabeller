using System;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Remote;
using System.Net;

namespace CcNet.Labeller
{

	[ReflectorType("ZiathBuildLabeller")]
	public class ZiathBuildLabeller : ILabeller, ITask
	{

        private string component;
        private string password;

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
			result.Label = Generate(result);
		}
		
		public string Generate(IIntegrationResult resultFromLastBuild)
		{
            string callURL = "http://www.ziath.com/number/serve?component=" + Component + "&password=" + Password;
            string buildnumber = new WebClient().DownloadString(callURL);
            Console.WriteLine("------------START--ZIATH BUILD LABELLER--------------");
            Console.WriteLine("Getting build number from " + callURL);
            Console.WriteLine("Build number is " + buildnumber);
            Console.WriteLine("-------------END---ZIATH BUILD LABELLER--------------");
            return buildnumber;

        }
		#endregion
	}
}