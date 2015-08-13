
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.HBox hbox1;
	
	private global::Gtk.Label label1;
	
	private global::Gtk.Entry ipEntry;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.Button connectButton;
	
	private global::Gtk.Entry passwordEntry;
	
	private global::Gtk.Label label3;
	
	private global::Gtk.Entry userNameEntry;
	
	private global::Gtk.DrawingArea cameraArea;
	
	private global::Gtk.HBox hbox2;
	
	private global::Gtk.Entry ttsEntry;
	
	private global::Gtk.Button ttsButton;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("NAO Remote Control");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.DefaultWidth = 300;
		this.DefaultHeight = 300;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("NAO IP:");
		this.hbox1.Add (this.label1);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label1]));
		w1.Position = 0;
		w1.Expand = false;
		w1.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.ipEntry = new global::Gtk.Entry ();
		this.ipEntry.WidthRequest = 100;
		this.ipEntry.CanFocus = true;
		this.ipEntry.Name = "ipEntry";
		this.ipEntry.Text = global::Mono.Unix.Catalog.GetString ("169.254.179.212");
		this.ipEntry.IsEditable = true;
		this.ipEntry.InvisibleChar = '●';
		this.hbox1.Add (this.ipEntry);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.ipEntry]));
		w2.Position = 1;
		// Container child hbox1.Gtk.Box+BoxChild
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Username:");
		this.hbox1.Add (this.label2);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label2]));
		w3.Position = 2;
		w3.Expand = false;
		w3.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.connectButton = new global::Gtk.Button ();
		this.connectButton.CanFocus = true;
		this.connectButton.Name = "connectButton";
		this.connectButton.UseUnderline = true;
		this.connectButton.Label = global::Mono.Unix.Catalog.GetString ("Connect");
		global::Gtk.Image w4 = new global::Gtk.Image ();
		w4.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-apply", global::Gtk.IconSize.Menu);
		this.connectButton.Image = w4;
		this.hbox1.Add (this.connectButton);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.connectButton]));
		w5.PackType = ((global::Gtk.PackType)(1));
		w5.Position = 3;
		w5.Expand = false;
		w5.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.passwordEntry = new global::Gtk.Entry ();
		this.passwordEntry.WidthRequest = 75;
		this.passwordEntry.CanFocus = true;
		this.passwordEntry.Name = "passwordEntry";
		this.passwordEntry.Text = global::Mono.Unix.Catalog.GetString ("nao");
		this.passwordEntry.IsEditable = true;
		this.passwordEntry.InvisibleChar = '●';
		this.hbox1.Add (this.passwordEntry);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.passwordEntry]));
		w6.PackType = ((global::Gtk.PackType)(1));
		w6.Position = 4;
		// Container child hbox1.Gtk.Box+BoxChild
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("Password:");
		this.hbox1.Add (this.label3);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.label3]));
		w7.PackType = ((global::Gtk.PackType)(1));
		w7.Position = 5;
		w7.Expand = false;
		w7.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.userNameEntry = new global::Gtk.Entry ();
		this.userNameEntry.WidthRequest = 75;
		this.userNameEntry.CanFocus = true;
		this.userNameEntry.Name = "userNameEntry";
		this.userNameEntry.Text = global::Mono.Unix.Catalog.GetString ("nao");
		this.userNameEntry.IsEditable = true;
		this.userNameEntry.InvisibleChar = '●';
		this.hbox1.Add (this.userNameEntry);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.userNameEntry]));
		w8.PackType = ((global::Gtk.PackType)(1));
		w8.Position = 6;
		this.vbox1.Add (this.hbox1);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
		w9.Position = 0;
		w9.Expand = false;
		w9.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.cameraArea = new global::Gtk.DrawingArea ();
		this.cameraArea.Name = "cameraArea";
		this.vbox1.Add (this.cameraArea);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.cameraArea]));
		w10.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox2 = new global::Gtk.HBox ();
		this.hbox2.Name = "hbox2";
		this.hbox2.Spacing = 6;
		// Container child hbox2.Gtk.Box+BoxChild
		this.ttsEntry = new global::Gtk.Entry ();
		this.ttsEntry.CanFocus = true;
		this.ttsEntry.Name = "ttsEntry";
		this.ttsEntry.IsEditable = true;
		this.ttsEntry.InvisibleChar = '●';
		this.hbox2.Add (this.ttsEntry);
		global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.ttsEntry]));
		w11.Position = 0;
		// Container child hbox2.Gtk.Box+BoxChild
		this.ttsButton = new global::Gtk.Button ();
		this.ttsButton.WidthRequest = 100;
		this.ttsButton.HeightRequest = 50;
		this.ttsButton.CanFocus = true;
		this.ttsButton.Name = "ttsButton";
		this.ttsButton.UseUnderline = true;
		this.ttsButton.Label = global::Mono.Unix.Catalog.GetString ("Say");
		global::Gtk.Image w12 = new global::Gtk.Image ();
		w12.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "stock_volume", global::Gtk.IconSize.Button);
		this.ttsButton.Image = w12;
		this.hbox2.Add (this.ttsButton);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.ttsButton]));
		w13.Position = 1;
		w13.Expand = false;
		w13.Fill = false;
		this.vbox1.Add (this.hbox2);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox2]));
		w14.PackType = ((global::Gtk.PackType)(1));
		w14.Position = 2;
		w14.Expand = false;
		w14.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.cameraArea.ExposeEvent += new global::Gtk.ExposeEventHandler (this.OnCameraAreaExposeEvent);
		this.ttsButton.Clicked += new global::System.EventHandler (this.OnTtsButtonClicked);
	}
}
