using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
namespace volkrenderer
{
	public class vScene
	{
		public int ImageWidth, ImageHeight;
		
		List<Primitive> prims = new List<Primitive> ();
		List<Light> lights = new List<Light>(); 
		
		public vScene (int width_, int height_)
		{
			ImageWidth = width_;
			ImageHeight = height_;
		}
		
		public bool addSphere (Vector3d center, int radius, Color colour)
		{
			if (radius < 0) 
			{
				return false;
			}
			prims.Add (new Sphere (colour, center, radius));
			return true;
		}
		
		public bool addSphere (Vector3d center, int radius, Color colour,double reflectd_)
		{
			if (radius < 0) {
				return false;
			}
			prims.Add (new Sphere (colour, center, radius,reflectd_));
			return true;
		}
		
		public bool addPlane (Vector3d point, Vector3d normal, Color colour)

		{
			prims.Add (new Plane (point, normal, colour));
			return true;
		}
		
		public bool addPointLight (Vector3d p_, Color col_)

		{
			lights.Add (new PointLight (p_, col_));
			return true;
		}
		
		public bool addPointLight (Vector3d p_, Color col_, double t_)
		{
			lights.Add (new PointLight (p_, col_,t_));
			return true;
		}
		
		public List<Primitive> getPrims ()
		{
			return prims;
		}
		
		public List<Light> getLights ()
		
		{
			return lights;
		}
		
		public Color getBack ()
		{
			return Color.HotPink;
		}
		
	}
}

