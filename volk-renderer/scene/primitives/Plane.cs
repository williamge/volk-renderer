using System;
using OpenTK;
using System.Drawing;
namespace volkrenderer
{
	public class Plane : Primitive
	{
		Vector3d point,planeNormal;
		
		Vector3d v,w;
		
		double[] colour;
		
		double reflectd;
		double ambient;
		double diffuse;
		double specular;
		double transparency;
		
		double[,,] texture;
		int tWidth, tHeight;
		
		public Plane (Vector3d point_, Vector3d planeNormal_, Color colour_)
		{
			point = point_;
			planeNormal = planeNormal_;
			planeNormal.Normalize ();
			
			v = new Vector3d (planeNormal.Y, planeNormal.Z, - planeNormal.X);
			w = Vector3d.Cross (v, planeNormal);
			v.Normalize ();
			w.Normalize ();
			
			colour = new double[3];
			colour[0] = colour_.R;
			colour[1] = colour_.G;
			colour[2] = colour_.B;
			
			texture = null;
			
			reflectd = 0.5;
			ambient = 0.0;
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
				if (numer / denom > 10000) {//have to find a good value for this
					return -1.0;}
				
				
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

		public double[] getColour (Vector3d p)
		{
			
			if (texture != null) 
			{
				double s1, t1;
				double s, t;
			
				s1 = Vector3d.Dot (p - point, v);
				t1 = Vector3d.Dot (p - point, w);
				
				s = s1 % tWidth;
				t = t1 % tHeight;
			
				s = Math.Abs (s);
				t = Math.Abs (t);
				
				//double pr, pg, pb;
				double[] pc = new double[3];
				
				int x1 = (int)Math.Floor (s);
				int x2 = (int)Math.Ceiling (s);
				int x2x1 = x2 - x1;
				if (x2 >= tWidth) {
					x2 = 0;
				
				}
				//int x2 = Math.Min ((int)Math.Ceiling (s), texture.Width-1);
				
				int y1 = (int)Math.Floor (t);
				//int y2 = Math.Min ((int)Math.Ceiling (t), texture.Height-1);
				int y2 = (int)Math.Ceiling (t);
				int y2y1 = y2 - y1;
				if (y2 >= tHeight) 
				{
					y2 = 0;
				}
				
				int bxx, byy;
				bxx = byy = 1;

				if (x2x1 <= 0) {
					bxx = 0;
					x2x1 = 1;
				}
				
				if (y2y1 <= 0) {
					byy = 0;
					y2y1 = 1;
				}
				
				
				if (y2y1 * x2x1 <= 0) {
					byy = 0;
					y2y1 = 1;
					bxx = 0;
					x2x1 = 1;
				}
				
				
				pc[0] = byy * (texture[x1, y1, 0] / (x2x1 * y2y1)) * ((x2 - s) * (y2 - t)) 
						+ bxx * (texture[x2, y1, 0] / (x2x1 * y2y1)) * ((s - x1) * (y2 - t)) 
						+ byy * (texture[x1, y2, 0] / (x2x1 * y2y1)) * ((x2 - s) * (t - y1)) 
						+ bxx * (texture[x2, y2, 0] / (x2x1 * y2y1)) * ((s - x1) * (t - y1));
				
				pc[1] = byy * (texture[x1, y1, 1] / (x2x1 * y2y1)) * ((x2 - s) * (y2 - t)) 
						+ bxx * (texture[x2, y1, 1] / (x2x1 * y2y1)) * ((s - x1) * (y2 - t)) 
						+ byy * (texture[x1, y2, 1] / (x2x1 * y2y1)) * ((x2 - s) * (t - y1)) 
						+ bxx * (texture[x2, y2, 1] / (x2x1 * y2y1)) * ((s - x1) * (t - y1));
				
				pc[2] = byy * (texture[x1, y1, 2] / (x2x1 * y2y1)) * ((x2 - s) * (y2 - t)) 
						+ bxx * (texture[x2, y1, 2] / (x2x1 * y2y1)) * ((s - x1) * (y2 - t)) 
						+ byy * (texture[x1, y2, 2] / (x2x1 * y2y1)) * ((x2 - s) * (t - y1)) 
						+ bxx * (texture[x2, y2, 2] / (x2x1 * y2y1)) * ((s - x1) * (t - y1));
				
				/*pr = Math.Max(Math.Min(pr,255),0);
				pg = Math.Max(Math.Min(pg,255),0);
				pb = Math.Max(Math.Min(pb,255),0);*/
		
				return pc;//Color.FromArgb((int)pr,(int)pg,(int)pb);
				
				
			}
			
			
			return colour;
		}
		
		public double getAmbient ()	
		{
			return ambient;
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
		
		public double getRefract ()
		{
			return 1.0;
		}
		
		public bool isLight (){return false;}

		//set

		
		public bool setColour (Color c_)
		{
			colour[0] = c_.R;
			colour[1] = c_.G;
			colour[2] = c_.B;
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
			
			//texture = im_;
			return true;
			
		}
	}
}

