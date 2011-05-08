using System;
using OpenTK;
namespace volkrenderer
{
	public class RQuad : RPatch
	{
		
		Vector3d p1,p2,p3,p4;
		
		Vector3d v_normal;
		Vector3d ubasis, vbasis;
		
		Vector3d center;
		
		RTriangle[] triangles;
		
		double[] colour;
		double reflectance;
		double emission;
		
		double[] incident, excident;
		
		public RQuad (Vector3d p1_, Vector3d p2_, Vector3d p3_, Vector3d p4_, double[] col_, double reflectance_)
		{
			p1 = p1_;
			p2 = p2_;
			p3 = p3_;
			p4 = p4_;
			
			ubasis = p2_ - p1_;
			vbasis = p4_ - p1_;
			
			center = p1 + (p3 - p1)/2;
			
			v_normal = Vector3d.Cross (ubasis, vbasis);
			v_normal.Normalize ();
			
			colour = col_;
			
			triangles = new RTriangle[2] { new RTriangle (p1, p2, p3, col_,0), new RTriangle (p1, p3, p4, col_,0) };
			
			reflectance = reflectance_; 
			emission = 0.0;
			
			excident = new double[3] { emission * colour[0], emission * colour[1], emission * colour[2] };
			
		}
	

		#region RPatch implementation
		double RPatch.getEmission ()
		{
			return emission;
		}
		
		public void setEmission (double e_)
		{
			emission = e_;
		}

		double RPatch.getReflectance ()
		{
			return reflectance;
		}

		double[] RPatch.getIncident ()
		{
			return incident;
		}

		double[] RPatch.getExcident ()
		{
			return excident;
		}

		void RPatch.setIncident (double[] i)
		{
			incident = i;
		}

		void RPatch.setExcident (double[] e)
		{
			excident = e;
		}

		double[] RPatch.getColour ()
		{
			return colour;
		}

		Vector3d RPatch.getCenter ()
		{
			return center;
		}

		Vector3d RPatch.getNormal ()
		{
			return v_normal;
		}

		double RPatch.intersect (Vector3d o, Vector3d d)
		{
			double intersectq = triangles[0].intersect (o, d);
			if (intersectq > 0) {
				return intersectq;
			}
			intersectq = triangles[1].intersect (o, d);
			if (intersectq > 0) {
				return intersectq;
			}
			
			return -1.0;
		}

		Vector3d RPatch.normal (Vector3d p)
		{
			return v_normal;
		}
		#endregion
}
}

