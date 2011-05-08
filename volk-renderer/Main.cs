using System;
using System.IO;
using System.Drawing;
#if WINDOWS
#else
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;
#endif
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
			
			vScene vs = MainClass.radiosityRoom ();
			
			long Frequency, Ticks, TotalTime;
			Stopwatch sw = new Stopwatch ();
			Frequency = Stopwatch.Frequency;
			
			raytrace rt = new raytrace (vs);
			
			sw.Start ();
			rt.RStart();			
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
		
		static vScene testScene ()
		
		{
			vScene vs = new vScene (640, 480);
			vs.setBack (Color.HotPink);
			
			vs.origin = new Vector3d (0, 0, -1);
			vs.target = new Vector3d (0, 0, 0);
			
			Teapot tp = new Teapot ();
			vs.addPrim (tp);
			
			
			return vs;
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
			
			
			cbvs.origin = new Vector3d (0, -5, -800);
			cbvs.target = new Vector3d (0, -5, 0);
			
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
			
			floor.setDiffuse (0.8);
			ceiling.setDiffuse (0.8);
			backwall.setDiffuse (0.8);
			leftwall.setDiffuse (0.8);
			rightwall.setDiffuse (0.8);
			
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
			cbvs.depth = 9;
			cbvs.lightnums = 7.0;
			
			cbvs.origin = new Vector3d (0, -5, -800);
			cbvs.target = new Vector3d (0, -5, 0);
			
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
		
		static vScene radiosityRoom ()
		{
			vScene rrvs = new vScene (640, 480);
			
			/*rrvs.origin = new Vector3d (300, 125, -500);
			rrvs.target = new Vector3d (100, 125, 0);*/
			
			rrvs.origin = new Vector3d (280, 125, -100);
			rrvs.target = new Vector3d (10, 125, 300);
			
			/*rrvs.origin = new Vector3d (-85, 125, 155);
			rrvs.target = new Vector3d (-85, 125, 200);*/
			
			rrvs.fov = 0.01;
			
			rrvs.lightnums = 7;
			
			#region Basic room
			
			Quad floor = new Quad (new Vector3d (300, 0, -100),
									new Vector3d (-250, 0, -100),
									new Vector3d (-250, 0, 300),
									new Vector3d (300, 0, 300), Color.Red);
			
			Quad ceiling = new Quad (new Vector3d (-250, 250, -100),
									new Vector3d (300, 250, -100),
									new Vector3d (300, 250, 300),
									new Vector3d (-250, 250, 300), Color.White);
			
			Quad leftwall = new Quad (new Vector3d (-250, 250, -100),
										new Vector3d (-250, 250, 300),
										new Vector3d (-250, 0, 300),
										new Vector3d (-250, 0, -100), Color.White);
			
			Quad rightwall = new Quad (new Vector3d (300, 250, 300),
										new Vector3d (300, 250, -100),
										new Vector3d (300, 0, -100),
										new Vector3d (300, 0, 300), Color.White);
			
			Quad frontwall = new Quad (new Vector3d (300, 250, -100),
									new Vector3d (-250, 250, -100),
									new Vector3d (-250, 0, -100),
									new Vector3d (300, 0, -100), Color.White);
			
			#endregion
			
			#region Left post
			
			
			Quad p1leftwall = new Quad (new Vector3d (-100, 250, 155),
										new Vector3d (-100, 250, 125),
										new Vector3d (-100, 0, 125),
										new Vector3d (-100, 0, 155), Color.White);
			
			Quad p1rightwall = new Quad (new Vector3d (-70, 250, 125),
										new Vector3d (-70, 250, 155),
										new Vector3d (-70, 0, 155),
										new Vector3d (-70, 0, 125), Color.White);
			
			Quad p1backwall = new Quad (new Vector3d (-100, 250, 125),
										new Vector3d (-70, 250, 125),
										new Vector3d (-70, 0, 125),
										new Vector3d (-100, 0, 125), Color.White);
			
			Quad p1frontwall = new Quad (new Vector3d (-70, 250, 155),
										new Vector3d (-100, 250, 155),
										new Vector3d (-100, 0, 155),
										new Vector3d (-70, 0, 155), Color.White);
			
			#endregion
			
			#region Right post
			
			
			Quad p2leftwall = new Quad (new Vector3d (100, 250, 155),
										new Vector3d (100, 250, 125),
										new Vector3d (100, 0, 125),
										new Vector3d (100, 0, 155), Color.White);
			
			Quad p2rightwall = new Quad (new Vector3d (130, 250, 125),
										new Vector3d (130, 250, 155),
										new Vector3d (130, 0, 155),
										new Vector3d (130, 0, 125), Color.White);
			
			Quad p2backwall = new Quad (new Vector3d (100, 250, 125),
										new Vector3d (130, 250, 125),
										new Vector3d (130, 0, 125),
										new Vector3d (100, 0, 125), Color.White);
			
			Quad p2frontwall = new Quad (new Vector3d (130, 250, 155),
										new Vector3d (100, 250, 155),
										new Vector3d (100, 0, 155),
										new Vector3d (130, 0, 155), Color.White);
			
			#endregion
			
			#region Back wall
			
			Quad b1wall = new Quad (new Vector3d (-250, 250, 300),
									new Vector3d (-170, 250, 300),
									new Vector3d (-170, 0, 300),
									new Vector3d (-250, 0, 300), Color.White);
			
			Quad b2wall = new Quad (new Vector3d (-90, 250, 300),
									new Vector3d (110, 250, 300),
									new Vector3d (110, 0, 300),
									new Vector3d (-90, 0, 300), Color.White);
			
			Quad b3wall = new Quad (new Vector3d (190, 250, 300),
									new Vector3d (300, 250, 300),
									new Vector3d (300, 0, 300),
									new Vector3d (190, 0, 300), Color.White);
			
			#region Left window
			
			Quad b12leftwall = new Quad (new Vector3d (-170, 250, 300),
										new Vector3d (-170, 250, 315),
										new Vector3d (-170, 0, 315),
										new Vector3d (-170, 0, 300), Color.White);
			
			Quad b12rightwall = new Quad (new Vector3d (-90, 0, 300),
											new Vector3d (-90, 0, 315),
											new Vector3d (-90, 250, 315),
											new Vector3d (-90, 250, 300), Color.White);
			
			Quad b12floor = new Quad (new Vector3d (-90, 0, 300),
										new Vector3d (-170, 0, 300),
										new Vector3d (-170, 0, 315),
										new Vector3d (-90, 0, 315), Color.Red);
			
			Quad b12ceiling = new Quad (new Vector3d (-90, 250, 300),
										new Vector3d (-90, 250, 315),
										new Vector3d (-170, 250, 315),
										new Vector3d (-170, 250, 300), Color.White);
			
			Quad b12lowerback = new Quad (new Vector3d(-170,60,315),
											new Vector3d(-90,60,315),
											new Vector3d(-90,0,315),
											new Vector3d(-170,0,315), Color.White);
			
			Quad b12upperback = new Quad (new Vector3d(-170,250,315),
											new Vector3d(-90,250,315),
											new Vector3d(-90,220,315),
											new Vector3d(-170,220,315), Color.White);
			
			#endregion
			
			#region Right window
			
			Quad b23leftwall = new Quad (new Vector3d (110, 250, 300),
										new Vector3d (110, 250, 315),
										new Vector3d (110, 0, 315),
										new Vector3d (110, 0, 300), Color.White);
			
			Quad b23rightwall = new Quad (new Vector3d (190, 0, 300),
											new Vector3d (190, 0, 315),
											new Vector3d (190, 250, 315),
											new Vector3d (190, 250, 300), Color.White);
			
			Quad b23floor = new Quad (new Vector3d (190, 0, 300),
										new Vector3d (110, 0, 300),
										new Vector3d (110, 0, 315),
										new Vector3d (190, 0, 315), Color.Red);
			
			Quad b23ceiling = new Quad (new Vector3d (190, 250, 300),
										new Vector3d (190, 250, 315),
										new Vector3d (110, 250, 315),
										new Vector3d (110, 250, 300), Color.White);
			
			Quad b23lowerback = new Quad (new Vector3d(110,60,315),
											new Vector3d(190,60,315),
											new Vector3d(190,0,315),
											new Vector3d(110,0,315), Color.White);
			
			Quad b23upperback = new Quad (new Vector3d(110,250,315),
											new Vector3d(190,250,315),
											new Vector3d(190,220,315),
											new Vector3d(110,220,315), Color.White);
			
			#endregion
				
			#endregion
			
			/*
			floor.setAmbient (0.0);
			ceiling.setAmbient (0.0);
			leftwall.setAmbient (0.0);
			rightwall.setAmbient (0.0);
			frontwall.setAmbient (0.0);
			
			p1leftwall.setAmbient (0.0);
			p1rightwall.setAmbient (0.0);
			p1backwall.setAmbient (0.0);
			p1frontwall.setAmbient (0.0);
			
			p2leftwall.setAmbient (0.0);
			p2rightwall.setAmbient (0.0);
			p2backwall.setAmbient (0.0);
			p2frontwall.setAmbient (0.0);
			
			b1wall.setAmbient (0.0);
			b2wall.setAmbient (0.0);
			b3wall.setAmbient (0.0);
			*/
		
			floor.setSpecular (0.0);
			ceiling.setSpecular (0.0);
			leftwall.setSpecular (0.0);
			rightwall.setSpecular (0.0);
			frontwall.setSpecular (0.0);
			
			p1leftwall.setSpecular (0.0);
			p1rightwall.setSpecular (0.0);
			p1backwall.setSpecular (0.0);
			p1frontwall.setSpecular (0.0);
			
			p2leftwall.setSpecular (0.0);
			p2rightwall.setSpecular (0.0);
			p2backwall.setSpecular (0.0);
			p2frontwall.setSpecular (0.0);
			
			b1wall.setSpecular (0.0);
			b2wall.setSpecular (0.0);
			b3wall.setSpecular (0.0);
			
			b12leftwall.setSpecular (0.0);
			b12rightwall.setSpecular (0.0);
		
			rrvs.addPrim (floor);
			rrvs.addPrim (ceiling);
			rrvs.addPrim (leftwall);
			rrvs.addPrim (rightwall);
			rrvs.addPrim (frontwall);
			
			rrvs.addPrim (p1leftwall);
			rrvs.addPrim (p1rightwall);
			rrvs.addPrim (p1backwall);
			rrvs.addPrim (p1frontwall);
		
			rrvs.addPrim (p2leftwall);
			rrvs.addPrim (p2rightwall);
			rrvs.addPrim (p2backwall);
			rrvs.addPrim (p2frontwall);
			
			rrvs.addPrim (b1wall);
			rrvs.addPrim (b2wall);
			rrvs.addPrim (b3wall);
			
			rrvs.addPrim (b12leftwall);
			rrvs.addPrim (b12rightwall);
			rrvs.addPrim (b12floor);
			rrvs.addPrim (b12ceiling);
			rrvs.addPrim(b12lowerback);
			rrvs.addPrim(b12upperback);
			
			rrvs.addPrim (b23leftwall);
			rrvs.addPrim (b23rightwall);
			rrvs.addPrim (b23floor);
			rrvs.addPrim (b23ceiling);
			rrvs.addPrim(b23lowerback);
			rrvs.addPrim(b23upperback);
			
			//rrvs.addPointLight (new Vector3d (-130, 125, 350), Color.White,0.5);
			//rrvs.addPointLight (new Vector3d (150, 125, 350), Color.White,0.5);
			
			rrvs.addAreaLight(new Vector3d(-170,220,380),
								new Vector3d(-90,220,380),
								new Vector3d(-90,60,380),
								new Vector3d(-170,60,380), Color.White,1.0);
			
			rrvs.addAreaLight(new Vector3d(110,220,380),
								new Vector3d(190,220,380),
								new Vector3d(190,60,380),
								new Vector3d(110,60,380), Color.White,1.0);

			
			return rrvs;
			
		}
	}
}

