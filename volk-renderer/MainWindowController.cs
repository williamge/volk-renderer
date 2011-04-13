
using System;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace volkrenderer
{
	public partial class MainWindowController : MonoMac.AppKit.NSWindowController
	{
		#region Constructors

		// Called when created from unmanaged code
		public MainWindowController (IntPtr handle) : base(handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base(coder)
		{
			Initialize ();
		}

		// Call to load from the XIB/NIB file
		public MainWindowController () : base("MainWindow")
		{
			Initialize ();
		}

		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		//strongly typed window accessor
		public new MainWindow Window {
			get { return (MainWindow)base.Window; }
		}
		
		partial void render (NSButton sender)
		{
			myOutlet1.StringValue = string.Format ("Blah Blah Blah");
			vScene vs = new vScene (640, 480);
			vs.addSphere (new Vector3d (0, 0, 100), 90, Color.Blue);
			vs.addPointLight (new Vector3d (-120, 120, 0), Color.White);
			vs.addPlane (new Vector3d (0,-100,-480), new Vector3d (0, 1, 0), Color.Red);
			new raytrace (vs);
		}
	}
}

