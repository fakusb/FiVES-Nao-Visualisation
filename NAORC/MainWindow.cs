using System;
using Gtk;
using RemotePy;
using System.Net;
using System.Net.Sockets;

public partial class MainWindow: Gtk.Window
{
	private PythonConnection commandConnection;
	private System.Net.Sockets.Socket dataConnection;
	private byte[] imageBuffer = new byte[3 * 320 * 240];
	private Gdk.Pixbuf imagePixBuf = null;
	private const double fps = 10.0;

	System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		refreshHandler = new GLib.IdleHandler(refreshCameraImage);
	}

	void connectCallback(IAsyncResult state)
	{
		((System.Net.Sockets.Socket)state.AsyncState).EndConnect(state);
	}

	protected bool refreshCameraImage()
	{
		if (watch.ElapsedMilliseconds < 1000.0 / fps)
			return true;

		watch.Restart();

		commandConnection.executeAsync("sendImage()");
		int level = 0;

		while (level < imageBuffer.Length)
		{
			level += dataConnection.Receive(imageBuffer, level, imageBuffer.Length - level, SocketFlags.None);
		}

		imagePixBuf = new Gdk.Pixbuf(imageBuffer, Gdk.Colorspace.Rgb, false, 8, 320, 240, 3 * 320);

		cameraArea.QueueDraw();

		return true;
	}
		
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		connectionToggle.Active = false;
		Application.Quit ();
		a.RetVal = true;
	}

	protected void say(string msg)
	{
		if (msg.Contains("\""))
			throw new ArgumentException("The message must not contain quotes!");
		commandConnection.execute(String.Format("tts.post.say(\"{0}\")", msg));
	}

	protected void OnTtsButtonClicked (object sender, EventArgs e)
	{
		say(ttsEntry.Text);
	}

	protected void OnCameraAreaExposeEvent (object sender, ExposeEventArgs args)
	{
		if (imagePixBuf == null)
			return;

		DrawingArea area = (DrawingArea)sender;

		using (Cairo.Context cx = Gdk.CairoHelper.Create(area.GdkWindow))
		{
			Gdk.CairoHelper.SetSourcePixbuf(cx, imagePixBuf, 0.0, 0.0);

			cx.Rectangle(0, 0, 320, 240);
			cx.Fill();
		}
	}

	System.Collections.Generic.HashSet<Gdk.Key> pressed = new System.Collections.Generic.HashSet<Gdk.Key>();

	private abstract class Control
	{
		public abstract void Start();
		public abstract void Stop();

		public static string toString(double d)
		{
			return d.ToString(new System.Globalization.CultureInfo("en-US", false));
		}

		public static string toString(int i)
		{
			return i.ToString(new System.Globalization.CultureInfo("en-US", false));
		}
	}

	private class JointControl : Control
	{
		string command;
		PythonConnection connection;
		int currentID = -1;

		public JointControl(PythonConnection connection, string joint, double max, double speed)
		{
			this.connection = connection;
			this.command = String.Format("print startMove(\"{0}\", {1}, {2})", joint, toString(max), toString(speed));
		}

		public override void Start()
		{
			var response = connection.execute(command).Trim();
			try
			{
				currentID = int.Parse(response);
			}
			catch (FormatException)
			{
				currentID = -1;
			}
		}

		public override void Stop()
		{
			if (currentID != -1)
				connection.execute(String.Format("stopMove({0})", toString(currentID)));
		}
	}

	System.Collections.Generic.Dictionary<Gdk.Key, Control> controls = new System.Collections.Generic.Dictionary<Gdk.Key, Control>();



	[GLib.ConnectBefore ()]
	protected void OnKeyPressEvent (object sender, KeyPressEventArgs a)
	{
		var key = a.Event.Key;
		if (!controls.ContainsKey(key))
			return;

		if (pressed.Contains(key))
			return;
		else
			pressed.Add(key);

		controls[key].Start();
	}

	protected void OnKeyReleaseEvent (object sender, KeyReleaseEventArgs a)
	{
		var key = a.Event.Key;
		if (!controls.ContainsKey(key))
			return;

		pressed.Remove(key);

		controls[key].Stop();
	}

	GLib.IdleHandler refreshHandler; 

	protected void OnCameraToggleToggled (object sender, EventArgs e)
	{
		if (cameraToggle.Active)
		{
			commandConnection.execute("enableCamera()");
			GLib.Idle.Add(refreshHandler);
		}
		else
		{
			commandConnection.execute("disableCamera()");
			GLib.Idle.Remove(refreshHandler);
		}
	}

	protected void OnConnectionToggleToggled (object sender, EventArgs e)
	{
		bool active = connectionToggle.Active;

		if (active)
		{
			IPAddress ip = IPAddress.Parse(ipEntry.Text);
			commandConnection = new PythonConnection(ipEntry.Text, userNameEntry.Text, passwordEntry.Text);

			using (var sr = new System.IO.StreamReader("prep_code.py"))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
					commandConnection.execute(line);
			}

			// Open a separate data connection:
			dataConnection = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);
			dataConnection.Connect(new IPEndPoint(ip, 4711));
			commandConnection.execute("connection, _ = sock.accept()");
		}
		else
		{
			controlToggle.Active = false;
			cameraToggle.Active = false;
			commandConnection.execute("close()");
			commandConnection.Dispose();
			dataConnection.Close();
			commandConnection = null;
			dataConnection = null;
		}

		stiffnessToggle.Sensitive = active;
		cameraToggle.Sensitive = active;
		ttsButton.Sensitive = active;
	}

	protected void OnStiffnessToggleToggled (object sender, EventArgs e)
	{
		if (stiffnessToggle.Active)
			commandConnection.execute("enableStiffness()");
		else
		{
			commandConnection.execute("disableStiffness()");
			controlToggle.Active = false;
		}

		controlToggle.Sensitive = stiffnessToggle.Active;
	}

	protected void OnControlToggleToggled (object sender, EventArgs e)
	{
		if (controlToggle.Active)
		{
			controls[Gdk.Key.Left] = new JointControl(this.commandConnection, "HeadYaw", 1.0, 0.1);
			controls[Gdk.Key.Right] = new JointControl(this.commandConnection, "HeadYaw", -1.0, 0.1);
		}
		else
		{
			foreach (var c in controls.Values)
				c.Stop();

			controls.Clear();
		}
	}

}
