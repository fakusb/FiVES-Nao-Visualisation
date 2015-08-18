using System;
using Gtk;
using RemotePy;

public partial class MainWindow: Gtk.Window
{
	private PythonConnection commandConnection;
	private InputChannel videoChannel;
	private const int cam_width = 320;
	private const int cam_height = 240;

	private byte[] imageBuffer = new byte[3 * cam_width * cam_height];
	private Gdk.Pixbuf imagePixBuf = null;
	private const double fps = 10.0;

	System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		refreshHandler = new GLib.IdleHandler(refreshCameraImage);
	}

	protected bool refreshCameraImage()
	{
		if (watch.ElapsedMilliseconds < 1000.0 / fps)
			return true;

		watch.Restart();

		commandConnection.executeAsync("sendImage()");
		videoChannel.Receive (ref imageBuffer, imageBuffer.Length);

		imagePixBuf = new Gdk.Pixbuf(imageBuffer, Gdk.Colorspace.Rgb, false, 8, cam_width, cam_height, 3 * cam_width);

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
			var x = (area.Allocation.Width - cam_width) / 2;
			var y = (area.Allocation.Height - cam_height) / 2;
			Gdk.CairoHelper.SetSourcePixbuf(cx, imagePixBuf, x, y);
			cx.Rectangle(x, y, cam_width, cam_height);
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
			commandConnection.execute("imgClient = video.subscribe(\"_client\", resolution, colorSpace, 5)");
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
			commandConnection = new PythonConnection(ipEntry.Text, userNameEntry.Text, passwordEntry.Text);

			// Open a separate data connection:
			// Has to be done before sending prep_code.py as it defines necessary variables for prep_code.py
			videoChannel = new InputChannel (commandConnection, ipEntry.Text, 4711);

			using (var sr = new System.IO.StreamReader("prep_code.py"))
			{
				String line;
				while ((line = sr.ReadLine()) != null)
					commandConnection.execute(line);
			}
		}
		else
		{
			controlToggle.Active = false;
			cameraToggle.Active = false;
			commandConnection.execute("close()");
			commandConnection.Dispose();
			videoChannel.Dispose();
			commandConnection = null;
			videoChannel = null;
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
