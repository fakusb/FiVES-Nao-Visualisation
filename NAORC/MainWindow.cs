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
	private const uint refreshFrequency = 1000;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		OnConnectButtonClicked(null, null);
	}


	void receiveCallback(IAsyncResult state)
	{
		// Nothing to do here.
	}

	void connectCallback(IAsyncResult state)
	{
		((System.Net.Sockets.Socket)state.AsyncState).EndConnect(state);
	}

	protected bool refreshCameraImage()
	{
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
		commandConnection.execute("close()");
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnConnectButtonClicked (object sender, EventArgs e)
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

		GLib.Timeout.Add(refreshFrequency, new GLib.TimeoutHandler(refreshCameraImage));
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

		Cairo.Context cx = Gdk.CairoHelper.Create(area.GdkWindow);

		Gdk.CairoHelper.SetSourcePixbuf(cx, imagePixBuf, 0.0, 0.0);

		cx.Rectangle(0, 0, 320, 240);
		cx.Fill();
	}
}
