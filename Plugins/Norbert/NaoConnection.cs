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
	public class NaoConnection : IDisposable
	{	
		// SSH infrastructure:
		private SshClient client;
		private ShellStream sshStream;
		private Regex shellTerm = new Regex("\\$");
		private Regex pythonTerm = new Regex("\\>\\>\\>|\\.\\.\\.");
		private bool shellReady = false;
		private bool pythonReady = false;

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


		public NaoConnection(string hostName, string userName, string password)
		{
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

		public Dictionary<string, double> QueryJoints()
		{
			string line;

			Dictionary<string, double> queryResult = new Dictionary<string, double>();

			using (var sr = new System.IO.StringReader(pythonCommand("query()")))
				while ((line = sr.ReadLine()) != null)
				{
					var data = line.Trim().Split();
					var key = data[0];
					var val = double.Parse(data[1]);

					queryResult[key] = val;
				}

			return queryResult;
		}

		public void Dispose()
		{
			sshStream.Dispose();
			client.Disconnect();
			client.Dispose();
		}
	}
}

