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
			Vector3d origin = new Vector3d (0, 0, -scene.ImageHeight);
			
			
			for (int x = 0; x < im.Width; x++) {
				for (int y = 0; y < im.Height; y++) {

					Vector3d direction = new Vector3d (x - im.Width/2, -(y - im.Height/2), 0);
					direction = direction - origin;
					direction.Normalize ();
					
					//closest t so far
					double ct = 0.0;
					Primitive cobject = new Sphere(Color.Red,new Vector3d(0,0,0),2);
					
					//loop through all objects in scene.
					foreach (Primitive pr in scene.getPrims()) {
						double iresult = pr.intersect (origin, direction);
						
						if (iresult > 0.0000001) 
						{
							if (ct == 0.0 || iresult < ct) 
							{
								ct = iresult;
								cobject = pr;
							}
						}
					}
					
					//ready to do lighting
					
					if (ct > 0.0){
						Vector3d intersectp = origin + direction*ct;
						
						//diffuse only for now
						Color pcol = Color.Black;
						int pcolr,pcolg,pcolb;
						pcolr = pcolg = pcolb = 0;

						
						foreach (Light li in scene.getLights()){
							foreach (Vector3d Lp in li.getPoints()){
								Vector3d L = Lp - intersectp;
								L.Normalize();
								//Vector3d testn = cobject.normal (intersectp);
								double dot = Vector3d.Dot (L, cobject.normal (intersectp));
								if (dot > 0){
									
									//diffuse multiplier
									double diff = cobject.getDiffuse() * dot;
									//reflected ray off primitive
									Vector3d R = (2 * dot * cobject.normal(intersectp)) - L;
									//specular multiplier
									double spec = cobject.getSpecular() * Math.Pow(Vector3d.Dot(R, direction),20);
									
									pcolr += (int) (diff*cobject.getColour(intersectp).R 
										+ spec * li.getColour().R);										
									
									pcolg += (int)(diff * cobject.getColour (intersectp).G 
										+ spec * li.getColour ().G);
									
									pcolb += (int)(diff * cobject.getColour (intersectp).B 
										+ spec * li.getColour ().B);
								}
								
								
								

								

								
								//at some point do some shadows
							}

						}
						//ambient lighting 
						pcolr += (int)((1.0 / 3.0) * cobject.getColour (intersectp).R);
						pcolg += (int)((1.0 / 3.0) * cobject.getColour (intersectp).G);
						pcolb += (int)((1.0 / 3.0) * cobject.getColour (intersectp).B);
						
						pcolr = Math.Max(Math.Min(255,pcolr),0);
						pcolg = Math.Max(Math.Min(255,pcolg),0);
						pcolb = Math.Max(Math.Min(255,pcolb),0);
					
						pcol = Color.FromArgb(pcolr,pcolg,pcolb);

						im.SetPixel(x,y,pcol);
						
					
						//should probably add some actual lighting
						//im.SetPixel (x, y, cobject.getColour(origin + direction*ct));
						//
					
					}
					else{
						//placeholder colour, should be black when ready.
						im.SetPixel (x, y, Color.HotPink);
					}
				}
			}
		
			//just testing so far
			im.Save("/Users/william/Dropbox/repos/volk-rend-csharp/volk-renderer/volk-renderer/bin/test.jpg");
		}
	}
}

