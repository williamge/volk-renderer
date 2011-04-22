using System;
using System.IO;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
using OpenTK;
using System.Diagnostics;

namespace volkrenderer
{
	class MainClass
	{
		
		/* Note: on the name of this, it turns out getting the relative path
		 * of something in C#/.NET is an exercise in pain, after googling around 
		 * and checking MSDN for ages I figured out to get an "Assembly" of this
		 * program executing, find the location from that, get the directory name
		 * of that path and then add filenames to the end of that. This also
		 * doesn't work that well when I program this for the mac since the relative
		 * path of the executing assembly is in the app, so I would have to go
		 * up by two or so directories to save it outside the app, great. */
		public static string THEGODDAMNRELATIVEPATH;
		
		
		static void Main (string[] args)
		{
			
			THEGODDAMNRELATIVEPATH = System.IO.Path.GetDirectoryName (System.Reflection.Assembly.GetExecutingAssembly ().Location);
			
			
			/* TODO
			 * clean this up to make a good example scene */

			//vScene vs = MainClass.defaultScene();
			vScene vs = MainClass.CornellBox();
			
			long Frequency, Ticks, TotalTime;
			Stopwatch sw = new Stopwatch ();
			Frequency = Stopwatch.Frequency;
			sw.Start ();
			
			raytrace rt = new raytrace (vs);
			
			sw.Stop ();
			Ticks = sw.ElapsedTicks;
			TotalTime = 1000000L * Ticks / Frequency * 1/1000000;
			Console.WriteLine ("Total time in seconds " + TotalTime);
#if WINDOWS
			rt.imageSave( THEGODDAMNRELATIVEPATH + @"\render.jpg");
#else
			rt.imageSave ("/Users/william/Dropbox/repos/volk-rend-csharp/volk-renderer/volk-renderer/bin/test.jpg");
#endif

			/*NSApplication.Init ();
			NSApplication.Main (args);*/

		}
		
		static vScene defaultScene ()
		{
			vScene vs = new vScene (640, 480);
			
			Sphere tsphere = new Sphere (Color.Blue, new Vector3d (0, -10, 100), 90);
			
			#if WINDOWS
			Bitmap checker = new Bitmap (THEGODDAMNRELATIVEPATH + @"\checker.jpg");
			#else
			Bitmap checker = new Bitmap (THEGODDAMNRELATIVEPATH + "/" + "checker.png");
			#endif 
			
			#if WINDOWS
			tsphere.setTexture (new Bitmap (THEGODDAMNRELATIVEPATH + @"\dog.jpg"));
			#else
			tsphere.setTexture (new Bitmap (THEGODDAMNRELATIVEPATH + "/" + "dog.jpg"));
			#endif
			//vs.addPrim (tsphere);
			//vs.addSphere (new Vector3d (-200, 0, 175), 90, Color.Gray, 0.5);
			
			vs.addPointLight (new Vector3d (-120, 120, 0), Color.White, 1.0);
			vs.addPointLight (new Vector3d (-120, 120, -20), Color.Gray, 1.0);
			Plane pl = new Plane (new Vector3d (0, -100, -480), new Vector3d (0, 1, 0), Color.DarkSalmon);

			Triangle tr = new Triangle(new Vector3d(0,0,100), new Vector3d(10,100,150),new Vector3d(50,50,50),Color.Aqua);
			
			Quad qu = new Quad(new Vector3d(-200,-100,100),
								new Vector3d(-200,200,150),
								new Vector3d(0,200,150),
								new Vector3d(0,-100,100),
								Color.WhiteSmoke);
			vs.addPrim(qu);
			vs.addPrim(tr);
			Console.WriteLine (pl.setTexture (checker));
			Console.WriteLine (vs.addPrim (pl));
			
			Teapot teap = new Teapot();
			vs.addPrim(teap);
			
			return vs;
		}
		
		static vScene CornellBox ()
		{
			vScene cbvs = new vScene (640, 480);
			
			
			cbvs.origin = new Vector3d (0, 0, -273);
			cbvs.target = new Vector3d (0, 0, 0);
			
			Quad floor = new Quad (
				new Vector3d (278, -278, 0.0), 
				new Vector3d (-278, -278, 0.0),
				new Vector3d (-278, -278, 556),
				new Vector3d (278, -278, 556),
				Color.White);
			
			Quad ceiling = new Quad (
				new Vector3d (-278, 278, 0.0),
				new Vector3d (278, 278, 0.0),
				new Vector3d (278, 278, 556),
				new Vector3d (-278, 278, 556),
				Color.White);
			
			Quad backwall = new Quad (
				new Vector3d (278, -278, 556), 
				new Vector3d (-278, -278, 556),
				new Vector3d (-278, 278, 556),
				new Vector3d (278, 278, 556),
				Color.White);
			
			Quad leftwall = new Quad (
				new Vector3d (-278, -278, 556), 
				new Vector3d (-278, -278, 0.0),
				new Vector3d (-278, 278, 0.0), 
				new Vector3d (-278, 278, 556), 
				Color.Red);
			
			Quad rightwall = new Quad (
				new Vector3d (278, -278, 0),
				new Vector3d (278, -278, 556),
				new Vector3d (278, 278, 556),
				new Vector3d (278, 278, 0), 
				Color.Green);
			
			cbvs.addPointLight (new Vector3d (0, 276, 278), Color.White);
			
			floor.setAmbient (0.0);
			ceiling.setAmbient (0.0);
			backwall.setAmbient (0.0);
			leftwall.setAmbient (0.0);
			rightwall.setAmbient (0.0);
			
			floor.setDiffuse (1.0);
			ceiling.setDiffuse (1.0);
			backwall.setDiffuse (1.0);
			leftwall.setDiffuse (1.0);
			rightwall.setDiffuse (1.0);
			
			floor.setSpecular (0.0);
			ceiling.setSpecular (0.0);
			backwall.setSpecular (0.0);
			leftwall.setSpecular (0.0);
			rightwall.setSpecular (0.0);
			
			cbvs.addSphere (new Vector3d (-140, -178, 275), 100, Color.White);
			
			cbvs.addSphere (new Vector3d (80, -178, 325), 100, Color.White,0.5);			
			
			cbvs.addPrim (floor);
			cbvs.addPrim (ceiling);
			cbvs.addPrim(backwall);
			cbvs.addPrim(leftwall);
			cbvs.addPrim(rightwall);			
			
			return cbvs;
				

				

		}
	}
}

