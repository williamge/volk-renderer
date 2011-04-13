using System;
using System.Drawing;
using OpenTK;
using System.Collections.Generic;
namespace volkrenderer
{
	public interface Light
	{
		
		Color getColour(Vector3d p);
		
		List<Vector3d> getPoints();
		
		

	}
}

