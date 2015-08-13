
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.HBox hbox1;
	
	private global::Gtk.Label label1;
	
	private global::Gtk.Entry ipEntry;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.ToggleButton connectionToggle;
	
	private global::Gtk.Entry passwordEntry;
	
	private global::Gtk.Label label3;
	
	private global::Gtk.Entry userNameEntry;
	
	private global::Gtk.DrawingArea cameraArea;
	
	private global::Gtk.HBox hbox3;
	
	private global::Gtk.ToggleButton cameraToggle;
	
	private global::Gtk.ToggleButton stiffnessToggle;
	
	private global::Gtk.ToggleButton controlToggle;
	
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
		this.connectionToggle = new global::Gtk.ToggleButton ();
		this.connectionToggle.CanFocus = true;
		this.connectionToggle.Name = "connectionToggle";
		this.connectionToggle.UseUnderline = true;
		this.connectionToggle.Label = global::Mono.Unix.Catalog.GetString ("Connection");
		global::Gtk.Image w4 = new global::Gtk.Image ();
		w4.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-connect", global::Gtk.IconSize.LargeToolbar);
		this.connectionToggle.Image = w4;
		this.hbox1.Add (this.connectionToggle);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.connectionToggle]));
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
		this.hbox3 = new global::Gtk.HBox ();
		this.hbox3.Name = "hbox3";
		this.hbox3.Homogeneous = true;
		this.hbox3.Spacing = 6;
		// Container child hbox3.Gtk.Box+BoxChild
		this.cameraToggle = new global::Gtk.ToggleButton ();
		this.cameraToggle.WidthRequest = 100;
		this.cameraToggle.HeightRequest = 50;
		this.cameraToggle.Sensitive = false;
		this.cameraToggle.CanFocus = true;
		this.cameraToggle.Name = "cameraToggle";
		this.cameraToggle.UseUnderline = true;
		this.cameraToggle.Label = global::Mono.Unix.Catalog.GetString ("Vision");
		global::Gtk.Image w11 = new global::Gtk.Image ();
		w11.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-find", global::Gtk.IconSize.LargeToolbar);
		this.cameraToggle.Image = w11;
		this.hbox3.Add (this.cameraToggle);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.cameraToggle]));
		w12.Position = 0;
		w12.Expand = false;
		w12.Fill = false;
		// Container child hbox3.Gtk.Box+BoxChild
		this.stiffnessToggle = new global::Gtk.ToggleButton ();
		this.stiffnessToggle.WidthRequest = 100;
		this.stiffnessToggle.HeightRequest = 50;
		this.stiffnessToggle.Sensitive = false;
		this.stiffnessToggle.CanFocus = true;
		this.stiffnessToggle.Name = "stiffnessToggle";
		this.stiffnessToggle.UseUnderline = true;
		this.stiffnessToggle.Label = global::Mono.Unix.Catalog.GetString ("Stiffness");
		global::Gtk.Image w13 = new global::Gtk.Image ();
		w13.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-dialog-authentication", global::Gtk.IconSize.LargeToolbar);
		this.stiffnessToggle.Image = w13;
		this.hbox3.Add (this.stiffnessToggle);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.stiffnessToggle]));
		w14.Position = 1;
		w14.Expand = false;
		w14.Fill = false;
		// Container child hbox3.Gtk.Box+BoxChild
		this.controlToggle = new global::Gtk.ToggleButton ();
		this.controlToggle.WidthRequest = 100;
		this.controlToggle.HeightRequest = 50;
		this.controlToggle.Sensitive = false;
		this.controlToggle.CanFocus = true;
		this.controlToggle.Name = "controlToggle";
		this.controlToggle.UseUnderline = true;
		this.controlToggle.Label = global::Mono.Unix.Catalog.GetString ("Control");
		global::Gtk.Image w15 = new global::Gtk.Image ();
		w15.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "stock_zoom-page", global::Gtk.IconSize.LargeToolbar);
		this.controlToggle.Image = w15;
		this.hbox3.Add (this.controlToggle);
		global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.controlToggle]));
		w16.Position = 2;
		w16.Expand = false;
		w16.Fill = false;
		this.vbox1.Add (this.hbox3);
		global::Gtk.Box.BoxChild w17 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox3]));
		w17.Position = 2;
		w17.Expand = false;
		w17.Fill = false;
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
		global::Gtk.Box.BoxChild w18 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.ttsEntry]));
		w18.Position = 0;
		// Container child hbox2.Gtk.Box+BoxChild
		this.ttsButton = new global::Gtk.Button ();
		this.ttsButton.WidthRequest = 100;
		this.ttsButton.HeightRequest = 50;
		this.ttsButton.Sensitive = false;
		this.ttsButton.CanFocus = true;
		this.ttsButton.Name = "ttsButton";
		this.ttsButton.UseUnderline = true;
		this.ttsButton.Label = global::Mono.Unix.Catalog.GetString ("Say");
		global::Gtk.Image w19 = new global::Gtk.Image ();
		w19.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "stock_volume", global::Gtk.IconSize.Button);
		this.ttsButton.Image = w19;
		this.hbox2.Add (this.ttsButton);
		global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.ttsButton]));
		w20.Position = 1;
		w20.Expand = false;
		w20.Fill = false;
		this.vbox1.Add (this.hbox2);
		global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox2]));
		w21.PackType = ((global::Gtk.PackType)(1));
		w21.Position = 3;
		w21.Expand = false;
		w21.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.KeyPressEvent += new global::Gtk.KeyPressEventHandler (this.OnKeyPressEvent);
		this.KeyReleaseEvent += new global::Gtk.KeyReleaseEventHandler (this.OnKeyReleaseEvent);
		this.connectionToggle.Toggled += new global::System.EventHandler (this.OnConnectionToggleToggled);
		this.cameraArea.ExposeEvent += new global::Gtk.ExposeEventHandler (this.OnCameraAreaExposeEvent);
		this.cameraToggle.Toggled += new global::System.EventHandler (this.OnCameraToggleToggled);
		this.ttsButton.Clicked += new global::System.EventHandler (this.OnTtsButtonClicked);
	}
}
