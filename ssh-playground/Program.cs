using System;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace sshplayground
{
	class MainClass
	{

		private static SshClient client;
		private static ShellStream sshStream;


		private static string command(string cmd, int timeout=100)
		{
			sshStream.WriteLine(cmd);
			System.Threading.Thread.Sleep(timeout);
			string s = sshStream.Read();
			Console.Write(s);
			return s;
		}


		private static void connect(string hostName, string userName, string password)
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

			System.Threading.Thread.Sleep(500);
			Console.Write(sshStream.Read());

			command("python");

			using (var sr = new System.IO.StreamReader("queryJoints.py"))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
					command(line);
			}


			/*
			shell = client.CreateShell(input, output, null);

			shell.Start();

			inputWriter.WriteLine("python");

			inputWriter.Flush();

			using (var sr = new System.IO.StreamReader("queryJoints.py"))
			{
				inputWriter.Write(sr.ReadToEnd());
			}

			inputWriter.Flush();

			Console.WriteLine(outputReader.ReadToEnd());
			*/
		}

		private static void queryJoints()
		{
			//input.WriteLine("query()");

			//var data = output.ReadToEnd().Split(null);

			//Console.WriteLine(data);
		}

		public static void Main (string[] args)
		{

			connect("localhost", "nao", "nao");
			//queryJoints();
		}
	}
}
