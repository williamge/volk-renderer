//#define THREADING
#define CONSFLAGD
#define AMBIENT
#define DIFFUSE
#define SPECULAR
#define SHADOWS
#define REFLECTION

using System;
using System.Drawing;
using OpenTK;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;


namespace volkrenderer
{
	public class raytrace
	{
		Bitmap im;
		
		Vector3d camx,camy,camz;
		
		/* for performance reasons (to avoid the use of Bitmap.getPixel/setPixel) the image (and textures)
		 * are stored in an array of doubles and during the image save (and texture load) the doubles are
		 * converted to an image */
		double[,,] dimage;
		
		#if CONSFLAGD
		Int64 rays;
		Int64 killedrays;
		Int64 shadowrays;
		#endif
		
		public raytrace (vScene scene)
		{
			im = new Bitmap (scene.ImageWidth, scene.ImageHeight);
			dimage = new double[scene.ImageWidth,scene.ImageHeight,3];
			
			Vector3d origin = scene.origin;			
			Vector3d target = scene.target;
			
			
			/* Note: some values were interpreted from this page: http://www.unknownroad.com/rtfm/graphics/rt_eyerays.html
			 * It is why there is a commented out pi/4
			 * */
		
			
			//Math.PI / 4.0;
			//enter fov here(on right)
			double fovx = 1.0 - 0.3;
			double fovy = fovx;
			
			fovx = Math.Tan (fovx);
			fovy = Math.Tan (fovy);
			
			camz = target - origin;
			camz.Normalize ();
			Vector3d up = new Vector3d (0, 1, 0);
			
			camx = Vector3d.Cross (up, camz);
			camy = Vector3d.Cross (camx, -camz);
			
			#if CONSFLAGD
			Console.WriteLine (camz.ToString ());
			Console.WriteLine (camx.ToString ());
			Console.WriteLine (camy.ToString ());
			#endif
			
			#if CONSFLAGD
			rays = 0;
			killedrays = 0;
			shadowrays = 0;
			#endif			
			
			double aacoef = 1.0;
#if THREADING
#else
			double[] pcol_ = new double[3];
			double[] pcol = new double[3];
#endif
			int iheight = im.Height;
			int iwidth = im.Width;
			
#if THREADING
			
			Task<double[]>[] task = new Task<double[]>[4];
			
			
			
			
			Parallel.For(0,iwidth,delegate(int x)	{
			
			/*for (int x = 0; x < iwidth; x++) {*/
				for (int y = 0; y < iheight; y++) {
					
					double[] pcol_ = new double[3];
					double[] pcol = new double[3];
				
					pcol_[0] = 0;
					pcol_[1] = 0;
					pcol_[2] = 0;
					pcol[0] = 0;
					pcol[1] = 0;
					pcol[2] = 0;
#else
			
			for (int x = 0; x < iwidth; x++) {
				for (int y = 0; y < iheight; y++) {
				
					pcol_[0] = 0;
					pcol_[1] = 0;
					pcol_[2] = 0;
					pcol[0] = 0;
					pcol[1] = 0;
					pcol[2] = 0;
					
#endif
					
#if THREADING				
					
					//
					
					Vector3d dirprime = (fovx * camx * (x - iwidth / 2)) + (fovy * camy * -(y - iheight / 2)) + camz - origin;
					dirprime.Normalize ();
							
					threadinfo[] tinfo = new threadinfo[4];
					tinfo[0] = new threadinfo(origin, dirprime, scene);


					task[0] = Task.Factory.StartNew (
								(ti__) 
								=>
		 						{ return threadtrace (ti__); },tinfo[0] );
					
					//
					
					dirprime = (fovx * camx * (x + 0.5 - iwidth / 2)) + (fovy * camy * -(y - iheight / 2)) + camz - origin;
					dirprime.Normalize ();
					
					tinfo[1] = new threadinfo(origin, dirprime, scene);

					task[1] = Task.Factory.StartNew (
								(ti__) 
								=>
		 						{ return threadtrace (ti__); },tinfo[1] );
					
					//
					
					dirprime = (fovx * camx * (x + 0.5 - iwidth / 2)) + (fovy * camy * -(y + 0.5 - iheight / 2)) + camz - origin;
					dirprime.Normalize ();
					
					tinfo[2] = new threadinfo(origin, dirprime, scene);

					task[2] = Task.Factory.StartNew (
								(ti__) 
								=>
		 						{ return threadtrace (ti__); },tinfo[2] );
					//
					
					dirprime = (fovx * camx * (x - iwidth / 2)) + (fovy * camy * -(y + 0.5 - iheight / 2)) + camz - origin;
					dirprime.Normalize ();
					
					tinfo[3] = new threadinfo(origin, dirprime, scene);

					task[3] = Task.Factory.StartNew (
								(ti__) 
								=>
		 						{ return threadtrace (ti__); },tinfo[3] );
					

		
					//Task.WaitAll (task[0], task[1], task[2], task[3]);
					
					//pcol_ = task[0].
		
					pcol[0] = 0.25 * task[0].Result[0] 
							+ 0.25 * task[1].Result[0] 
							+ 0.25 * task[2].Result[0] 
							+ 0.25 * task[3].Result[0];
					
					pcol[1] = 0.25 * task[0].Result[1] 
							+ 0.25 * task[1].Result[1] 
							+ 0.25 * task[2].Result[1] 
							+ 0.25 * task[3].Result[1];
					
					pcol[2] = 0.25 * task[0].Result[2] 
							+ 0.25 * task[1].Result[2] 
							+ 0.25 * task[2].Result[2] 
							+ 0.25 * task[3].Result[2];
					
					//exposures, may not be useful idk
					double exposure = scene.exposure;
					pcol[0] = 255.0 * (1.0 - Math.Exp (pcol[0] / 255.0 * exposure));
					pcol[1] = 255.0 * (1.0 - Math.Exp (pcol[1] / 255.0 * exposure));
					pcol[2] = 255.0 * (1.0 - Math.Exp (pcol[2] / 255.0 * exposure));
					
					//gamma correction, may be wrong or unnecessary
					pcol[0] = pcol[0] * Math.Pow (pcol[0] / 255.0, 1.0 / 2.2);
					pcol[1] = pcol[1] * Math.Pow (pcol[1] / 255.0, 1.0 / 2.2);
					pcol[2] = pcol[2] * Math.Pow (pcol[2] / 255.0, 1.0 / 2.2);
					
					pcol[0] = Math.Max (Math.Min (255, pcol[0]), 0);
					pcol[1] = Math.Max (Math.Min (255, pcol[1]), 0);
					pcol[2] = Math.Max (Math.Min (255, pcol[2]), 0);
					
					dimage[x,y,0] = pcol[0];
					dimage[x,y,1] = pcol[1];
					dimage[x,y,2] = pcol[2];
					
						}});
					

						
#else
					
					for (double offx = (double)x; offx <= (double)x + 0.5; offx += 1.5) {
						for (double offy = (double)y; offy <= (double)y + 0.5; offy += 1.5) {
							Vector3d dirprime = (fovx * camx * (offx - scene.ImageWidth / 2)) + (fovy * camy * -(offy - scene.ImageHeight / 2)) + camz - origin;
							//Vector3d dirprime = new Vector3d (offx - im.Width / 2, -(offy - im.Height / 2), 0) - origin;
							dirprime.Normalize ();
											if (x == 319 && y == 76){
					x = x;
				}
							pcol_ = trace (origin, dirprime, scene, 0);
							pcol[0] += aacoef * pcol_[0];
							pcol[1] += aacoef * pcol_[1];
							pcol[2] += aacoef * pcol_[2];
						}
					}
					
					//exposures, may not be useful idk
					double exposure = scene.exposure;
					pcol[0] = 255.0 * (1.0 - Math.Exp (pcol[0] / 255.0 * exposure));
					pcol[1] = 255.0 * (1.0 - Math.Exp (pcol[1] / 255.0 * exposure));
					pcol[2] = 255.0 * (1.0 - Math.Exp (pcol[2] / 255.0 * exposure));
					
					//gamma correction, may be wrong or unnecessary
					pcol[0] = pcol[0] * Math.Pow (pcol[0] / 255.0, 1.0 / 2.2);
					pcol[1] = pcol[1] * Math.Pow (pcol[1] / 255.0, 1.0 / 2.2);
					pcol[2] = pcol[2] * Math.Pow (pcol[2] / 255.0, 1.0 / 2.2);
					
					pcol[0] = Math.Max (Math.Min (255, pcol[0]), 0);
					pcol[1] = Math.Max (Math.Min (255, pcol[1]), 0);
					pcol[2] = Math.Max (Math.Min (255, pcol[2]), 0);		
					
					dimage[x,y,0] = pcol[0];
					dimage[x,y,1] = pcol[1];
					dimage[x,y,2] = pcol[2];									
	
				}
			}
#endif			
			#if CONSFLAGD
			Console.WriteLine ("Rays shot: " + Convert.ToString (rays));
			Console.WriteLine ("Rays that hit depth limit: " + Convert.ToString (killedrays));
			Console.WriteLine ("Shadow rays shot: " + Convert.ToString (shadowrays));
			#endif	

		}
		
#if THREADING
		private double[] threadtrace (object ti__)
		{
			threadinfo ti_ = (threadinfo)ti__;
			return trace(ti_.origin_,ti_.direction_,ti_.scene_,0);
		}
		
