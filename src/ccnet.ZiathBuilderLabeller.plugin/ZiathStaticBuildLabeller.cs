using System;
using Exortech.NetReflector;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Remote;
using System.Net;
using ccnet.ZiathBuild.plugin;
using ThoughtWorks.CruiseControl.Core.Tasks;

namespace ccnet.ZiathBuildLabeller.plugin
{

	[ReflectorType("ZiathStaticBuildLabeller")]
	public class ZiathStaticBuildLabeller : TaskBase, ILabeller
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

        protected override bool Execute(IIntegrationResult result)
		{
            Utilities.LogTaskStart(result, "ZiathStaticBuildLabeller");
			result.Label = Generate(result);
            result.Status = IntegrationStatus.Success;
            Utilities.LogTaskEnd(result);
            return true;
        }
		
		public string Generate(IIntegrationResult resultFromLastBuild)
		{
            Utilities.LogConsoleAndTask(resultFromLastBuild, "------------START--ZIATH STATIC BUILD LABELLER--------------");
            Utilities.LogConsoleAndTask(resultFromLastBuild, "Build number is " + Label);
            Utilities.LogConsoleAndTask(resultFromLastBuild, "-------------END---ZIATH BUILD LABELLER--------------");
            return Label;

        }
		#endregion
	}
}