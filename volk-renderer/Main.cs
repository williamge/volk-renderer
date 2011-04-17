using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
#if CONSFLAG
using OpenTK;
using System.Diagnostics;
#endif

namespace volkrenderer
{
	class MainClass
	{
		static void Main (string[] args)
		{
#if CONSFLAG
			
			/* TODO
			 * clean this up to make a good example scene */

			vScene vs = new vScene (640, 480);
			//vs.addSphere (new Vector3d (0, -10, 100), 90, Color.Blue);
			Sphere tsphere = new Sphere(Color.Blue,new Vector3d (0, -10, 100), 90 );
			tsphere.setTexture(new Bitmap("/Users/william/Dropbox/repos/volk-rend-csharp/volk-renderer/textures/dog.jpg"));
			//tsphere.setTexture(new Bitmap("C:\\Users\\pafcon\\Desktop\\volk-rend\\volk-renderer\\textures\\dog.jpg"));
			vs.addPrim(tsphere);
			vs.addSphere (new Vector3d (-200, 0, 175), 90, Color.Gray,0.5);
			vs.addPointLight (new Vector3d (-120, 120, 0), Color.White, 1.0);
			vs.addPointLight(new Vector3d(-120,120,-20),Color.Gray,1.0);
			Plane pl = new Plane (new Vector3d (0, -100, -480), new Vector3d (0, 1, 0), Color.DarkSalmon);
			Bitmap checker = new Bitmap("/Users/william/Dropbox/repos/volk-rend-csharp/volk-renderer/textures/checker.png");
			//Bitmap checker = new Bitmap("C:\\Users\\pafcon\\Desktop\\volk-rend\\volk-renderer\\textures\\checker.png");
			Console.WriteLine(pl.setTexture(checker));
			Console.WriteLine(vs.addPrim(pl));
			//vs.addPlane (new Vector3d(0,-100,400), new Vector3d(0,0,-1),Color.Salmon); 
			
			long Frequency, Ticks, TotalTime;
			Stopwatch sw = new Stopwatch ();
			Frequency = Stopwatch.Frequency;
			sw.Start ();
			
			raytrace rt = new raytrace (vs);
			
			sw.Stop ();
			Ticks = sw.ElapsedTicks;
			TotalTime = 1000000L * Ticks / Frequency * 1/1000000;
			Console.WriteLine ("Total time in seconds " + TotalTime);
			rt.imageSave("/Users/william/Dropbox/repos/volk-rend-csharp/volk-renderer/volk-renderer/bin/test.jpg");
			//rt.imageSave ("C:\\Users\\pafcon\\Desktop\\volk-rend\\volk-renderer\\volk-renderer\\bin\\test.jpg");
#else
			NSApplication.Init ();
			NSApplication.Main (args);
#endif
		}
	}
}

