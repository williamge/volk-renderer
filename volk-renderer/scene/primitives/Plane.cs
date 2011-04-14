using System;
using OpenTK;
using System.Drawing;
namespace volkrenderer
{
	public class Plane : Primitive
	{
		Vector3d point,planeNormal;
		Color colour;
		
		double reflectd;
		double diffuse;
		double specular;
		double transparency;
		
		public Plane (Vector3d point_, Vector3d planeNormal_, Color colour_)
		{
			point = point_;
			planeNormal = planeNormal_;
			planeNormal.Normalize ();
			
			colour = colour_;
			
			reflectd = 0.0;
			diffuse = 2.0/3.0;
			specular = 0.0;
			transparency = 0.0;
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
		
		
		//get

		public Color getColour (Vector3d p)
		{
			return colour;
		}

		public double getDiffuse ()
		{
			return diffuse;
		}

		public double getSpecular ()
		{
			return specular;
		}

		public double getTransparency ()
		{
			return transparency;
		}

		
		public double getReflect ()
		{
			return reflectd;
		}

		//set

		
		public bool setColour (Color c_)
		{
			colour = c_;
			return true;
		}

		public bool setDiffuse (double d_)
		{
			diffuse = d_;
			return true;
		}

		public bool setSpecular (double s_)
		{
			specular = s_;
			return true;
		}

		public bool setReflect (double r_)
		{
			reflectd = r_;
			return true;
		}

		public bool setTransparency (double t_)
		{
			transparency = t_;
			return true;
		}
	}
}

