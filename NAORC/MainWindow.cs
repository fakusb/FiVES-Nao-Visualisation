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
			if (currentID != -1)
				return;

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
			{
				connection.execute(String.Format("stopMove({0})", toString(currentID)));
				currentID = -1;
			}
		}
	}

	private class WalkControl : Control
	{
		PythonConnection connection;

		private static double x_speed = 0.0;
		private static double stepAngle = 0.0;
		private static double frequency = 0.0;

		private double deltaSpeed;
		private double deltaAngle;
		private double deltaFrequency;
		private bool active = false;

		public WalkControl(PythonConnection connection, double deltaSpeed, double deltaAngle, double delta_frequency)
		{
			this.connection = connection;
			this.deltaSpeed = deltaSpeed;
			this.deltaAngle = deltaAngle;
			this.deltaFrequency = deltaFrequency;
		}

		public override void Start()
		{	
			if (active)
				return;

			active = true;

			WalkControl.x_speed += this.deltaSpeed;
			WalkControl.stepAngle += this.deltaAngle;
			WalkControl.frequency += this.deltaFrequency;

			var command = String.Format("motion.setWalkArmsEnabled(True, True)\nmotion.setWalkTargetVelocity({0}, 0, {1}, {2})", toString(WalkControl.x_speed), toString(WalkControl.stepAngle), toString(WalkControl.frequency));

			connection.execute(command);
		}

		public override void Stop()
		{
			if (!active)
				return;

			active = false;

			WalkControl.x_speed -= this.deltaSpeed;
			WalkControl.stepAngle -= this.deltaAngle;
			WalkControl.frequency -= this.deltaFrequency;

			var command = String.Format("motion.setWalkArmsEnabled(True, True)\nmotion.setWalkTargetVelocity({0}, 0, {1}, {2})", toString(WalkControl.x_speed), toString(WalkControl.stepAngle), toString(WalkControl.frequency));

			connection.execute(command);
		}
	}

	System.Collections.Generic.HashSet<Gdk.Key> pressed = new System.Collections.Generic.HashSet<Gdk.Key>();

	System.Collections.Generic.Dictionary<Gdk.Key, Control> shiftlessControls = new System.Collections.Generic.Dictionary<Gdk.Key, Control>();
	System.Collections.Generic.Dictionary<Gdk.Key, Control> shiftedControls = new System.Collections.Generic.Dictionary<Gdk.Key, Control>();

	System.Collections.Generic.Dictionary<Gdk.Key, Control> selectedControls;

	[GLib.ConnectBefore ()]
	protected void OnKeyPressEvent (object sender, KeyPressEventArgs a)
	{
		if (selectedControls == null)
			return;

		var key = a.Event.Key;

		if (pressed.Contains(key))
			return;

		if (key == Gdk.Key.Shift_L)
		{
			selectedControls = shiftedControls;
			foreach (var ctrl in shiftlessControls)
			{
				if (pressed.Contains(ctrl.Key))
				{
					ctrl.Value.Stop();

					if (shiftedControls.ContainsKey(ctrl.Key))
						shiftedControls[ctrl.Key].Start();
				}
			}
		}
		else
		{
			if (!selectedControls.ContainsKey(key))
				return;

			pressed.Add(key);

			selectedControls[key].Start();
		}
	}

	protected void OnKeyReleaseEvent (object sender, KeyReleaseEventArgs a)
	{
		var key = a.Event.Key;

		if (key == Gdk.Key.Shift_L)
		{	
			selectedControls = shiftlessControls;
			foreach (var ctrl in shiftedControls)
			{
				if (pressed.Contains(ctrl.Key))
				{
					ctrl.Value.Stop();

					if (shiftlessControls.ContainsKey(ctrl.Key))
						shiftlessControls[ctrl.Key].Start();
				}
			}
		}
		else
		{
			if (shiftlessControls.ContainsKey(key))
				shiftlessControls[key].Stop();
			if (shiftedControls.ContainsKey(key))
				shiftedControls[key].Stop();
			if (pressed.Contains(key))
				pressed.Remove(key);
		}
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

		ipEntry.Sensitive = !active;
		userNameEntry.Sensitive = !active;
		passwordEntry.Sensitive = !active;

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
			shiftedControls[Gdk.Key.Left] = new JointControl(this.commandConnection, "HeadYaw", 1.0, 0.15);
			shiftedControls[Gdk.Key.Right] = new JointControl(this.commandConnection, "HeadYaw", -1.0, 0.15);
			shiftedControls[Gdk.Key.Up] = new JointControl(this.commandConnection, "HeadPitch", -0.7, 0.1);
			shiftedControls[Gdk.Key.Down] = new JointControl(this.commandConnection, "HeadPitch", 0.5, 0.1);

			shiftlessControls[Gdk.Key.Left] = new WalkControl(this.commandConnection, 0.0, 0.75, 0.0);
			shiftlessControls[Gdk.Key.Right] = new WalkControl(this.commandConnection, 0.0, -0.75, 0.0);
			shiftlessControls[Gdk.Key.Up] = new WalkControl(this.commandConnection, 1.0, 0.0, 1.0);
			shiftlessControls[Gdk.Key.Down] = new WalkControl(this.commandConnection, -0.5, 0.0, 0.5);

			shiftlessControls[Gdk.Key.c] = new JointControl(this.commandConnection, "LShoulderPitch", 1.57, 0.1);
			shiftlessControls[Gdk.Key.v] = new JointControl(this.commandConnection, "LShoulderPitch", 0, 0.1);
			shiftlessControls[Gdk.Key.d] = new JointControl(this.commandConnection, "LElbowRoll", 0, 0.1);
			shiftlessControls[Gdk.Key.f] = new JointControl(this.commandConnection, "LElbowRoll", -1.57, 0.1);
			shiftlessControls[Gdk.Key.e] = new JointControl(this.commandConnection, "LHand", 0, 0.25);
			shiftlessControls[Gdk.Key.r] = new JointControl(this.commandConnection, "LHand", 1, 0.25);

			shiftlessControls[Gdk.Key.m] = new JointControl(this.commandConnection, "RShoulderPitch", 1.57, 0.1);
			shiftlessControls[Gdk.Key.n] = new JointControl(this.commandConnection, "RShoulderPitch", 0, 0.1);
			shiftlessControls[Gdk.Key.k] = new JointControl(this.commandConnection, "RElbowRoll", 0, 0.1);
			shiftlessControls[Gdk.Key.j] = new JointControl(this.commandConnection, "RElbowRoll", 1.57, 0.1);
			shiftlessControls[Gdk.Key.o] = new JointControl(this.commandConnection, "RHand", 0, 0.25);
			shiftlessControls[Gdk.Key.i] = new JointControl(this.commandConnection, "RHand", 1, 0.25);

			selectedControls = shiftlessControls;
		}
		else
		{
			foreach (var c in shiftedControls.Values)
				c.Stop();
			foreach (var c in shiftlessControls.Values)
				c.Stop();

			shiftedControls.Clear();
			shiftlessControls.Clear();

			selectedControls = null;
		}		

		ttsEntry.Sensitive = !controlToggle.Active;
	}

}
