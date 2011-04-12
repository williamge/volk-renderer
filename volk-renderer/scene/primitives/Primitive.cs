using System;
using System.Drawing;
using OpenTK;
namespace volkrenderer
{
	public interface Primitive
	{
		double intersect(Vector3d d0, Vector3d d1);
		
		Color getColour();
		
		//getDiffuse();
		//getSpecular();
	}
}

