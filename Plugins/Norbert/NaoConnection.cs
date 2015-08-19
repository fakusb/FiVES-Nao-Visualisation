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
using System.Diagnostics;

namespace NorbertPlugin
{
	public struct RobotPosition {
		public float XPosition;
		public float YPosition;
		public float Orientation;
		public float XAngle;
		public float YAngle;
	};

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
			{25, "LAnkleRoll"},
			{26, "XPosition"},
			{27, "YPosition"},
			{28, "Orientation"}
		};

		private bool running = false;
		private string ip;

		private InputChannel dc;

		public Dictionary<string, float> jointState = new Dictionary<string, float>();

		public RobotPosition positionState;

		public NaoConnection(string hostName, string userName, string password) : base(hostName, userName, password)
		{
			ip = hostName;
			dc = new InputChannel (this, ip, 4712);
			using (var sr = new System.IO.StreamReader("queryJoints.py"))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
					execute(line);
			}
		}

		public void Start()
		{
			StartDaemon ();
			execute ("rt.start()");
			running = true;
		}

		Thread daemon;

		private void StartDaemon()
		{
			daemon = new Thread (new ThreadStart (DaemonFunction));
			daemon.Start();
		}

		private void DaemonFunction()
		{
			byte[] buf = new byte[5];

//			Stopwatch sw = Stopwatch.StartNew();

			while (true) {
				dc.Receive (ref buf, 5);
				byte idx = buf [0];
				float val = BitConverter.ToSingle(buf, 1);

				switch (idx) {
				case 26:
					lock (this)
						positionState.XPosition = val;
					break;
				case 27:
					lock (this)
						positionState.YPosition = val;
					break;
				case 28:
					lock (this)
						positionState.Orientation = val;
					break;
				case 29:
					lock (this)
						positionState.XAngle = val;
					break;
				case 30:
					lock (this)
						positionState.YAngle = val;
					break;
				default:
					string name = jointDict [idx];
					lock (this)
						jointState [name] = val;
					break;
				}

//				Console.WriteLine(String.Format("Network read took {0}ms.", sw.ElapsedMilliseconds));
//				sw.Restart();
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

