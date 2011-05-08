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
		public double fov;
		public double lightnums;
		public int depth;
		double[] bgcolor;
		
		public vScene (int width_, int height_)
		{
			ImageWidth = width_;
			ImageHeight = height_;
			
			bgcolor = new double[3]{0.0,0.0,0.0};
			
			lightnums = 3.0;
			depth = 4;
			fov = 0.3;
			
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
			double lightnum = this.lightnums;
			Random monteoffset = new Random();
			
			for (int i = 0;i<=lightnum;i++){
				for (int j = 0;j<=lightnum;j++){						
					PointLight pl = new PointLight(p1_ 
							//u_vector offset
							+ ((p2_ - p1_) * ((i * 1.0/lightnum) + 1.0/lightnum * monteoffset.NextDouble()))
							//v_vector offset
							+ ((p4_ - p1_)* ((j * 1.0/lightnum) + 1.0/lightnum * monteoffset.NextDouble()))
							, col_, t_/Math.Pow(lightnum+1,2),this);
					pl.setAreaLight();
					lights.Add(pl);
					
				}
			}
			Quad aq = new Quad(p1_,p2_,p3_,p4_,col_);
			aq.setAmbient(1.0);
			aq.setDiffuse(0.0);
			aq.setSpecular(0.0);
			aq.setLight(t_);
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
		
		/// <summary>
		/// Sets the background colour for the scene.
		/// </summary>
		/// <param name="col_">
		/// Background colour for the scene to be set to <see cref="Color"/>
		/// </param>
		/// <returns>
		/// true on success, false otherwise <see cref="System.Boolean"/>
		/// </returns>
		public bool setBack (Color col_)

		{
			bgcolor = new double[3] { col_.R, col_.G, col_.B };
			return true;
		}
		
		/// <summary>
		/// Returns the background colour for a fired ray.
		/// </summary>
		/// <param name="origin">
		/// Origin point of the ray <see cref="Vector3d"/>
		/// </param>
		/// <param name="direction">
		/// Direction vector of the ray <see cref="Vector3d"/>
		/// </param>
		/// <returns>
		/// Background colour for the ray as a double[3].
		/// </returns>
		public double[] getBack (Vector3d origin, Vector3d direction)
		{
			return bgcolor;
		}
		
	}
}

