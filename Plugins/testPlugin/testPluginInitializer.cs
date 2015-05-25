using System;
using System.Collections.Generic;
using FIVES;
using TerminalPlugin;

namespace testPluginPlugin
{
	public class testPluginInitializer : IPluginInitializer
	{
		HashSet<Entity> testTrucks = new HashSet<Entity>();

		private void PrintCurrentTime (string commandLine)
		{
			String time = System.DateTime.Now.ToString ("HH:mm:ss tt");
			Terminal.Instance.WriteLine ("Current time: " + time);
		}

		private void PrintCommandLine (string commandLine)
		{
			Terminal.Instance.WriteLine (commandLine);
		}

		string planeMash = "resources/models/plane/plane.xml";
		string fireTruckMash = "resources/models/firetruck/xml3d/firetruck.xml";
		private void SpawnTruck (string commandLine)
		{
			Entity newTruck = new Entity();
			newTruck["mesh"]["uri"].Suggest(fireTruckMash);
			//newTruck["mesh"]["uri"].Suggest(commandLine.Equals("alarm")?fireTruckMash : planeMash);
			newTruck["mesh"]["visible"].Suggest(true);
			Random random = new Random();
			int randomNumber = random.Next(-10, 10);
			newTruck["location"]["position"].Suggest(new Vector(randomNumber, 10, 0));
			World.Instance.Add(newTruck);
			testTrucks.Add (newTruck);
		}

		public void Initialize ()
		{
			Terminal.Instance.RegisterCommand("currentTime", "Prints the current time (NORBERT)", false,
				PrintCurrentTime, new List<string> { "now" });
			Terminal.Instance.RegisterCommand("spawnTruck", "Spawns an new Truck (NORBERT)", false,
				SpawnTruck, new List<string> { "alarm" });
			Terminal.Instance.RegisterCommand("commandLineArgument", "Prints the argument (NORBERT)", false,
				PrintCommandLine, new List<string> { "arg" });
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

