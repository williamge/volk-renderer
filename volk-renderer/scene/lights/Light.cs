using System;
using System.Drawing;
using OpenTK;
using System.Collections.Generic;
namespace volkrenderer
{
	public interface Light
	{
		
		double[] getColour();
		
		List<Vector3d> getPoints();
		Vector3d getPoint();
		
		double getIntensity();
		
		

	}
}

