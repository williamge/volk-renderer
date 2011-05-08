using System;
using System.Drawing;
using OpenTK;
namespace volkrenderer
{
	public class Sphere : Primitive
	{
		double[] colour;
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
		double refract;
		
		double[,,] texture;
		int tWidth, tHeight;
		
		public Sphere (Color colour0, Vector3d center0, int radius0)
		{
			colour = new double[3];
			colour[0] = colour0.R;
			colour[1] = colour0.G;
			colour[2] = colour0.B;
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
			refract = 1.0;
			
			texture = null;
		}
		
		public Sphere (Color colour0, Vector3d center0, int radius0, double reflectd_)
		{
			colour = new double[3];
			colour[0] = colour0.R;
			colour[1] = colour0.G;
			colour[2] = colour0.B;
			center = center0;
			radius = radius0;
			
			reflectd = reflectd_;
			diffuse = 1.0 / 3.0;
			specular = 1.0 / 3.0;
			transparency = 0.0;
			ambient = 1.0 / 3.0;
			refract = 1.0;
			
			texture = null;
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
			t = Math.Min (Math.Max( t + t2,0), Math.Max( t - t2,0));
			
			return t;
		
			
		}
		
		//probably should check if 'point' is actually on the sphere but who cares at this point.
		public Vector3d normal (Vector3d point)
		{
			Vector3d N = point - center;
			if (N.Length < radius - 0.1) 
			{
				N = -N;
			}
			N.Normalize ();
			return N;
		}
		
		
		//get
		
		public double[] getColour (Vector3d p)
		{
			
			/* TODO: some kind of interpolation to clean up textures */
			if (texture != null) 
			{
				Vector3d vp = (p - center) * 1.0/radius;
				double r = Math.Acos (Vector3d.Dot (-vp, vnorth));
				double v = (r/Math.PI) * tWidth;
				double u;
				
				double theta = Math.Acos(Vector3d.Dot(new Vector3d(1,0,0),vp / Math.Sin(r))) / (2.0 * Math.PI);
				
				if (Vector3d.Dot (vc, vp) <0)
					u = (1.0f - theta) * tHeight;
				else
					u = theta * tHeight;
				
				int x_ = Math.Min( (int)Math.Floor(u),tWidth - 1);
				int y_ = Math.Min( (int)Math.Floor (v), tHeight - 1);
				double[] tcol = new double[3];
				tcol[0] = texture[x_,y_,0];
				tcol[1] = texture[x_,y_,1];
				tcol[2] = texture[x_,y_,2];
				return tcol;
					//texture.GetPixel(Math.Min( (int)Math.Floor(u),tWidth - 1 ),
										//Math.Min( (int)Math.Floor (v), tHeight - 1));
				
				
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
			return refract;
		}
		
		public bool isLight(){return false;}
	
		
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
		
		public bool setAmbient (double a_)
		
		{
			ambient = a_;
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
		
		public bool setRefract (double r_)
		{
			refract = r_;
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
		
		public RPatch[] RSplit (int gridSize)
		{
			throw new NotImplementedException ();
		}
	}
}

