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
		
		public PointLight (Vector3d p, Color col_)
		{
			points = new List<Vector3d> ();
			points.Add (p);
			colour = col_;
		}
		
		public List<Vector3d> getPoints ()
		{
			return points;
		}
		
		public Color getColour ()
		{
			return colour;
		}
	}
}

