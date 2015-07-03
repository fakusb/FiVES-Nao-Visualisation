using System;
using System.Collections.Generic;
using FIVES;
using ClientManagerPlugin;
using EventLoopPlugin;
using Renci.SshNet;
using KIARA;
using KIARAPlugin;


namespace NorbertPlugin
{
	public class NorbertPluginInitializer : IPluginInitializer
	{	
		// Stores all the robot entities:
		private List<Entity> entities = new List<Entity>();

		// Connection to the robot:
		private SshClient client;
		private System.IO.MemoryStream input = new System.IO.MemoryStream();
		private System.IO.MemoryStream output = new System.IO.MemoryStream();
		private System.IO.TextWriter inputWriter;
		private System.IO.TextReader outputReader;
		private Shell shell;

		public void Initialize ()
		{
			inputWriter = new System.IO.StreamWriter(input);
			outputReader = new System.IO.StreamReader(output);
			// TODO: Remove this debug output!
			System.Console.Error.WriteLine ("Call me Norbert. MuHAHA!");
			DefineComponents();
			RegisterEvents();
		}

		public void Shutdown ()
		{
			if (client != null)
			{
				shell.Stop();
				client.Disconnect();
				client.Dispose();
				client = null;
			}
		}

		public string Name {
			get {
				return "Norbert";
			}
		}

		public List<string> PluginDependencies {
			get {
				return new List<string> {"KIARA", "ClientManager", "EventLoop"};
			}
		}

		public List<string> ComponentDependencies {
			get {
				return new List<string> ();
			}
		}

		internal void DefineComponents()
		{
			ComponentDefinition posture = new ComponentDefinition("nao_posture");
			posture.AddAttribute<double>("HeadYaw", 0);
			posture.AddAttribute<double>("HeadPitch", 0);
			posture.AddAttribute<double>("RShoulderPitch", 0);
			posture.AddAttribute<double>("RShoulderRoll", -0.5);
			posture.AddAttribute<double>("RElbowRoll", 0.5);
			posture.AddAttribute<double>("RElbowYaw", 0);
			posture.AddAttribute<double>("RWristYaw", 0);
			posture.AddAttribute<double>("RHand", 0);

			posture.AddAttribute<double>("LShoulderPitch", 0);
			posture.AddAttribute<double>("LShoulderRoll", -0.5);
			posture.AddAttribute<double>("LElbowRoll", 0.5);
			posture.AddAttribute<double>("LElbowYaw", 0);
			posture.AddAttribute<double>("LWristYaw", 0);
			posture.AddAttribute<double>("LHand", 0);

			posture.AddAttribute<double>("RHipYawPitch", 0);
			posture.AddAttribute<double>("RHipPitch", 0);
			posture.AddAttribute<double>("RHipRoll", 0);
			posture.AddAttribute<double>("RKneePitch", 0);
			posture.AddAttribute<double>("RAnklePitch", 0);
			posture.AddAttribute<double>("RAnkleRoll", 0);

			posture.AddAttribute<double>("LHipYawPitch", 0);
			posture.AddAttribute<double>("LHipPitch", 0);
			posture.AddAttribute<double>("LHipRoll", 0);
			posture.AddAttribute<double>("LKneePitch", 0);
			posture.AddAttribute<double>("LAnklePitch", 0);
			posture.AddAttribute<double>("LAnkleRoll", 0);

			ComponentRegistry.Instance.Register(posture);
		}

		/// <summary>
		/// Subscribes to EventLoop events
		/// </summary>
		private void RegisterEvents()
		{
			EventLoop.Instance.TickFired += new EventHandler<TickEventArgs>(HandleEventTick);


			World.Instance.AddedEntity += HandleAddedEntity;

			foreach (var entity in World.Instance)
				CheckAndRegisterAvatarEntity(entity);
		}

		private void connect(string hostName, string userName, string password)
		{
			if (client != null && client.IsConnected)
				return;

			client = new SshClient(new PasswordConnectionInfo(hostName, userName, password));
			shell = client.CreateShell(input, output, null);

			shell.Start();

			inputWriter.WriteLine("python");

			using (var sr = new System.IO.StreamReader("queryJoints.py"))
			{
				inputWriter.Write(sr.ReadToEnd());
			}

			outputReader.ReadToEnd();
		}

		private void queryJoints()
		{
			inputWriter.WriteLine("query()");

			var data = outputReader.ReadToEnd().Split(null);

			for (int i = 0; i < data.Length;)
			{
				var key = data[i++];
				var val = data[i++];

				foreach (Entity entity in entities)
					entity["nao_posture"][key].Suggest(val);
			}

		}

		/// <summary>
		/// Handles a TickFired Evenet of EventLoop. Queries the robot for its posture.
		/// </summary>
		/// <param name="sender">Sender of tick event args (EventLoop)</param>
		/// <param name="e">TickEventArgs</param>
		private void HandleEventTick(Object sender, TickEventArgs e)
		{
			System.Console.WriteLine("Querying robot posture.");

			connect("192.168.176.121", "nao", "nao");
			queryJoints();
		}


		void HandleAddedEntity (object sender, EntityEventArgs e)
		{
			CheckAndRegisterAvatarEntity(e.Entity);
		}

		bool CheckAndRegisterAvatarEntity(Entity entity)
		{
			if (entity.ContainsComponent("nao_posture"))
				entities.Add(entity);
		}
	}
}

