using System;
using OpenTK;
using System.Drawing;

namespace volkrenderer
{
	public class Triangle : Primitive
	{
		public Triangle ()
		{
		}
	
		#region Primitive implementation
		public double intersect (Vector3d d0, Vector3d d1)
		{
			throw new NotImplementedException ();
		}

		public Vector3d normal (Vector3d point)
		{
			throw new NotImplementedException ();
		}

		public Color getColour (Vector3d p)
		{
			throw new NotImplementedException ();
		}

		public double getDiffuse ()
		{
			throw new NotImplementedException ();
		}

		public double getSpecular ()
		{
			throw new NotImplementedException ();
		}

		public double getTransparency ()
		{
			throw new NotImplementedException ();
		}

		public double getReflect ()
		{
			throw new NotImplementedException ();
		}

		public double getAmbient ()
		{
			throw new NotImplementedException ();
		}

		public bool isLight ()
		{
			throw new NotImplementedException ();
		}
		#endregion

}
}

