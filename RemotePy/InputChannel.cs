using System;
using System.Net;
using System.Net.Sockets;

namespace RemotePy
{
	public class DataConnection : IDisposable
	{
		private Socket dataConnection;

		public DataConnection (ref PythonConnection pyConn, string ipAddress, int port)
		{
			IPAddress ip = IPAddress.Parse(ipAddress);

			using (var sr = new System.IO.StreamReader("setup_socket.py"))
			{
				String line;
				while ((line = sr.ReadLine ()) != null) {
					if (line.Contains ("sock_port = "))
						line = String.Format ("sock_port = {0}", port);
					pyConn.execute (line);
				}
			}

			dataConnection = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
			dataConnection.Connect(new IPEndPoint(ip, port));
			pyConn.execute("connection, _ = sock.accept()");
		}

		public void Receive(ref Byte[] dest, int numBytes) {
			int level = 0;

			while (level < numBytes)
			{
				level += dataConnection.Receive(dest, level, numBytes - level, SocketFlags.None);
			}
		}


		public void Dispose ()
		{
			dataConnection.Close ();
			dataConnection = null;
		}

	}
}

