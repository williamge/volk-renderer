using System;
using System.Drawing;
using OpenTK;
using System.Collections.Generic;
namespace volkrenderer
{
	public class PointLight : Light, Primitive
	{
		List<Vector3d> points;
		Vector3d point;
		double[] colour;
		double intensity;
		
		//Sphere fields
		int radius;
		
		public PointLight (Vector3d p, Color col_, vScene scene)
		{
			points = new List<Vector3d> ();
			points.Add (p);
			point = p;
			colour = new double[3];
			colour[0] = col_.R;
			colour[1] = col_.G;
			colour[2] = col_.B;
			
			intensity = 1.0;
			radius = (int)(5.0 * intensity);
			//viewSphere = new LightSphere (col_, p, (int)(5.0 * intensity),this);
			scene.addPrim (this);
		}
		
		public PointLight (Vector3d p, Color col_, double intensity_, vScene scene)
		{
			points = new List<Vector3d> ();
			points.Add (p);
			point = p;
			
			colour = new double[3];
			colour[0] = col_.R;
			colour[1] = col_.G;
			colour[2] = col_.B;
			
			intensity = intensity_;
			radius = (int)(5.0 * intensity);
			//viewSphere = new LightSphere (col_, p, (int)(5.0 * intensity),this);
			scene.addPrim (this);
		}
		
		public List<Vector3d> getPoints ()
		{
			return points;
		}
		public Vector3d getPoint ()
		{
			return point;
		}
		
		public double[] getColour ()
		{
			return colour;
		}
		
		public double getIntensity ()
		{
			return intensity;
		}
		
	

		#region Primitive implementation
		public double intersect (Vector3d d0, Vector3d d1)
		{
			Vector3d v = d0 - point;
			
			double t, t2;
			
			t2 = Math.Pow (Vector3d.Dot (v, d1), 2) - (Vector3d.Dot (v, v) - Math.Pow (radius, 2));
			
			if (t2 < 0) {
				return -1;
			}
			
			t2 = Math.Sqrt (t2);
			t = -Vector3d.Dot (v, d1);
			t = Math.Min (t + t2, t - t2);
			
			return t;
		}

		public Vector3d normal (Vector3d point_)
		{
			Vector3d N = point_ - point;
			N.Normalize ();
			return N;
		}

		public double[] getColour (Vector3d p)
		{
			return colour;
		}

		public double getDiffuse ()
		{
			return 0.0;
		}

		public double getSpecular ()
		{
			return 0.0;
		}

		public double getTransparency ()
		{
			return 0.0;
		}

		public double getReflect ()
		{
			return 0.0;
		}

		public double getAmbient ()
		{
			return 1.0;
		}

		public bool isLight ()
		{
			return true;
		}
		#endregion
}
}

