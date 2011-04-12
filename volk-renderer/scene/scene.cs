using System;
using System.Drawing;
using System.Collections.Generic;
using OpenTK;
namespace volkrenderer
{
	public class vScene
	{
		public int ImageWidth, ImageHeight;
		
		public List<Primitive> prims = new List<Primitive> ();
		
		public vScene (int width_, int height_)
		{
			ImageWidth = width_;
			ImageHeight = height_;
		}
		
		public bool addSphere (Color colour, Vector3d center, int radius)
		{
			if (radius < 0) 
			{
				return false;
			}
			prims.Add (new Sphere (colour, center, radius));
			return true;
		}
		
	}
}

