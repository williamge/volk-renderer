using System;
using System.Drawing;
using OpenTK;
namespace volkrenderer
{
	public class Quad : Primitive
	{
		
		Vector3d p1,p2,p3,p4, normalq;

		Vector3d ubasis, vbasis;
		
		Triangle[] triangles;

		double[] colour;

		double reflectd;
		double ambient;
		double diffuse;
		double specular;
		double transparency;
		
		double intensity;

		double[,,] texture;
		int tWidth, tHeight;
		
		bool islight;
		
		public Quad (Vector3d p1_, Vector3d p2_, Vector3d p3_, Vector3d p4_, Color col_)
		{
			p1 = p1_;
			p2 = p2_;
			p3 = p3_;
			p4 = p4_;
			
			colour = new double[3] { col_.R, col_.G, col_.B };
			
			ubasis = p2_ - p1_;
			vbasis = p4_ - p1_;
			
			normalq = Vector3d.Cross (ubasis, vbasis);
			normalq.Normalize ();
			
			triangles = new Triangle[2] {
				new Triangle (p1, p2, p3, col_), 
				new Triangle (p1, p3, p4, col_)
			};
			
			for (int n = 0; n < 2; n++) {
				triangles[n].setDiffuse (diffuse);
				triangles[n].setSpecular (specular);
				triangles[n].setAmbient (ambient);
				triangles[n].setReflect (reflectd);
				triangles[n].setTransparency (transparency);
			}
			
			
			
			ambient = 0.15;
			diffuse = 0.6;
			specular = 0.5;
			reflectd = 0.0;
			transparency = 0.0;
			
			islight = false;
			intensity = 0.0;
			
			texture = null;
			
		}
	

		#region Primitive implementation
		public double intersect (OpenTK.Vector3d d0, OpenTK.Vector3d d1)
		{
			double intersectq = triangles[0].intersect (d0, d1);
			if (intersectq > 0) 
			{
				return intersectq;
			}
			intersectq = triangles[1].intersect (d0, d1);
			if (intersectq > 0)
			{
				return intersectq;
			}
		
			return -1.0;
		}

		public OpenTK.Vector3d normal (OpenTK.Vector3d point)
		{
			return normalq;
		}
		
		
		//set

		
		public bool setColour (Color c_)
		{
			colour[0] = c_.R;
			colour[1] = c_.G;
			colour[2] = c_.B;
			for (int n = 0; n < 2; n++) {
				triangles[n].setColour (c_);
			}
			return true;
		}
		
		public bool setAmbient (double a_)
		{
			ambient = a_;
			for (int n = 0; n < 2; n++) {
				triangles[n].setAmbient (ambient);
			}
			return true;
		}

		public bool setDiffuse (double d_)
		{
			diffuse = d_;
			for (int n = 0; n < 2; n++) {
				triangles[n].setDiffuse (diffuse);
			}
			return true;
		}

		public bool setSpecular (double s_)
		{
			specular = s_;
			for (int n = 0; n < 2; n++) {
				triangles[n].setSpecular (specular);
			}
			return true;
		}

		public bool setReflect (double r_)
		{
			reflectd = r_;
			for (int n = 0; n < 2; n++) {
				triangles[n].setReflect (reflectd);
			}
			return true;
		}

		public bool setTransparency (double t_)
		{
			transparency = t_;
			for (int n = 0; n < 2; n++) {
				triangles[n].setTransparency (transparency);
			}
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
		
			return true;
		}
		
		public void setLight (double i_)
		{
			islight = true;
			intensity = i_;
		}
		
		//get
		
		public double[] getColour (OpenTK.Vector3d p)
		{
			if (texture != null) 
			{
				//do something
			}
			
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
			return islight;
		}
		
		public RPatch[] RSplit (int gridSize)
		{
			int xgrids = (int)Math.Ceiling(ubasis.Length/gridSize);
			int ygrids = (int)Math.Ceiling(vbasis.Length/gridSize);
			
			RPatch[] rp = new RQuad[xgrids*ygrids];
			
			/*
			 * p1 + ubasis * (x * 1.0/xgrids) + vbasis * (y * 1.0/ygrids)
			 * ->
			 * p1 + ubasis * ((x+1) * 1.0/xgrids) + vbasis * ((y+1)* 1.0/ygrids)
			 * 
			 */
			
			int n=0;
			for (int x=0;x<xgrids;x++){
				for (int y=0;y<ygrids;y++){
					rp[n] = new RQuad(
						p1 + ubasis * (x * 1.0/xgrids) + vbasis * (y * 1.0/ygrids),
						p1 + ubasis * ((x+1) * 1.0/xgrids) + vbasis * (y * 1.0/ygrids),
						p1 + ubasis * ((x+1) * 1.0/xgrids) + vbasis * ((y+1) * 1.0/ygrids), 
						p1 + ubasis * (x * 1.0/xgrids) + vbasis * ((y+1) * 1.0/ygrids),
						colour,
						diffuse);
					if (islight){
						rp[n].setEmission(intensity);
					}
					n++;
				}
			}
			
			return rp;
		}
		#endregion
	}
}

