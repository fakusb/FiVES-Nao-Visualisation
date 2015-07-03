using System;
using Renci.SshNet;

namespace sshplayground
{
	class MainClass
	{

		private static SshClient client;
		private static System.IO.MemoryStream input = new System.IO.MemoryStream();
		private static System.IO.MemoryStream output = new System.IO.MemoryStream();
		private static System.IO.TextWriter inputWriter;
		private static System.IO.TextReader outputReader;
		private static Shell shell;

		private static void connect(string hostName, string userName, string password)
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

		private static void queryJoints()
		{
			inputWriter.WriteLine("query()");

			var data = outputReader.ReadToEnd().Split(null);

			Console.WriteLine(data);
		}

		public static void Main (string[] args)
		{
			inputWriter = new System.IO.StreamWriter(input);
			outputReader = new System.IO.StreamReader(output);

			connect("localhost", "nao", "nao");
			queryJoints();
		}
	}
}
