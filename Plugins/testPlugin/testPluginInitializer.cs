using System;
using System.Collections.Generic;
using FIVES;
using TerminalPlugin;

namespace testPluginPlugin
{
	public class testPluginInitializer : IPluginInitializer
	{
		Stack<Entity> ownModels = new Stack<Entity>();

		private void PrintCurrentTime (string commandLine)
		{
			String time = System.DateTime.Now.ToString ("HH:mm:ss tt");
			Terminal.Instance.WriteLine ("Current time: " + time);
		}

		private void PrintCommandLine (string commandLine)
		{
			Terminal.Instance.WriteLine (commandLine);
		}

		//string tf2Mash = "resources/models/plane/plane.xml";
		string fireTruckMash = "resources/models/firetruck/xml3d/firetruck.xml";
		string planeMash = "resources/models/plane/plane.xml";

		private void SpawnModel (string path){
			Entity newModel = new Entity();
			newModel["mesh"]["uri"].Suggest(path);
			newModel["mesh"]["visible"].Suggest(true);
			newModel["location"]["position"].Suggest(new Vector(0, 10, 0));
			World.Instance.Add(newModel);
			ownModels.Push(newModel);
		}

		private void DespawnModel (){
			Entity toRemove = ownModels.Pop ();
			toRemove["mesh"]["visible"].Suggest(false);
			//World.Instance.Remove(toRemove);
		}

		private void SpawnModelCommand (string commandLine){
			string[] url = commandLine.Split(new Char [] {' '});
			if (url.Length > 1) {
				SpawnModel (url [1]);
			}
		}

		private void DespawnModelCommand (string commandLine){
			DespawnModel ();
		}

		private void SpawnTruck (string commandLine)
		{
			SpawnModel (fireTruckMash);
		}

		private void SpawnPlane (string commandLine)
		{
			SpawnModel (planeMash);
		}

		public void Initialize ()
		{
			Terminal.Instance.RegisterCommand("currentTime", "Prints the current time (NORBERT)", false,
				PrintCurrentTime, new List<string> { "now" });
			Terminal.Instance.RegisterCommand("spawnTruck", "Spawns an new Truck (NORBERT)", false,
				SpawnTruck, new List<string> { "truck" });
			Terminal.Instance.RegisterCommand("spawnPlane", "Spawns an new Plane (NORBERT)", false,
				SpawnPlane, new List<string> { "plane" });
			Terminal.Instance.RegisterCommand("spawn", "Spawns an new Model with given Name (NORBERT)", false,
				SpawnModelCommand, new List<string> { });
			Terminal.Instance.RegisterCommand("despawn", "despawns the last spawned model (NORBERT)", false,
				DespawnModelCommand, new List<string> { });
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

