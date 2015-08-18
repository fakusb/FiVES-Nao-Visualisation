using System;
using System.Collections.Generic;
using FIVES;
using ClientManagerPlugin;
using EventLoopPlugin;
using KIARA;
using KIARAPlugin;
using TerminalPlugin;
using System.Threading;

namespace NorbertPlugin
{
	public class NorbertPluginInitializer : IPluginInitializer
	{	
		private ReaderWriterLock entRWL = new ReaderWriterLock();
		// Stores all the robot entities:
		private Dictionary<Entity, NaoConnection> entities = new Dictionary<Entity, NaoConnection>();
	
		// Our component definition:
		private ComponentDefinition posture = new ComponentDefinition("nao_posture");


		public void Initialize ()
		{
			Terminal.Instance.RegisterCommand("nao", "Adds an entity for a NAO robot", false,
				AddEntity);

			DefineComponents();
			RegisterEvents();
//			StartDaemon();
		}

		public void Shutdown ()
		{
//			StopDaemon();
			entRWL.AcquireWriterLock(-1);
			try {
				foreach (NaoConnection nc in entities.Values)
					nc.Dispose();
			} finally {
				entRWL.ReleaseWriterLock ();
			}
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
		}
			
		private void HandleEventTick(Object sender, TickEventArgs e)
		{

			entRWL.AcquireReaderLock (-1);
			try {
				foreach (KeyValuePair<Entity, NaoConnection> ec in entities)
				{
					var entity = ec.Key;
					var connection = ec.Value;
					lock (connection)
					{
						foreach (KeyValuePair<string, float> kv in connection.jointState)
							entity["nao_posture"][kv.Key].Suggest(kv.Value);

						connection.jointState.Clear();
					}
				}
			} finally {
				entRWL.ReleaseReaderLock ();
			}
//			doUpdate.Set();
		}

		void AddEntity(string commandLine)
		{
			var data = commandLine.Split();
			AddEntity(data[1], data[2], data[3]);
		}

		static int XoffsetBase = 0;

		void AddEntity(string hostName, string userName, string password)
		{
			var e = new Entity();

			e["mesh"]["uri"].Suggest("resources/models/v11/nao.xml");
			e["mesh"]["visible"].Suggest(true);
			e["location"]["position"].Suggest(new Vector(XoffsetBase, 10, 0));
			e["location"]["orientation"].Suggest(new Quat((float)3.0, (float)3.0, (float)3.0, (float)10 ));

			XoffsetBase += 5;

			World.Instance.Add(e);

			try {
				Terminal.Instance.WriteLine("Connecting to " + userName +"@"+ hostName + "...");

				var connection = new NaoConnection(hostName, userName, password);
				entRWL.AcquireWriterLock(-1);
				try {
					entities[e] = connection;
					connection.Start();
				} finally {
					entRWL.ReleaseWriterLock();
				}
				Terminal.Instance.WriteLine("\tConnected :-)");
			}
			catch (Exception ex) {
				Terminal.Instance.WriteLine("\tFailed: " + ex.Message);

			}
		}
	}
}

