using System;
using System.Collections.Generic;
using FIVES;
using EventLoopPlugin;
using Renci.SshNet;

namespace NorbertPlugin
{
	public class NorbertPluginInitializer : IPluginInitializer
	{
		private SshClient client;
		private System.IO.MemoryStream input = new System.IO.MemoryStream();
		private System.IO.MemoryStream output = new System.IO.MemoryStream();
		private System.IO.TextWriter inputWriter = new System.IO.TextWriter(input);
		private System.IO.TextReader outputReader = new System.IO.TextReader(output);

		private Shell shell;

		public void Initialize ()
		{
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
			ComponentDefinition posture = new ComponentDefinition("nao_posture");
			location.AddAttribute<double>("HeadYaw", 0);
			location.AddAttribute<double>("HeadPitch", 0);
			location.AddAttribute<double>("RShoulderPitch", 0);
			location.AddAttribute<double>("RShoulderRoll", -0.5);
			location.AddAttribute<double>("RElbowRoll", 0.5);
			location.AddAttribute<double>("RElbowYaw", 0);
			location.AddAttribute<double>("RWristYaw", 0);
			location.AddAttribute<double>("RHand", 0);

			location.AddAttribute<double>("LShoulderPitch", 0);
			location.AddAttribute<double>("LShoulderRoll", -0.5);
			location.AddAttribute<double>("LElbowRoll", 0.5);
			location.AddAttribute<double>("LElbowYaw", 0);
			location.AddAttribute<double>("LWristYaw", 0);
			location.AddAttribute<double>("LHand", 0);

			location.AddAttribute<double>("RHipYawPitch", 0);
			location.AddAttribute<double>("RHipPitch", 0);
			location.AddAttribute<double>("RHipRoll", 0);
			location.AddAttribute<double>("RKneePitch", 0);
			location.AddAttribute<double>("RAnklePitch", 0);
			location.AddAttribute<double>("RAnkleRoll", 0);

			location.AddAttribute<double>("LHipYawPitch", 0);
			location.AddAttribute<double>("LHipPitch", 0);
			location.AddAttribute<double>("LHipRoll", 0);
			location.AddAttribute<double>("LKneePitch", 0);
			location.AddAttribute<double>("LAnklePitch", 0);
			location.AddAttribute<double>("LAnkleRoll", 0);

			ComponentRegistry.Instance.Register(posture);
		}

		/// <summary>
		/// Subscribes to EventLoop events
		/// </summary>
		private void RegisterEvents()
		{
			EventLoop.Instance.TickFired += new EventHandler<TickEventArgs>(HandleEventTick);


			ClientManager.Instance.NotifyWhenAnyClientAuthenticated(delegate(Connection connection)
				{
					Activate(connection);
					connection.Closed += (sender, e) => Deactivate(connection);
				});

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

			using (var sr = new System.IO.TextReader("queryJoints.py"))
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

				foreach (Entity e in entities.Values)
					e["nao_posture"][key].Suggest(val);
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

			connect("192.168.176.34", "nao", "nao");
			queryJoints();
		}


		void HandleAddedEntity (object sender, EntityEventArgs e)
		{
			if (!CheckAndRegisterAvatarEntity(e.Entity))
				e.Entity.CreatedComponent += HandleCreatedComponent;
		}

		void HandleCreatedComponent(object sender, ComponentEventArgs e)
		{
			if (e.Component.Name == "nao_posture")
				e.Component.ChangedAttribute += HandleChangedAvatarComponent;
		}

		void HandleChangedAvatarComponent(object sender, ChangedAttributeEventArgs e)
		{
			if (e.AttributeName == "userLogin")
			{
				if (e.OldValue != null && avatarEntities.ContainsKey((string)e.OldValue))
					entities.Remove((string)e.OldValue);
				entities[(string)e.NewValue] = e.Entity;
			}
		}

		bool CheckAndRegisterAvatarEntity(Entity entity)
		{
			if (entity.ContainsComponent("nao_posture"))
			{
				entities[(string)entity["avatar"]["userLogin"].Value] = entity;
				return true;
			}

			return false;
		}

		Dictionary<string, Entity> entities = new Dictionary<string, Entity>();

	}
}

