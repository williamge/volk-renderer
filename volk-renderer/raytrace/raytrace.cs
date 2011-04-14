using System;
using System.Drawing;
using OpenTK;
namespace volkrenderer
{
	public class raytrace
	{
		Bitmap im;
		
		public raytrace (vScene scene)
		{
			im = new Bitmap (scene.ImageWidth, scene.ImageHeight);
			Vector3d origin = new Vector3d (0, 0, - scene.ImageHeight);
			
			
			for (int x = 0; x < im.Width; x++) {
				for (int y = 0; y < im.Height; y++) {
					Vector3d direction = new Vector3d (x - im.Width / 2, -(y - im.Height / 2), scene.ImageHeight);
					direction = direction - origin;
					
				
					
					Vector3d dir1 = direction + new Vector3d (-0.5, 0.5, 0);
					Vector3d dir2 = direction + new Vector3d (-0.5, -0.5, 0);
					Vector3d dir3 = direction + new Vector3d (0.5, 0.5, 0);
					Vector3d dir4 = direction + new Vector3d (0.5, -0.5, 0);
					
					dir1.Normalize ();
					dir2.Normalize ();
					dir3.Normalize ();
					dir4.Normalize ();
					
					int l = 0;
					if(x==418 && y==317){
						l =2;
					}
					
					Color pix1 = trace (origin, dir1, scene,0);
					Color pix2 = trace (origin, dir2, scene,0+l);
					Color pix3 = trace (origin, dir3, scene,0);
					Color pix4 = trace (origin, dir4, scene,0);
					
					Color pixcol = Color.FromArgb(
						(int)((.25 * pix1.R)
							+(.25 * pix2.R)
							+(.25 * pix3.R)
							+(.25 * pix4.R)),
						(int)((.25 * pix1.G)
							+(.25 * pix2.G)
							+(.25 * pix3.G)
							+(.25 * pix4.G)),
						(int)((.25 * pix1.B)
							+(.25 * pix2.B)
							+(.25 * pix3.B)
							+(.25 * pix4.B))
						);
					
					
					
					//direction.Normalize ();
					//Color pixcol = trace(origin,direction,scene,0);
					im.SetPixel(x,y,pixcol);
					
				}
			}
		
			//just testing so far
			im.Save ("/Users/william/Dropbox/repos/volk-rend-csharp/volk-renderer/volk-renderer/bin/test.jpg");
			im.Save ("/Users/william/Dropbox/Public/test.jpg");
		}
		
		Color trace (Vector3d origin, Vector3d direction, vScene scene, int rdepth)
		{
			
			if (rdepth > 4) {
				return Color.Black;
			}

			
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
			
			if (rdepth == 2)
			{
				rdepth = rdepth;
			}
			
			if (ct > 0.0) {
				Vector3d intersectp = origin + direction * ct;
				
				Color pcol;
				int pcolr, pcolg, pcolb;
				pcolr = pcolg = pcolb = 0;
				
				
				foreach (Light li in scene.getLights ()) {
					foreach (Vector3d Lp in li.getPoints ()) {
						if (rdepth == 2) {
							rdepth = rdepth;
						}
						
						Vector3d L = Lp - intersectp;
						L.Normalize ();
						//Vector3d testn = cobject.normal (intersectp);
						double dot = Vector3d.Dot (L, cobject.normal (intersectp));
						if (dot > 0) {
							
							if (rdepth == 2) {
								rdepth = rdepth;
							}
							
							double shade = shadowCheck (intersectp, Lp, scene, cobject);
							
							//diffuse multiplier
							double diff = li.getIntensity () * cobject.getDiffuse () * dot;
							//reflected ray off primitive
							Vector3d R = (2 * dot * cobject.normal (intersectp)) - L;
							//specular multiplier
							double spec = li.getIntensity () * cobject.getSpecular () * Math.Pow (Vector3d.Dot (R, direction), 20);
							
							pcolr += (int)(shade * 
								(diff * cobject.getColour (intersectp).R 
									+ spec * li.getColour ().R));
							
							pcolg += (int)(shade * 
								(diff * cobject.getColour (intersectp).G 
									+ spec * li.getColour ().G));
							
							pcolb += (int)(shade * 
								(diff * cobject.getColour (intersectp).B 
									+ spec * li.getColour ().B));
							
							if (rdepth == 2) {
								rdepth = rdepth;
							}
						}
						
						
						
						
						
						
						
						//at some point do some shadows
					}
				
				}
				//ambient lighting 
				double ambient = (1.0 / 3.0);
				
				pcolr += (int)(ambient * cobject.getColour (intersectp).R);
				pcolg += (int)(ambient * cobject.getColour (intersectp).G);
				pcolb += (int)(ambient * cobject.getColour (intersectp).B);
				
				if (rdepth == 2) {
					rdepth = rdepth;
				}
				
				//reflection
				if (cobject.getReflect () > 0) {
					Color rcol = trace (intersectp, cobject.normal (intersectp), scene, rdepth+1);
					pcolr += (int)(cobject.getReflect () * rcol.R);
					pcolg += (int)(cobject.getReflect () * rcol.G);
					pcolb += (int)(cobject.getReflect () * rcol.B);
				}
				
				
				pcolr = Math.Max (Math.Min (255, pcolr), 0);
				pcolg = Math.Max (Math.Min (255, pcolg), 0);
				pcolb = Math.Max (Math.Min (255, pcolb), 0);
				
				pcol = Color.FromArgb (pcolr, pcolg, pcolb);
				
				return pcol;
				
				
				//should probably add some actual lighting
				//im.SetPixel (x, y, cobject.getColour(origin + direction*ct));
				//
			
			} else {
				//placeholder colour, should be black when ready.
				return scene.getBack();
			}
		}
		
		
		private double shadowCheck (Vector3d p, Vector3d Lp, vScene scene, Primitive cobject)
		{
			double shade = 1.0;
			Vector3d L = Lp - p;
			L.Normalize();
			
			foreach (Primitive spr in scene.getPrims ())
			{
				if (spr != cobject) 
				{
					double objintersect = spr.intersect (p, L);
					if (objintersect > 0.000001) 
					{
						double objtrans = spr.getTransparency ();
						if (objtrans == 0) 
						{
							//shade = shade / 2;
						}
						else{
							//shade = Math.Min(1.0, shade / objtrans);
						}
						
					}
				}
			}
			return shade;
		}
		
	}
	
	
}

