using System;
using System.Collections.Generic;
using FIVES;
using ClientManagerPlugin;
using EventLoopPlugin;
using RemotePy;
using System.Text.RegularExpressions;
using KIARA;
using KIARAPlugin;
using TerminalPlugin;

namespace NorbertPlugin
{
	public class NaoConnection : PythonConnection
	{	

		Dictionary<string, double> jointState = new Dictionary<string, double>();

		public NaoConnection(string hostName, string userName, string password) : base(hostName, userName, password)
		{
			using (var sr = new System.IO.StreamReader("queryJoints.py"))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
					execute(line);
			}
		}

		public Dictionary<string, double> QueryJoints(bool updatesOnly=false)
		{
			string line;

			Dictionary<string, double> queryResult = new Dictionary<string, double>();

			using (var sr = new System.IO.StringReader(execute("query()")))
				while ((line = sr.ReadLine()) != null)
				{
					var data = line.Trim().Split();
					var key = data[0];
					var val = double.Parse(data[1]);

					double current;
					if (!updatesOnly || !jointState.TryGetValue (key, out current) || current != val) {

						queryResult [key] = val;
					}
					jointState[key] = val;
				}


			jointState ["RHipYawPitch"] = -jointState ["LHipYawPitch"];
			if (queryResult.ContainsKey ("LHipYawPitch") || queryResult.ContainsKey ("RHipYawPitch"))
				queryResult ["RHipYawPitch"] = jointState ["RHipYawPitch"];


			return queryResult;
		}

	}
}