		private class threadinfo{
			
			public Vector3d origin_, direction_;
			public vScene scene_;
			
			public threadinfo (Vector3d origin__, Vector3d direction__, vScene scene__)
			{
				origin_ = origin__;
				direction_ = direction__;
				scene_ = scene__;
			}
		}
		
#endif
				
		double[] trace (Vector3d origin, Vector3d direction, vScene scene, int rdepth)
		{
			
			if (rdepth > 4) {
				#if CONSFLAGD
				killedrays++;
				#endif
				double[] bcol = new double[3];
				bcol[0] = bcol[1] = bcol[2] = 0.0;
				return bcol;
			}
			
			#if CONSFLAGD
			rays++;
			#endif
			
			//closest t so far
			double ct = 0.0;
			Primitive cobject = null;
			
			//loop through all objects in scene.
			foreach (Primitive pr in scene.getPrims ()) {
				double iresult = pr.intersect (origin, direction);			
				
				if (iresult > 0.0000001) {
					if (ct == 0.0 || iresult < ct) {
						ct = iresult;
						cobject = pr;
					}
				}
			}
			
			//ready to do lighting
			
			if (ct > 0.0) {
				Vector3d intersectp = origin + direction * ct;
				
				//Color pcol;
				double[] pcol = new double[3] {0.0,0.0,0.0};
			
					/* TODO:
					 * either fix this or get rid of it completely */
			/*	if (cobject.isLight ()) 
				{
					return cobject.getColour(intersectp);
				}*/			
				
				foreach (Light li in scene.getLights ()) {
					Vector3d Lp = li.getPoint ();
						
					Vector3d L = Lp - intersectp;
					L.Normalize ();
						
					double dot = Vector3d.Dot (L, cobject.normal (intersectp));
					if (dot > 0) {
						
						double shade = 1.0;
						#if SHADOWS
						shade = shadowCheck (intersectp, li, scene, cobject);
						#endif
						
						//diffuse multiplier
						double diff = 0.0;
						#if DIFFUSE
						diff = li.getIntensity () * cobject.getDiffuse () * dot;
						#endif
							
						//reflected ray off primitive
						Vector3d R = (2.0 * dot * cobject.normal (intersectp)) - L;
						//specular multiplier
						double spec = 0.0;
							
						#if SPECULAR
						spec = li.getIntensity () * cobject.getSpecular () * Math.Pow (Vector3d.Dot (R, direction), 20);
						#endif
						
							pcol[0] += (shade * 
								(diff * cobject.getColour (intersectp)[0]
									+ spec * li.getColour ()[0]));
						
							pcol[1] += (shade * 
								(diff * cobject.getColour (intersectp)[1]
									+ spec * li.getColour ()[1]));
						
							pcol[2] += (shade * 
								(diff * cobject.getColour (intersectp)[2]
									+ spec * li.getColour ()[2]));
					
					}
				
				}
					
				//ambient lighting 
				double ambient = 0.0;
				#if AMBIENT
				ambient = cobject.getAmbient ();
				#endif
				
				pcol[0] += (ambient * cobject.getColour (intersectp)[0]);
				pcol[1] += (ambient * cobject.getColour (intersectp)[1]);
				pcol[2] += (ambient * cobject.getColour (intersectp)[2]);
				
				#if REFLECTION
				//reflection
				if (cobject.getReflect () > 0) {
					
					Vector3d N = cobject.normal (intersectp);
					Vector3d reflectray = direction - (2.0 * (Vector3d.Dot (N, direction))) * N;
					reflectray.Normalize ();
					
					double[] rcol = trace (intersectp, reflectray,
										scene, rdepth+1);
					pcol[0] += (cobject.getReflect () * rcol[0]);
					pcol[1] += (cobject.getReflect () * rcol[1]);
					pcol[2] += (cobject.getReflect () * rcol[2]);
				}
				#endif
				
				return pcol;
				

			
			} else {
				/* TODO */
				/* add environment mapping */
				return scene.getBack(origin,direction);
			}
		}
		
		
		private double shadowCheck (Vector3d p, Light li, vScene scene, Primitive cobject)
		{
			#if CONSFLAGD
			shadowrays++;
			#endif
			
			double shade = 1.0;
			
			/* TODO */
			/*at some point (adding area lights) i'll have to change this so it accounts for all the points not just the center */
			Vector3d L = li.getPoint () - p;
			double lilength = L.Length;
			L.Normalize ();
			
			foreach (Primitive spr in scene.getPrims ())
			{
				if (!spr.isLight() && spr != cobject && spr != li)
				{
					double objintersect = spr.intersect (p, L);
					if (objintersect > 0.0 && objintersect <= lilength) 
					{
						double objtrans = spr.getTransparency ();
						if (objtrans == 0.0) 
						{
							//shade = shade / 2.0;
							return 0.0;//shade;
						}
						else{
							shade = Math.Min(1.0, shade * objtrans);
						}
						
					}
				}
			}
			return shade;
		}
			
		public bool imageSave (string path)
		{
			for (int i = 0; i < im.Width; i++)
			{
				for (int j = 0; j < im.Height; j++) 
				{
					//Convert the values from the array of doubles to a Color value
					//and set the pixel to that Color
					Color pixcol = Color.FromArgb (Math.Max (Math.Min (255, (int)dimage[i, j, 0]), 0),
						Math.Max (Math.Min (255, (int)dimage[i, j, 1]), 0)
						, Math.Max (Math.Min (255, (int)dimage[i, j, 2]), 0));
					im.SetPixel (i, j, pixcol);
	
				}
			}
			
			im.Save (path);
			
			/* HACK */
			//im.Save ("/Users/william/Dropbox/Public/test.jpg");
			
			
			/* TODO
			 * give it a reason to return something other than true */
			return true;
		}
		
	}

	
	
}

