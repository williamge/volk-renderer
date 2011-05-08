using System;
using OpenTK;
namespace volkrenderer
{
	public class RTriangle : RPatch
	{
		
		private Vector3d p1, p2, p3;
		private Vector3d normalt, norm1, norm2;
		private Vector3d ubasis, vbasis;
		
		Vector3d center;
		
		private double td1, td2;
		
		double[] colour;
		
		double reflectance;
		double emission;
		
		double[] incident, excident;
		
		public RTriangle (Vector3d p1_,Vector3d p2_, Vector3d p3_, double[] col_, double reflectance_)
		{			
			p1 = p1_;
			p2 = p2_;
			p3 = p3_;
			
			ubasis = p2 - p1;
			vbasis = p3 - p1;
			
			normalt = Vector3d.Cross (ubasis, vbasis);
			
			norm1 = Vector3d.Cross (vbasis, normalt) / Math.Pow (normalt.Length, 2);
			norm2 = Vector3d.Cross (normalt, ubasis) / Math.Pow (normalt.Length, 2);
			
			td1 = Vector3d.Dot (-norm1, p1);
			td2 = Vector3d.Dot (-norm2, p1);
			
			normalt.Normalize ();
			
			if (normalt == Vector3d.Zero) {
				throw new NotSupportedException ();
			}
			
			center = p1 + (p3 - p1)/2;
			center = center + (p2 - center)/2;
			
			colour = col_;
			
			reflectance = reflectance_;
			emission = 0.0;
			
			excident = new double[3] { emission * colour[0], emission * colour[1], emission * colour[2] };
			
		}
	

		#region RPatch implementation
		public double getEmission ()
		{
			return emission;
		}
		
		public void setEmission (double e_)

		{
			emission = e_;
		}

		public double getReflectance ()
		{
			return reflectance;
		}

		public double[] getIncident ()
		{
			return incident;
		}

		public double[] getExcident ()
		{
			return excident;
		}

		public void setIncident (double[] i)
		{
			incident = i;
		}

		public void setExcident (double[] e)
		{
			excident = e;
		}

		public double[] getColour ()
		{
			return colour;
		}

		public Vector3d getCenter ()
		{
			return center;
		}

		public Vector3d getNormal ()
		{
			return normalt;
		}

		public double intersect (Vector3d d0, OpenTK.Vector3d d1)
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

		public Vector3d normal (Vector3d p)
		{
			return normalt;
		}
		#endregion
}
}

