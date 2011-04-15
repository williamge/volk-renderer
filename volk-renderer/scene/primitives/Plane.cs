using System;
using OpenTK;
using System.Drawing;
namespace volkrenderer
{
	public class Plane : Primitive
	{
		Vector3d point,planeNormal;
		
		Vector3d v,w;
		
		Color colour;
		
		double reflectd;
		double ambient;
		double diffuse;
		double specular;
		double transparency;
		
		Bitmap texture;
		
		public Plane (Vector3d point_, Vector3d planeNormal_, Color colour_)
		{
			point = point_;
			planeNormal = planeNormal_;
			planeNormal.Normalize ();
			
			v = new Vector3d (planeNormal.Y, planeNormal.Z, - planeNormal.X);
			w = Vector3d.Cross (v, planeNormal);
			v.Normalize ();
			w.Normalize ();
			
			colour = colour_;
			
			texture = null;
			
			reflectd = 0.0;
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

		public Color getColour (Vector3d p)
		{
			
			if (texture != null) 
			{
				double s1, t1;
				
				double s, t;

			
				s1 = Vector3d.Dot (p - point, v);
				t1 = Vector3d.Dot (p - point, w);
				
			


				
				s = s1 % texture.Width;
				t = t1 % texture.Height;
				
				s = Math.Abs (s);
				t = Math.Abs (t);
				

				
				double pr, pg, pb;
				

				
				
				int x1 = (int)Math.Floor (s);
				int x2 = (int)Math.Ceiling (s);
				int x2x1 = x2 - x1;
				if (x2 >= texture.Width) {
					x2 = 0;
				
				}
				//int x2 = Math.Min ((int)Math.Ceiling (s), texture.Width-1);
				
				int y1 = (int)Math.Floor (t);
				//int y2 = Math.Min ((int)Math.Ceiling (t), texture.Height-1);
				int y2 = (int)Math.Ceiling (t);
				int y2y1 = y2 - y1;
				if (y2 >= texture.Height) 
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
				
				
				pr = 	byy*(texture.GetPixel(x1,y1).R / (x2x1*y2y1)) * ((x2-s)*(y2-t)) 
						+ bxx *(texture.GetPixel(x2,y1).R / (x2x1*y2y1)) * ((s-x1)*(y2-t)) 
						+ byy*(texture.GetPixel(x1,y2).R / (x2x1*y2y1)) * ((x2-s)*(t-y1)) 
						+ bxx * (texture.GetPixel(x2,y2).R / (x2x1*y2y1)) * ((s-x1)*(t-y1));
				
				pg = 	byy*(texture.GetPixel(x1,y1).G / (x2x1*y2y1)) * ((x2-s)*(y2-t)) 
						+ bxx *(texture.GetPixel(x2,y1).G / (x2x1*y2y1)) * ((s-x1)*(y2-t)) 
						+ byy*(texture.GetPixel(x1,y2).G / (x2x1*y2y1)) * ((x2-s)*(t-y1)) 
						+ bxx * (texture.GetPixel(x2,y2).G / (x2x1*y2y1)) * ((s-x1)*(t-y1));
				
				pb = 	byy*(texture.GetPixel(x1,y1).B / (x2x1*y2y1)) * ((x2-s)*(y2-t)) 
						+ bxx *(texture.GetPixel(x2,y1).B / (x2x1*y2y1)) * ((s-x1)*(y2-t)) 
						+ byy*(texture.GetPixel(x1,y2).B / (x2x1*y2y1)) * ((x2-s)*(t-y1)) 
						+ bxx * (texture.GetPixel(x2,y2).B / (x2x1*y2y1)) * ((s-x1)*(t-y1));
				
				pr = Math.Max(Math.Min(pr,255),0);
				pg = Math.Max(Math.Min(pg,255),0);
				pb = Math.Max(Math.Min(pb,255),0);
				
				return Color.FromArgb((int)pr,(int)pg,(int)pb);
				
				
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
		
		public bool isLight (){return false;}

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
		
		public bool setTexture (Bitmap im_)
		{
			texture = im_;
			return true;
			
		}
	}
}

