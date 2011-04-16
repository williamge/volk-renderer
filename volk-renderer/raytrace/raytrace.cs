#define NAIVE
//#define THREADING

using System;
using System.Drawing;
using OpenTK;
using System.Threading;
using System.Threading.Tasks;


namespace volkrenderer
{
	public class raytrace
	{
		Bitmap im;
		
		Vector3d camx,camy,camz;
		
		#if CONSFLAG
		Int64 rays;
		Int64 killedrays;
		Int64 shadowrays;
		#endif
		
		public raytrace (vScene scene)
		{
			im = new Bitmap (scene.ImageWidth, scene.ImageHeight);
			//Vector3d origin = new Vector3d (0, 0, -scene.ImageHeight);
			//Vector3d origin = new Vector3d (600, 0, 100);
			Vector3d origin = scene.origin;
			
			Vector3d target = scene.target;
			
			
			/* Note: some values were interpreted from this page: http://www.unknownroad.com/rtfm/graphics/rt_eyerays.html
			 * It is why there is a commented out pi/4
			 * */
		
			double fovx = 1.0;
			//Math.PI / 4.0;
			fovx *= 1.0 - 0.3;
			//enter fov here(on right)
			double fovy = fovx;
			
			fovx = Math.Tan (fovx);
			fovy = Math.Tan (fovy);
			
			//double fovz = 0.0000001;
			
			camz = target - origin;
			camz.Normalize ();
			Vector3d up = new Vector3d (0, 1, 0);
			
			camx = Vector3d.Cross (up, camz);
			camy = Vector3d.Cross (camx, -camz);
			
			#if CONSFLAG
			Console.WriteLine (camz.ToString ());
			Console.WriteLine (camx.ToString ());
			Console.WriteLine (camy.ToString ());
			#endif
			
			#if CONSFLAG
			rays = 0;
			killedrays = 0;
			shadowrays = 0;
			#endif
			
			
			double aacoef = 0.25;
			
			double[] pcol_ = new double[3];
			double[] pcol = new double[3];
			

			
			for (int x = 0; x < im.Width; x++) {
				for (int y = 0; y < im.Height; y++) {
					



					
					//int aan = 1;
				
					pcol_[0] = 0;
					pcol_[1] = 0;
					pcol_[2] = 0;
					pcol[0] = 0;
					pcol[1] = 0;
					pcol[2] = 0;
					
					
#if THREADING
					
					Task<double[]>[] task = new Task<double[]>[4];
					
					
					//
					
					Vector3d dirprime = (fovx * camx * (x - im.Width / 2)) + (fovy * camy * -(y - im.Height / 2)) + camz - origin;
					dirprime.Normalize ();
					
					threadinfo tinfo = new threadinfo(origin,dirprime,scene);

					task[0] = Task.Factory.StartNew (
								(ti__) 
								=>
		 						{ return threadtrace (ti__); },tinfo );
					
					//
					
					dirprime = (fovx * camx * (x + 0.5 - im.Width / 2)) + (fovy * camy * -(y - im.Height / 2)) + camz - origin;
					dirprime.Normalize ();
					
					tinfo = new threadinfo(origin,dirprime,scene);

					task[1] = Task.Factory.StartNew (
								(ti__) 
								=>
		 						{ return threadtrace (ti__); },tinfo );
					
					//
					
					dirprime = (fovx * camx * (x + 0.5 - im.Width / 2)) + (fovy * camy * -(y + 0.5 - im.Height / 2)) + camz - origin;
					dirprime.Normalize ();
					
					tinfo = new threadinfo(origin,dirprime,scene);

					task[2] = Task.Factory.StartNew (
								(ti__) 
								=>
		 						{ return threadtrace (ti__); },tinfo );
					//
					
					dirprime = (fovx * camx * (x - im.Width / 2)) + (fovy * camy * -(y + 0.5 - im.Height / 2)) + camz - origin;
					dirprime.Normalize ();
					
					tinfo = new threadinfo(origin,dirprime,scene);

					task[3] = Task.Factory.StartNew (
								(ti__) 
								=>
		 						{ return threadtrace (ti__); },tinfo );
					

		
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
					
					Color pixcol = Color.FromArgb ((int)pcol[0], (int)pcol[1], (int)pcol[2]);
					
#endif
					

						
#if NAIVE
					
					for (double offx = (double)x; offx <= (double)x + 0.5; offx += 0.5) {
						for (double offy = (double)y; offy <= (double)y + 0.5; offy += 0.5) {
							Vector3d dirprime = (fovx * camx * (offx - im.Width / 2)) + (fovy * camy * -(offy - im.Height / 2)) + camz - origin;
							//Vector3d dirprime = new Vector3d (offx - im.Width / 2, -(offy - im.Height / 2), 0) - origin;
							dirprime.Normalize ();
	
							

							
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
			
					Color pixcol = Color.FromArgb ((int)pcol[0], (int)pcol[1], (int)pcol[2]);
					
#endif
					
					im.SetPixel (x, y, pixcol);
				
				}
			}
			
			#if CONSFLAG
			Console.WriteLine ("Rays shot: " + Convert.ToString (rays));
			Console.WriteLine ("Rays that hit depth limit: " + Convert.ToString (killedrays));
			Console.WriteLine ("Shadow rays shot: " + Convert.ToString (shadowrays));
			#endif
		
			/* HACK */
		im.Save ("/Users/william/Dropbox/repos/volk-rend-csharp/volk-renderer/volk-renderer/bin/test.jpg");
			im.Save ("/Users/william/Dropbox/Public/test.jpg");
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
				#if CONSFLAG
				killedrays++;
				#endif
				double[] bcol = new double[3];
				bcol[0] = bcol[1] = bcol[2] = 0.0;
				return bcol;
			}
			
			#if CONSFLAG
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
				double[] pcol = new double[3];
				
				if (cobject.isLight ()) 
				{
					Color ccol = cobject.getColour(intersectp);
					return new double[3] {ccol.R,ccol.G,ccol.B};
				}
				
				
				foreach (Light li in scene.getLights ()) {
					Vector3d Lp = li.getPoint ();
					
					Vector3d L = Lp - intersectp;
					L.Normalize ();
					double dot = Vector3d.Dot (L, cobject.normal (intersectp));
					if (dot > 0) {
						
						double shade = shadowCheck (intersectp, li, scene, cobject);
						
						//diffuse multiplier
						double diff = li.getIntensity () * cobject.getDiffuse () * dot;
						//reflected ray off primitive
						Vector3d R = (2.0 * dot * cobject.normal (intersectp)) - L;
						//specular multiplier
						double spec = li.getIntensity () * cobject.getSpecular () * Math.Pow (Vector3d.Dot (R, direction), 20);
						
							/*pcolr*/pcol[0] += (shade * 
								(diff * cobject.getColour (intersectp).R 
									+ spec * li.getColour ().R));
						
							/*pcolg*/pcol[1] += (shade * 
								(diff * cobject.getColour (intersectp).G 
									+ spec * li.getColour ().G));
						
							/*pcolb*/pcol[2] += (shade * 
								(diff * cobject.getColour (intersectp).B 
									+ spec * li.getColour ().B));
					
					}
				
				}
				//ambient lighting 
				double ambient = cobject.getAmbient ();
				//double ambient = 1.0 / 3.0;
				
				pcol[0] += (ambient * cobject.getColour (intersectp).R);
				pcol[1] += (ambient * cobject.getColour (intersectp).G);
				pcol[2] += (ambient * cobject.getColour (intersectp).B);
				
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
				
				return pcol;
				

			
			} else {
				/* TODO */
				/* add environment mapping */
				Color sbcol = scene.getBack(origin,direction);
				return new double[3] {sbcol.R,sbcol.G,sbcol.B};
			}
		}
		
		
		private double shadowCheck (Vector3d p, Light li, vScene scene, Primitive cobject)
		{
			#if CONSFLAG
			shadowrays++;
			#endif
			
			double shade = 1.0;
			
			/* TODO */
			/*at some point (adding area lights) i'll have to change this so it accounts for all the points not just the center */
			Vector3d L = li.getPoint () - p;
			L.Normalize ();
			
			foreach (Primitive spr in scene.getPrims ())
			{
				if (spr != cobject && spr != li)
				{
					double objintersect = spr.intersect (p, L);
					if (objintersect > 0.000001) 
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
		
	}
	
	
}

