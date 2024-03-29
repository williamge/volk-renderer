using System;
using System.Drawing;
using OpenTK;
namespace volkrenderer
{
	public interface Primitive
	{
		
		double intersect(Vector3d d0, Vector3d d1);
		Vector3d normal (Vector3d point);
		
		double[] getColour(Vector3d p);
		
		double getDiffuse();
		double getSpecular();
		double getTransparency ();		
		double getReflect();
		double getAmbient ();
		double getRefract();
		
		bool isLight();
		
		RPatch[] RSplit(int gridSize);
	}
}

