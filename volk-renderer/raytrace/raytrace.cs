#define THREADING
#define CONSFLAGD
#define AMBIENT
#define DIFFUSE
#define SPECULAR
#define SHADOWS
#define REFLECTION
#define REFRACTION

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
		
		bool cameraSetFlag;
		
		int DEPTHLIMIT;
		
		Vector3d camx,camy,camz;
		Vector3d origin__;
		vScene scene;
		
		/* for performance reasons (to avoid the use of Bitmap.getPixel/setPixel) the image (and textures)
		 * are stored in an array of doubles and during the image save (and texture load) the doubles are
		 * converted to an image */
		double[,,] dimage;
		
		#if CONSFLAGD
		Int64 rays;
		Int64 killedrays;
		Int64 shadowrays;
		#endif
		
		/// <summary>
		/// Constructor for a Raytracer object to be constructed from a Vscene object.
		/// </summary>
		/// <param name="scene_">
		/// The vScene object that will be raytraced. <see cref="vScene"/>
		/// </param>
		public raytrace (vScene scene_)
		{
			scene = scene_;
			
			im = new Bitmap (scene.ImageWidth, scene.ImageHeight);
			dimage = null;
			
			DEPTHLIMIT = scene.depth;
			
			cameraSetFlag = false;
		}
		
		/// <summary>
		/// Sets the camera and constructs the camera axis for the scene.
		/// </summary>
		/// <param name="origin_">
		/// Origin point of the scene <see cref="Vector3d"/>
		/// </param>
		/// <param name="target_">
		/// Targeted point <see cref="Vector3d"/>
		/// </param>
		/// <returns>
		/// True on success, false otherwise.
		/// </returns>
		public bool setCamera (Vector3d origin_, Vector3d target_)
		{
			
			if (origin_ == target_) {
				return false;
			}
			
			origin__ = origin_;
			
			camz = target_ - origin__;
			camz.Normalize ();
			
			Vector3d up = new Vector3d (0, 1, 0);
			
			camx = Vector3d.Cross (up, camz);			
			//if (up = scalar times camz) then we should choose a different up 
			camx = camx != Vector3d.Zero ? camx : Vector3d.Cross (new Vector3d (0, 0, 1), camz);
			
			camy = Vector3d.Cross (camx, -camz);
			
			cameraSetFlag = true;
			
			return true;
		}
		
		/// <summary>
		/// Starts the raytracing for this raytracer object.
		/// </summary>
		public void RStart(){
			
			if (!cameraSetFlag){
			
				if (!setCamera(scene.origin,scene.target)){throw new ArgumentException();}
			}
			
			dimage = new double[scene.ImageWidth, scene.ImageHeight, 3];
			
			/* Note: some values were interpreted from this page: http://www.unknownroad.com/rtfm/graphics/rt_eyerays.html
			 * It is why there is a commented out pi/4
			 * */
			
			//Math.PI / 4.0;
			//enter fov here(on right)
			double fovx = 1.0 - scene.fov;
			double fovy = fovx;
			
			fovx = Math.Tan (fovx);
			fovy = Math.Tan (fovy);
			
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
			
			double aacoef = 0.25;

			int iheight = im.Height;
			int iwidth = im.Width;
			
			Vector3d camzz = camz * iwidth;
			
			//we make a new variable to try and stop any blocking from threads on accessing origin__
			Vector3d origin = origin__;
			
#if THREADING			
			Parallel.For(0,iwidth,delegate(int x)	{
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
			double[] pcol_ = new double[3];
			double[] pcol = new double[3];
					
			for (int x = 0; x < iwidth; x++) {
				for (int y = 0; y < iheight; y++) {
				
					pcol_[0] = 0;
					pcol_[1] = 0;
					pcol_[2] = 0;
					pcol[0] = 0;
					pcol[1] = 0;
					pcol[2] = 0;

					
#endif
					

					
					for (double offx = (double)x; offx <= (double)x + 0.5; offx += 0.5) {
						for (double offy = (double)y; offy <= (double)y + 0.5; offy += 0.5) {
							Vector3d dirprime = ((fovx * camx * (offx - iwidth / 2)) + (fovy * camy * -(offy - iheight / 2)) + camzz) ;
							dirprime.Normalize ();
											
							pcol_ = trace (origin, dirprime, scene, 0);
							pcol[0] += aacoef * pcol_[0];
							pcol[1] += aacoef * pcol_[1];
							pcol[2] += aacoef * pcol_[2];
						}
					}	
					
					dimage[x,y,0] = pcol[0];
					dimage[x,y,1] = pcol[1];
					dimage[x,y,2] = pcol[2];									
	
				}
			}
#if THREADING
			);
#endif
			#if CONSFLAGD
			Console.WriteLine ("Rays shot: " + Convert.ToString (rays));
			Console.WriteLine ("Rays that hit depth limit: " + Convert.ToString (killedrays));
			Console.WriteLine ("Shadow rays shot: " + Convert.ToString (shadowrays));
			#endif	

		}
		
		/// <summary>
		/// Traces a ray from origin to direction in scene.
		/// </summary>
		/// <param name="origin">
		/// Origin of the ray <see cref="Vector3d"/>
		/// </param>
		/// <param name="direction">
		/// Direction vector of the ray <see cref="Vector3d"/>
		/// </param>
		/// <param name="scene">
		/// Scene for the ray to be fired in <see cref="vScene"/>
		/// </param>
		/// <param name="rdepth">
		/// Current raytracing-recursion depth
		/// </param>
		/// <returns>
		/// The colour for the ray stored in a double[3] object.
		/// </returns>
		private double[] trace (Vector3d origin, Vector3d direction, vScene scene, int rdepth)
		{
			
			if (rdepth > DEPTHLIMIT) {
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
							
						
						//specular multiplier
						double spec = 0.0;
							
						#if SPECULAR
							if (cobject.getSpecular() > 0.0){
								//reflected ray off primitive
								Vector3d R = (2.0 * dot * cobject.normal (intersectp)) - L;
								spec = li.getIntensity () * cobject.getSpecular () * Math.Pow (Vector3d.Dot (R, direction), 20);	
							}							
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
				
				Vector3d N = cobject.normal (intersectp);
				#if REFLECTION
				//reflection
				if (cobject.getReflect () > 0) {
					
					
					Vector3d reflectray = direction - (2.0 * (Vector3d.Dot (N, direction))) * N;
					reflectray.Normalize ();
					
					double[] rcol = trace (intersectp, reflectray,
										scene, rdepth+1);
					pcol[0] += (cobject.getReflect () * rcol[0]);
					pcol[1] += (cobject.getReflect () * rcol[1]);
					pcol[2] += (cobject.getReflect () * rcol[2]);
				}
				#endif
					
				#if REFRACTION
				if (cobject.getTransparency() > 0.0){
						
						double n = 1.0/cobject.getRefract();
						double cosI = - Vector3d.Dot(N,direction);
						double cosT2 = (1.0 - n * n) * (1.0 - cosI * cosI);
						if (cosT2 > 0.0){
							Vector3d T = (n * direction) + (n * cosI - Math.Sqrt(cosT2)) * N;
							T.Normalize();
							double[] rrcol = trace(intersectp + 0.15 * T,T,scene,rdepth + 1);
							pcol[0] += (cobject.getTransparency() * rrcol[0]);
							pcol[1] += (cobject.getTransparency() * rrcol[1]);
							pcol[2] += (cobject.getTransparency() * rrcol[2]);
						}
					}
				#endif
				
				return pcol;
				

			
			} else {
				/* TODO */
				/* add environment mapping */
				return scene.getBack(origin,direction);
			}
		}
		
		/// <summary>
		/// Checks for the 'shadow factor' of the point p in scene.
		/// </summary>
		/// <param name="p">
		/// Intersection point of the ray <see cref="Vector3d"/>
		/// </param>
		/// <param name="li">
		/// The light to check for shadows from <see cref="Light"/>
		/// </param>
		/// <param name="scene">
		///  <see cref="vScene"/>
		/// </param>
		/// <param name="cobject">
		/// So the object doesn't cast shadows all over itself from floating point precision errors <see cref="Primitive"/>
		/// </param>
		/// <returns>
		/// The 'shadow factor' of the point for light li.
		/// </returns>
		private double shadowCheck (Vector3d p, Light li, vScene scene, Primitive cobject)
		{
			#if CONSFLAGD
			shadowrays++;
			#endif
			
			double shade = 1.0;
			
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
				
		/// <summary>
		/// Colour corrects the floating point image for the raytracer object, corrects for exposure, gamma and clamps the colour in [0,255]
		/// </summary>
		private void colourCorrection ()
		{
			
			//exposures, may not be useful idk
			double exposure = scene.exposure;
			
			for (int x = 0; x < im.Width; x++) {
				for (int y = 0; y < im.Height; y++) {

					dimage[x,y,0] = 255.0 * (1.0 - Math.Exp (dimage[x,y,0] / 255.0 * exposure));
					dimage[x,y,1] = 255.0 * (1.0 - Math.Exp (dimage[x,y,1] / 255.0 * exposure));
					dimage[x,y,2] = 255.0 * (1.0 - Math.Exp (dimage[x,y,2] / 255.0 * exposure));
			
					//gamma correction, may be wrong or unnecessary
					dimage[x,y,0] = dimage[x,y,0] * Math.Pow (dimage[x,y,0] / 255.0, 1.0 / 2.2);
					dimage[x,y,1] = dimage[x,y,1] * Math.Pow (dimage[x,y,1] / 255.0, 1.0 / 2.2);
					dimage[x,y,2] = dimage[x,y,2] * Math.Pow (dimage[x,y,2] / 255.0, 1.0 / 2.2);
			
					dimage[x,y,0] = Math.Max (Math.Min (255, dimage[x,y,0]), 0);
					dimage[x,y,1] = Math.Max (Math.Min (255, dimage[x,y,1]), 0);
					dimage[x,y,2] = Math.Max (Math.Min (255, dimage[x,y,2]), 0);
				}
			}
		}
				
		/// <summary>
		/// Saves the image stored in the double array to a jpeg file. Requires the raytrace to have been completed.
		/// </summary>
		/// <param name="path">
		/// Path for the file to be saved.
		/// </param>
		/// <returns>
		/// True on success, false otherwise.
		/// </returns>	
		public bool imageSave (string path)
		{
			if (dimage == null) {
				return false;
			}
			
			colourCorrection ();
					
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
			
			return true;
		}
		
	}

	
	
}

