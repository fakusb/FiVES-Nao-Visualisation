using System;
using System.Collections.Generic;
using FIVES;
using ClientManagerPlugin;
using EventLoopPlugin;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Text.RegularExpressions;
using KIARA;
using KIARAPlugin;
using TerminalPlugin;

namespace NorbertPlugin
{
	public class NorbertPluginInitializer : IPluginInitializer
	{	
		// Stores all the robot entities:
		private List<Entity> entities = new List<Entity>();

		// SSH infrastructure:
		private SshClient client;
		private ShellStream sshStream;
		private Regex shellTerm = new Regex("\\$");
		private Regex pythonTerm = new Regex("\\>\\>\\>|\\.\\.\\.");
		private bool shellReady = false;
		private bool pythonReady = false;

	
		// Our component definition:
		private ComponentDefinition posture = new ComponentDefinition("nao_posture");


		public void Initialize ()
		{
			Terminal.Instance.RegisterCommand("nao", "Adds an entity for a NAO robot", false,
				AddEntity);

			DefineComponents();
			RegisterEvents();
		}

		public void Shutdown ()
		{
			//TODO Close the connection properly!
		}

		public string Name {
			get {
				return "Norbert";
			}
		}

		public List<string> PluginDependencies {
			get {
				return new List<string> {"KIARA", "ClientManager", "EventLoop", "Terminal"};
			}
		}

		public List<string> ComponentDependencies {
			get {
				return new List<string> ();
			}
		}

		internal void DefineComponents()
		{
			posture.AddAttribute<double>("HeadYaw", 0.0);
			posture.AddAttribute<double>("HeadPitch", 0.0);
			posture.AddAttribute<double>("RShoulderPitch", 0.0);
			posture.AddAttribute<double>("RShoulderRoll", -0.5);
			posture.AddAttribute<double>("RElbowRoll", 0.5);
			posture.AddAttribute<double>("RElbowYaw", 0.0);
			posture.AddAttribute<double>("RWristYaw", 0.0);
			posture.AddAttribute<double>("RHand", 0.0);

			posture.AddAttribute<double>("LShoulderPitch", 0.0);
			posture.AddAttribute<double>("LShoulderRoll", -0.5);
			posture.AddAttribute<double>("LElbowRoll", 0.5);
			posture.AddAttribute<double>("LElbowYaw", 0.0);
			posture.AddAttribute<double>("LWristYaw", 0.0);
			posture.AddAttribute<double>("LHand", 0.0);

			posture.AddAttribute<double>("RHipYawPitch", 0.0);
			posture.AddAttribute<double>("RHipPitch", 0.0);
			posture.AddAttribute<double>("RHipRoll", 0.0);
			posture.AddAttribute<double>("RKneePitch", 0.0);
			posture.AddAttribute<double>("RAnklePitch", 0.0);
			posture.AddAttribute<double>("RAnkleRoll", 0.0);

			posture.AddAttribute<double>("LHipYawPitch", 0.0);
			posture.AddAttribute<double>("LHipPitch", 0.0);
			posture.AddAttribute<double>("LHipRoll", 0.0);
			posture.AddAttribute<double>("LKneePitch", 0.0);
			posture.AddAttribute<double>("LAnklePitch", 0.0);
			posture.AddAttribute<double>("LAnkleRoll", 0.0);

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
				CheckAndRegisterEntity(entity);
		}

		private string command(string cmd, Regex terminator, Regex readyTerminator=null)
		{
			if (readyTerminator!=null)
				sshStream.Expect(readyTerminator);

			sshStream.WriteLine(cmd);

			if (terminator == null)
				return null;

			string output = sshStream.Expect(terminator);

			var s = output.IndexOf('\n');
			var e = output.LastIndexOf('\n');

			output = output.Substring(s + 1, e - s);

			return output;
		}

		private string shellCommand(string cmd)
		{
			return shellCommand(cmd, shellTerm);
		}

		private string shellCommand(string cmd, Regex terminator)
		{
			var r = command(cmd, terminator, shellReady ? null : shellTerm);
			shellReady = true;
			return r;
		}

		private string pythonCommand(string cmd)
		{
			return pythonCommand(cmd, pythonTerm);
		}

		private string pythonCommand(string cmd, Regex terminator)
		{
			var r = command(cmd, terminator, pythonReady ? null : pythonTerm);
			pythonReady = true;
			return r;
		}

		private void connect(string hostName, string userName, string password)
		{
			if (client != null && client.IsConnected)
				return;


			var connectionInfo = new KeyboardInteractiveConnectionInfo(hostName, userName);
			connectionInfo.AuthenticationPrompt += delegate(object sender, AuthenticationPromptEventArgs e)
			{
				foreach (var prompt in e.Prompts)
					prompt.Response = password;
			};

			client = new SshClient(connectionInfo);
			client.Connect();

			sshStream = client.CreateShellStream("", 80, 40, 80, 40, 1024);

			shellCommand("python", null);

			using (var sr = new System.IO.StreamReader("queryJoints.py"))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
					pythonCommand(line);
			}
		}

		private void queryJoints()
		{
			string line;

			using (var sr = new System.IO.StringReader(pythonCommand("query()")))
				while ((line = sr.ReadLine()) != null)
				{
					var data = line.Trim().Split();
					var key = data[0];
					var val = double.Parse(data[1]);

					foreach (Entity entity in entities)
						entity["nao_posture"][key].Suggest(val);

					Console.WriteLine("{0} = {1}", key, val);
				}
		}

		/// <summary>
		/// Handles a TickFired Evenet of EventLoop. Queries the robot for its posture.
		/// </summary>
		/// <param name="sender">Sender of tick event args (EventLoop)</param>
		/// <param name="e">TickEventArgs</param>
		private void HandleEventTick(Object sender, TickEventArgs e)
		{
			if (client != null && client.IsConnected)
				queryJoints();
		}
	
		void HandleAddedEntity (object sender, EntityEventArgs e)
		{
			CheckAndRegisterEntity(e.Entity);
		}

		void CheckAndRegisterEntity(Entity entity)
		{
			if (entity.ContainsComponent("nao_posture"))
				entities.Add(entity);
		}

		void AddEntity(string commandLine)
		{
			var data = commandLine.Split();
			AddEntity(data[1], data[2], data[3]);
		}

		void AddEntity(string hostName, string userName, string password)
		{
			connect(hostName, userName, password);
			var e = new Entity();

			World.Instance.Add(e);
			CheckAndRegisterEntity(e);
		}

	}
}

