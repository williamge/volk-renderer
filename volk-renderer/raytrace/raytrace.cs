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
					Vector3d direction = new Vector3d (x - im.Width/2, y - im.Height/2, 0);
					direction = direction - origin;
					direction.Normalize ();
					
					//closest t so far
					double ct = 0.0;
					Primitive cobject;
					
					//loop through all objects in scene.
					foreach (Primitive pr in scene.prims) {
						double iresult = pr.intersect (origin, direction);
						
						if (iresult > 0) 
						{
							if (iresult < ct || ct == 0.0) 
							{
								ct = iresult;
								cobject = pr;
							}
						}
					}
					
					//ready to do lighting
					
					if (ct > 0.0){
					
						//should probably add some actual lighting
						im.SetPixel (x, y, cobject.getColour());
						//
					
					}
					else{
						//placeholder colour, should be black when ready.
						im.SetPixel (x, y, Color.HotPink);
					}
				}
			}
		
			im.Save("/Users/william/Downloads/test.jpg");
		}
	}
}

