using System;
using OpenTK;
using System.Drawing;
namespace volkrenderer
{
	public class Plane : Primitive
	{
		Vector3d point,basis;
		Color colour;
		
		public Plane (Vector3d point_, Vector3d basis_, Color colour_)
		{
			point = point_;
			basis = basis_;
			colour = colour_;
		}
		
		public double intersect (Vector3d d0, Vector3d d1)
		{
		}
		
		public Color getColour ()
		{
			return colour;
		}
	}
}

