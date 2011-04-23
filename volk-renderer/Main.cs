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
			
			vScene vs = MainClass.CornellBox ();
			
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
			
			
			cbvs.origin = new Vector3d (0, 0, -800);
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
			
			//cbvs.addPointLight (new Vector3d (0, 276, 278), Color.White);
			cbvs.addAreaLight (
				new Vector3d (-65, 277.99999, 227),
				new Vector3d (65, 277.99999, 227),
				new Vector3d (65, 277.99999, 332),
				new Vector3d (-65, 277.99999, 332),
				Color.White, 1.0);

			
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
			
			/*Sphere bluesphere = new Sphere (Color.Blue, new Vector3d (-140, -178, 275), 100);
			//bluesphere.setRefract(1.3);
			//bluesphere.setTransparency(0.5);
			bluesphere.setAmbient (0.0);
			bluesphere.setDiffuse (1.0);
			bluesphere.setSpecular (1.0);
			cbvs.addPrim (bluesphere);*/
		
			Sphere bluesphere2 = new Sphere (Color.Black, new Vector3d (120, -178, 400), 100);
			bluesphere2.setRefract (1.3);
			bluesphere2.setTransparency (0.7);
			bluesphere2.setAmbient (0.0);
			bluesphere2.setDiffuse (0.2);
			bluesphere2.setSpecular (1.0);
			cbvs.addPrim (bluesphere2);
			
			//cbvs.addSphere (new Vector3d (-140, -178, 275), 100, Color.Blue);
			
			
			Sphere reflectsphere = new Sphere (Color.Black, new Vector3d (-91, -178, 300), 100, 1.0);
			reflectsphere.setSpecular (0.7);
			reflectsphere.setAmbient (0.0);
			cbvs.addPrim (reflectsphere);
			
			cbvs.addPrim (floor);
			cbvs.addPrim (ceiling);
			cbvs.addPrim (backwall);
			cbvs.addPrim (leftwall);
			cbvs.addPrim (rightwall);
			
			return cbvs;
		

				

		}
		
		static vScene reflectionBox ()
		
		{
			vScene cbvs = new vScene (640, 480);			
			
			cbvs.origin = new Vector3d (0, -50, -500);
			cbvs.target = new Vector3d (0, -50, 0);
			
			Quad floor = new Quad (
				new Vector3d (278, -278, 0.0), 
				new Vector3d (-278, -278, 0.0),
				new Vector3d (-278, -278, 556),
				new Vector3d (278, -278, 556),
				Color.Gray);
			
			Quad ceiling = new Quad (
				new Vector3d (-278, 278, 0.0),
				new Vector3d (278, 278, 0.0),
				new Vector3d (278, 278, 556),
				new Vector3d (-278, 278, 556),
				Color.Gray);
			
			Quad backwall = new Quad (
				new Vector3d (278, -278, 556), 
				new Vector3d (-278, -278, 556),
				new Vector3d (-278, 278, 556),
				new Vector3d (278, 278, 556),
				Color.Blue);
			
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
			
			//cbvs.addPointLight (new Vector3d (0, 276, 278), Color.White);
			cbvs.addAreaLight (
				new Vector3d (-65, 277.99999, 227),
				new Vector3d (65, 277.99999, 227),
				new Vector3d (65, 277.99999, 332),
				new Vector3d (-65, 277.99999, 332),
				Color.Gray, 1.0);

			
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
			
			floor.setReflect (1.0);
			ceiling.setReflect (1.0);
			backwall.setReflect (1.0);
			leftwall.setReflect (1.0);
			rightwall.setReflect (1.0);
			
			/*Sphere bluesphere = new Sphere (Color.Blue, new Vector3d (-140, -178, 275), 100);
			//bluesphere.setRefract(1.3);
			//bluesphere.setTransparency(0.5);
			bluesphere.setAmbient (0.0);
			bluesphere.setDiffuse (1.0);
			bluesphere.setSpecular (1.0);
			cbvs.addPrim (bluesphere);*/
			
			Sphere bluesphere2 = new Sphere (Color.Black, new Vector3d (120, -178, 400), 100);
			bluesphere2.setRefract (1.3);
			bluesphere2.setTransparency (0.7);
			bluesphere2.setAmbient (0.0);
			bluesphere2.setDiffuse (0.2);
			bluesphere2.setSpecular (1.0);
			cbvs.addPrim (bluesphere2);
			
			//cbvs.addSphere (new Vector3d (-140, -178, 275), 100, Color.Blue);			
			
			Sphere reflectsphere = new Sphere ( Color.Black,new Vector3d (-91, -178, 300), 100, 1.0);
			reflectsphere.setSpecular (0.7);
			reflectsphere.setAmbient (0.0);
			cbvs.addPrim (reflectsphere);
			
			cbvs.addPrim (floor);
			cbvs.addPrim (ceiling);
			cbvs.addPrim(backwall);
			cbvs.addPrim(leftwall);
			cbvs.addPrim(rightwall);			
			
			return cbvs;
		}
	}
}

