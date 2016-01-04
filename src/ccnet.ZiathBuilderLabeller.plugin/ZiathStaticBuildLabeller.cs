using System;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Remote;
using System.Net;

namespace ccnet.ZiathBuildLabeller.plugin
{

	[ReflectorType("ZiathStaticBuildLabeller")]
	public class ZiathStaticBuildLabeller : ILabeller, ITask
	{

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SvnRevisionLabeller"/> class.
		/// </summary>
		public ZiathStaticBuildLabeller()
		{
		}


		#endregion

		#region Properties

		[ReflectorProperty("label", Required = true)]
		public string Label { get; set; }

        #endregion

        #region Methods

        public void Run(IIntegrationResult result)
		{
			result.Label = Generate(result);
            result.Status = IntegrationStatus.Success;
        }
		
		public string Generate(IIntegrationResult resultFromLastBuild)
		{
            Console.WriteLine("------------START--ZIATH STATIC BUILD LABELLER--------------");
            Console.WriteLine("Build number is " + Label);
            Console.WriteLine("-------------END---ZIATH BUILD LABELLER--------------");
            return Label;

        }
		#endregion
	}
}