using System;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Text.RegularExpressions;

namespace sshplayground
{
	class MainClass
	{

		private static SshClient client;
		private static ShellStream sshStream;


		private static Regex shellTerm = new Regex("\\$");
		private static Regex pythonTerm = new Regex("\\>\\>\\>|\\.\\.\\.");
		private static bool shellReady = false;
		private static bool pythonReady = false;


		private static string command(string cmd, Regex terminator, Regex readyTerminator=null)
		{
			if (readyTerminator!=null)
				Console.Write(sshStream.Expect(readyTerminator));

			sshStream.WriteLine(cmd);

			if (terminator == null)
				return null;

			string output = sshStream.Expect(terminator);
			Console.Write(output);

			var s = output.IndexOf('\n');
			var e = output.LastIndexOf('\n');

			output = output.Substring(s + 1, e - s);

			return output;
		}

		private static string shellCommand(string cmd)
		{
			return shellCommand(cmd, shellTerm);
		}

		private static string shellCommand(string cmd, Regex terminator)
		{
			var r = command(cmd, terminator, shellReady ? null : shellTerm);
			shellReady = true;
			return r;
		}

		private static string pythonCommand(string cmd)
		{
			return pythonCommand(cmd, pythonTerm);
		}

		private static string pythonCommand(string cmd, Regex terminator)
		{
			var r = command(cmd, terminator, pythonReady ? null : pythonTerm);
			pythonReady = true;
			return r;
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

			shellCommand("python", null);

			using (var sr = new System.IO.StreamReader("queryJoints.py"))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
					pythonCommand(line);
			}
		}

		private static void queryJoints()
		{
			string line;

			using (var sr = new System.IO.StringReader(pythonCommand("query()")))
				while ((line = sr.ReadLine()) != null)
				{
					var data = line.Trim().Split();
					Console.WriteLine("{0} = {1}", data[0], data[1]);
				}
		}

		public static void Main (string[] args)
		{
			connect("192.168.176.189", "nao", "nao");
			queryJoints();
		}
	}
}
