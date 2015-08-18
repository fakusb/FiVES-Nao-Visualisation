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
using System.Threading;
namespace NorbertPlugin
{
	public class NaoConnection : PythonConnection
	{	
		private static readonly Dictionary<byte, string> jointDict = new Dictionary<byte, string>{
			{0, "HeadYaw"},
			{1, "HeadPitch"},
			{2, "RShoulderPitch"},
			{3, "RShoulderRoll"},
			{4, "RElbowRoll"},
			{5, "RElbowYaw"},
			{6, "RWristYaw"},
			{7, "RHand"},
			{8, "LShoulderPitch"},
			{9, "LShoulderRoll"},
			{10, "LElbowRoll"},
			{11, "LElbowYaw"},
			{12, "LWristYaw"},
			{13, "LHand"},
			{14, "RHipYawPitch"},
			{15, "RHipPitch"},
			{16, "RHipRoll"},
			{17, "RKneePitch"},
			{18, "RAnklePitch"},
			{19, "RAnkleRoll"},
			{20, "LHipYawPitch"},
			{21, "LHipPitch"},
			{22, "LHipRoll"},
			{23, "LKneePitch"},
			{24, "LAnklePitch"},
			{25, "LAnkleRoll"}
		};

		private bool running = false;
		private string ip;

		private InputChannel dc;

		public Dictionary<string, float> jointState = new Dictionary<string, float>();

		public NaoConnection(string hostName, string userName, string password) : base(hostName, userName, password)
		{
			ip = hostName;
			using (var sr = new System.IO.StreamReader("queryJoints.py"))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
					execute(line);
			}
		}

		public void Start()
		{
			dc = new InputChannel (this, ip, 4712);
			StartDaemon ();
			execute ("rt.start()");
			running = true;
		}

//		public void QueryJoints()
//		{
//			string line;
//			using (var sr = new System.IO.StringReader(execute("query()")))
//				while ((line = sr.ReadLine()) != null)
//				{
//					var data = line.Trim().Split();
//					var key = data[0];
//					var val = double.Parse(data[1]);
//					lock (this)
//						jointState [key] = val;
//				}
//			lock (this)
//				jointState ["RHipYawPitch"] = -jointState ["LHipYawPitch"];
//		}

		Thread daemon;

		private void StartDaemon()
		{
			daemon = new Thread (new ThreadStart (DaemonFunction));
			daemon.Start();
		}

		private void DaemonFunction()
		{
			byte[] buf = new byte[5];
			while (true) {
				dc.Receive (ref buf, 5);
				string joint = jointDict [buf [0]];
				float val = BitConverter.ToSingle(buf, 1);
				
				lock (this) {
					jointState [joint] = val;
				}
			}
		}

		private void StopDaemon()
		{
			daemon.Abort ();
		}

		public new void Dispose()
		{
			if (running) {
				execute ("rt.stop()");
				StopDaemon ();
			}

			if (dc != null)
				dc.Dispose ();
			base.Dispose ();
		}
	}
}

