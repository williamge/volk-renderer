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
		public Vector3d origin;
		public Vector3d target;
		public double exposure;
		
		public vScene (int width_, int height_)
		{
			ImageWidth = width_;
			ImageHeight = height_;
			
			origin = new Vector3d (0, 0, -480);
			target = new Vector3d (0, 0, 1);
			
			exposure = -2.0f;
		}
		
		public bool addPrim (Primitive p_)
		
		{
			prims.Add (p_);
			return true;
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
			lights.Add (new PointLight (p_, col_,this));
			return true;
		}
		
		public bool addPointLight (Vector3d p_, Color col_, double t_)
		{
			lights.Add (new PointLight (p_, col_,t_,this));
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
		
		public Color getBack (Vector3d origin, Vector3d direction)
		{
			return Color.Black;
		}
		
	}
}

