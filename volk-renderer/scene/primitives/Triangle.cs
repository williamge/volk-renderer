using System;
using OpenTK;
using System.Drawing;

namespace volkrenderer
{
	public class Triangle : Primitive
	{
		private Vector3d p1, p2, p3;
		private Vector3d normalt, norm1, norm2;
		private Vector3d ubasis,vbasis;
		private double td1, td2;
		double[] colour;
		
		double[,,] texture;
		int tWidth,tHeight;
		
		double diffuse, specular, transparency, reflect, ambient;
		
		
		public Triangle (Vector3d p1_, Vector3d p2_, Vector3d p3_, Color colour_)
		{
			p1 = p1_;
			p2 = p2_;
			p3 = p3_;
			
			ubasis = p2 - p1;
			vbasis = p3 - p1;
			
			normalt = Vector3d.Cross (ubasis, vbasis);

			
			norm1 = Vector3d.Cross (vbasis, normalt)/Math.Pow(normalt.Length,2);
			norm2 = Vector3d.Cross (normalt, ubasis)/Math.Pow(normalt.Length,2);
			
			td1 = Vector3d.Dot(- norm1,p1);
			td2 = Vector3d.Dot(- norm2,p1);
			
			normalt.Normalize ();
			
			if (normalt == Vector3d.Zero){throw new NotSupportedException();}
			
			colour = new double[3] { colour_.R, colour_.G, colour_.B };
			
			diffuse = 1.0;
			specular = 0.5;
			transparency = 0.0;
			reflect = 0.0;
			ambient = 0.3;
			
			texture = null;
		
		}
	
		#region Primitive implementation
		public double intersect (Vector3d d0, Vector3d d1)
		{			
			/* triangle intersection code taken from:
			 * http://igad.nhtv.nl/~bikker/files/faster.pdf
			 * it might be wrong but i really hope it's right */
			
			//L dot normal
			double denom = Vector3d.Dot (d1, normalt);
			//(P0-L0) dot normal			
			double numer = Vector3d.Dot (p1 - d0, normalt);
			
			if (denom == 0 || numer == 0)
			{
				return -1.0;
			}
			
			double intersectt = numer/denom;

			
			Vector3d tpoint = d0 + (intersectt * d1);
			
			double u = Vector3d.Dot(norm1,tpoint) + td1;
			if (u < 0){return -1.0;}
			double v = Vector3d.Dot(norm2,tpoint) + td2;
			if (v < 0 || u + v > 1){return -1.0;}
			
			return intersectt;
			/* perhaps a short cache for the u,v values at a point would speed this up,
			 * instead of recomputing u and v, however small they are. (maybe .net wouldn't
			 * let this be very efficient, look into it) */

		}

		public Vector3d normal (Vector3d point)
		{
			return normalt;
		}

		public double[] getColour (Vector3d p)
		{
			if (texture != null) {
				throw new NotImplementedException ();
				/*
				double u = Vector3d.Dot (norm1, tpoint) + td1;
				if (u < 0) { throw exception;}
				double v = Vector3d.Dot (norm2, tpoint) + td2;
				if (v < 0 || u + v > 1) { throw exception;}
				*/
				
				//here's some code that gets the u,v to look up in the texture, probably would work.
			}
			
			return colour;
		}
		
		//set
		
		public bool setColour (Color c_)
		
		{
			colour[0] = c_.R;
			colour[1] = c_.G;
			colour[2] = c_.B;
			return true;
		}
		
		public bool setAmbient (double a_)
		
		{
			ambient = a_;
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
			reflect = r_;
			return true;
		}
		
		public bool setTransparency (double t_)
		{
			transparency = t_;
			return true;
		}
		
		public bool setTexture (Bitmap text_)

		{
			
			texture = new double[text_.Width, text_.Height, 3];
			
			for (int i = 0; i < text_.Width; i++) {
				for (int j = 0; j < text_.Height; j++) {
					Color tcol = text_.GetPixel (i, j);
					texture[i, j, 0] = (double)tcol.R;
					texture[i, j, 1] = (double)tcol.G;
					texture[i, j, 2] = (double)tcol.B;
				
				}
			
			}
			
			tWidth = text_.Width;
			tHeight = text_.Height;
			
			//texture = text_;
			return true;
		}
		
		//get

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
			return reflect;
		}

		public double getAmbient ()
		{
			return ambient;
		}
		
		public double getRefract ()
		
		{
			return 1.0;
		}

		public bool isLight ()
		{
			return false;
		}
		
		public RPatch[] RSplit (int gridSize)
		{
			throw new NotImplementedException ();
		}
		#endregion

}
}

