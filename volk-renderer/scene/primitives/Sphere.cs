using System;
using System.Drawing;
using OpenTK;
namespace volkrenderer
{
	public class Sphere : Primitive
	{
		Color colour;
		Vector3d center;
		Vector3d vnorth;
		Vector3d ve;
		Vector3d vc;
		int radius;
		double reflectd;
		double ambient;
		double diffuse;
		double specular;
		double transparency;
		
		Bitmap texture;
		
		public Sphere (Color colour0, Vector3d center0, int radius0)
		{
			colour = colour0;
			center = center0;
			radius = radius0;
			
			vnorth = new Vector3d (0, 1, 0) ;
			ve = new Vector3d(1,0,0);
			vc = Vector3d.Cross(vnorth,ve);
			
			reflectd = 0.0;
			
			diffuse = 1.0/3.0;
			specular = 1.0/3.0;
			transparency = 0.0;
			ambient = 1.0/3.0;
			
			texture = null;
		}
		
		public Sphere (Color colour0, Vector3d center0, int radius0, double reflectd_)
		{
			colour = colour0;
			center = center0;
			radius = radius0;
			
			reflectd = reflectd_;
			diffuse = 1.0 / 3.0;
			specular = 1.0 / 3.0;
			transparency = 0.0;
			ambient = 1.0 / 3.0;
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
		
		
		//get
		
		public Color getColour (Vector3d p)
		{
			
			/* TODO */
			/* some kind of interpolation to clean up textures */
			if (texture != null) 
			{
				Vector3d vp = (p - center) * 1.0/radius;
				double r = Math.Acos (Vector3d.Dot (-vp, vnorth));
				double v = (r/Math.PI) * texture.Width;
				double u;
				
				double theta = Math.Acos(Vector3d.Dot(new Vector3d(1,0,0),vp / Math.Sin(r))) / (2.0 * Math.PI);
				
				if (Vector3d.Dot (vc, vp) <0)
					u = (1.0f - theta) * texture.Height;
				else
					u = theta * texture.Height;
				

				
				return texture.GetPixel(Math.Min( (int)Math.Floor(u),texture.Width - 1 ),
										Math.Min( (int)Math.Floor (v), texture.Height - 1));
				
				
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
		
		public bool isLight(){return false;}
	
		
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
		
		public bool setTexture (Bitmap text_)

		{
			texture = text_;
			return true;
		}
	}
}

