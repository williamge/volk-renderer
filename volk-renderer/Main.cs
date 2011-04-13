using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
#if CONSFLAG
using OpenTK;
#endif

namespace volkrenderer
{
	class MainClass
	{
		static void Main (string[] args)
		{
#if CONSFLAG
			vScene vs = new vScene (640, 480);
			vs.addSphere (new Vector3d (0, 0, 15), 80, Color.Blue);
			vs.addPointLight (new Vector3d (80, 120, 0), Color.White);
			vs.addPlane(new Vector3d(0,-250,0),new Vector3d(0,1,0),Color.Red);
			new raytrace (vs);
#else
			NSApplication.Init ();
			NSApplication.Main (args);
#endif
		}
	}
}

