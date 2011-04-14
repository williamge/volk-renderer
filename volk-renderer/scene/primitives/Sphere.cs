using System;
using System.Drawing;
using OpenTK;
namespace volkrenderer
{
	public class Sphere : Primitive
	{
		Color colour;
		Vector3d center;
		int radius;
		
		public Sphere (Color colour0, Vector3d center0, int radius0)
		{
			colour = colour0;
			center = center0;
			radius = radius0;
		}
		
		public double intersect (Vector3d d0, Vector3d d1)
		{
			
			
			Vector3d v = d0 - center;
			
			double t, t2;
			
			t2 = Math.Pow (Vector3d.Dot (v, d1), 2) - (Vector3d.Dot (v, v) - Math.Pow (radius, 2));
			
			if (t2 < 0) {
				return -1;
			}
			
			t2 = Math.Sqrt (t2);
			t = - Vector3d.Dot (v, d1);
			t = Math.Min (t + t2, t - t2);
			
			return t;
		
			
		}
		
		//probably should check if 'point' is actually on the sphere but who cares at this point.
		public Vector3d normal (Vector3d point)
		{
			Vector3d N = point - center;
			N.Normalize ();
			return N;
		}
		
		public Color getColour (Vector3d p)
		{
			return colour;
		}
		
		public double getDiffuse ()
		{
			return (1.0 / 3.0);
		}
		
		public double getSpecular ()
		{
			return (1.0 / 3.0);
		}
		
		public double getTransparency ()
		{
			return 0.0;
		}
	}
}

