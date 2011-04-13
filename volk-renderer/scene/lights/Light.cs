using System;
using System.Drawing;
using OpenTK;
using System.Collections.Generic;
namespace volkrenderer
{
	public interface Light
	{
		
		Color getColour();
		
		List<Vector3d> getPoints();
		
		

	}
}

