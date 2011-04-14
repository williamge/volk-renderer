using System;
using System.Drawing;
using OpenTK;
using System.Collections.Generic;
namespace volkrenderer
{
	public class PointLight : Light
	{
		List<Vector3d> points;
		Color colour;
		double intensity;
		
		public PointLight (Vector3d p, Color col_)
		{
			points = new List<Vector3d> ();
			points.Add (p);
			colour = col_;
			intensity = 1.0;
		}
		
		public PointLight (Vector3d p, Color col_, double intensity_)
		{
			points = new List<Vector3d> ();
			points.Add (p);
			colour = col_;
			intensity = intensity_;
		}
		
		public List<Vector3d> getPoints ()
		{
			return points;
		}
		
		public Color getColour ()
		{
			return colour;
		}
		
		public double getIntensity ()
		{
			return intensity;	
		}
	}
}

