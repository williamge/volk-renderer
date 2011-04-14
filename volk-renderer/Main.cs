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
			vScene vs = new vScene (1440, 768);
			vs.addSphere (new Vector3d (0, -10, 100), 90, Color.Blue);
			vs.addSphere (new Vector3d (-200, 0, 175), 90, Color.Black,0.9);
			vs.addPointLight (new Vector3d (-120, 120, 0), Color.White, 1.0);
			vs.addPlane (new Vector3d (0, -100, -480), new Vector3d (0, 1, 0), Color.DarkSalmon);
			//vs.addPlane (new Vector3d(0,-100,400), new Vector3d(0,0,-1),Color.Salmon); 
			new raytrace (vs);
#else
			NSApplication.Init ();
			NSApplication.Main (args);
#endif
		}
	}
}

