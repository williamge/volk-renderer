using System;
using OpenTK;
using System.Drawing;
namespace volkrenderer
{
	public class Plane : Primitive
	{
		Vector3d point,planeNormal;
		Color colour;
		
		public Plane (Vector3d point_, Vector3d planeNormal_, Color colour_)
		{
			point = point_;
			planeNormal = planeNormal_;
			planeNormal.Normalize ();
			colour = colour_;
		}
		
		public double intersect (Vector3d d0, Vector3d d1)
		{
			//L dot normal
			double denom = Vector3d.Dot (d1, planeNormal);
			//(P0-L0) dot normal			
			double numer = Vector3d.Dot (point - d0, planeNormal);
			
			if (denom != 0 && numer != 0) 
			{
				return numer/denom;
			}
			else {
				//intersection everywhere/nowhere, so just pick a number.
				return -1.0;
			}
			

		}
		
		public Vector3d normal (Vector3d point)
		{
			return planeNormal;
		}
		
		public Color getColour (Vector3d p)
		{
			return colour;
		}
		
		public double getDiffuse ()
		{
			return 0.6666666666666;
		}
		
		public double getSpecular ()
		{
			return 0.0;
		}
	}
}

