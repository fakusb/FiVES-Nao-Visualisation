using System;
using System.Collections.Generic;
using FIVES;

namespace testPluginPlugin
{
	public class testPluginInitializer : IPluginInitializer
	{
		public void Initialize ()
		{
			System.Console.Error.WriteLine ("test");
			//throw new NotImplementedException ();
		}

		public void Shutdown ()
		{
			//throw new NotImplementedException ();
		}

		public string Name {
			get {
				return "testPluginNorbert";
			}
		}

		public List<string> PluginDependencies {
			get {
				return new List<string> ();
			}
		}

		public List<string> ComponentDependencies {
			get {
				return new List<string> ();
			}
		}
	}
}

