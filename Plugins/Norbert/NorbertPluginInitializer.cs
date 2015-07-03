using System;
using System.Collections.Generic;
using FIVES;
using EventLoopPlugin;

namespace NorbertPlugin
{
	public class NorbertPluginInitializer : IPluginInitializer
	{
		public void Initialize ()
		{
			System.Console.Error.WriteLine ("Call me Norbert. MuHAHA!");
			DefineComponents();
			RegisterToPluginEvents();
		}

		public void Shutdown ()
		{
		
		}

		public string Name {
			get {
				return "Norbert";
			}
		}

		public List<string> PluginDependencies {
			get {
				return new List<string> {"EventLoop"};
			}
		}

		public List<string> ComponentDependencies {
			get {
				return new List<string> ();
			}
		}

		internal void DefineComponents()
		{
			// so far we're not defining anything.
		}

		/// <summary>
		/// Subscribes to EventLoop events
		/// </summary>
		private void RegisterToPluginEvents()
		{
			//EventLoop.Instance.TickFired += new EventHandler<TickEventArgs>(HandleEventTick);
		}

		/// <summary>
		/// Handles a TickFired Evenet of EventLoop. Queries the robot for its posture.
		/// </summary>
		/// <param name="sender">Sender of tick event args (EventLoop)</param>
		/// <param name="e">TickEventArgs</param>
		private void HandleEventTick(Object sender, TickEventArgs e)
		{
			System.Console.WriteLine("Querying robot posture.");
		}
	}
}

