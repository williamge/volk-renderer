using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
namespace volkrenderer
{
	public class vScene
	{
		public int ImageWidth, ImageHeight;
		
		List<Primitive> prims = new List<Primitive> ();
		List<Light> lights = new List<Light>(); 
		public Vector3d origin;
		public Vector3d target;
		public double exposure;
		
		public vScene (int width_, int height_)
		{
			ImageWidth = width_;
			ImageHeight = height_;
			
			origin = new Vector3d (0, 0, -480);
			target = new Vector3d (0, 0, 1);
			
			/* TODO
			 * make this user-settable */
			exposure = -2.0f;
		}
		
		public bool addPrim (Primitive p_)
		
		{
			prims.Add (p_);
			return true;
		}
		
		public bool addSphere (Vector3d center, int radius, Color colour)
		{
			if (radius < 0) 
			{
				return false;
			}
			prims.Add (new Sphere (colour, center, radius));
			return true;
		}
		
		public bool addSphere (Vector3d center, int radius, Color colour,double reflectd_)
		{
			if (radius < 0) {
				return false;
			}
			prims.Add (new Sphere (colour, center, radius,reflectd_));
			return true;
		}
		
		public bool addPlane (Vector3d point, Vector3d normal, Color colour)

		{
			prims.Add (new Plane (point, normal, colour));
			return true;
		}
		
		public bool addPointLight (Vector3d p_, Color col_)

		{
			lights.Add (new PointLight (p_, col_,this));
			return true;
		}
		
		public bool addPointLight (Vector3d p_, Color col_, double t_)
		{
			lights.Add (new PointLight (p_, col_, t_, this));
			return true;
		}
		
		public bool addAreaLight (Vector3d p1_, Vector3d p2_, Vector3d p3_, Vector3d p4_, Color col_, double t_)
		{
			double lightnum = 7.0;
			Random monteoffset = new Random();
			
			for (int i = 0;i<=lightnum;i++){
				for (int j = 0;j<=lightnum;j++){
					/* TODO
					 * change or fix or do something so this works on more than just the xz-axis plane */
					lights.Add(new PointLight(p1_ + new Vector3d(((p2_.X-p1_.X)/lightnum * monteoffset.NextDouble() ) + i * (p2_.X-p1_.X)/lightnum,
													0,
													((p4_.Z-p1_.Z)/lightnum * monteoffset.NextDouble() ) + j * (p4_.Z-p1_.Z)/lightnum),
							col_, t_ / Math.Pow(lightnum + 1, 2), this,true));
				}
			}
			Quad aq = new Quad(p1_,p2_,p3_,p4_,col_);
			aq.setAmbient(1.0);
			aq.setDiffuse(0.0);
			aq.setSpecular(0.0);
			aq.setLight();
			prims.Add(aq);
			return true;
		}
		
		public List<Primitive> getPrims ()
		{
			return prims;
		}
		
		public List<Light> getLights ()
		
		{
			return lights;
		}
		
		public double[] getBack (Vector3d origin, Vector3d direction)
		{
			//return Color.Black;
			return new double[] { 0.0, 0.0, 0.0 };
		}
		
	}
}

