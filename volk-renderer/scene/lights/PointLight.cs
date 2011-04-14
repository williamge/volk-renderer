using System;
using System.Drawing;
using OpenTK;
using System.Collections.Generic;
namespace volkrenderer
{
	public class PointLight : Light
	{
		List<Vector3d> points;
		Vector3d point;
		Color colour;
		double intensity;
		LightSphere viewSphere;
		
		public PointLight (Vector3d p, Color col_, vScene scene)
		{
			points = new List<Vector3d> ();
			points.Add (p);
			point = p;
			colour = col_;
			intensity = 1.0;
			viewSphere = new LightSphere (col_, p, (int)(5.0 * intensity));
			scene.addPrim (viewSphere);
		}
		
		public PointLight (Vector3d p, Color col_, double intensity_, vScene scene)
		{
			points = new List<Vector3d> ();
			points.Add (p);
			point = p;
			colour = col_;
			intensity = intensity_;
			viewSphere = new LightSphere (col_, p, (int)(5.0 * intensity));
			scene.addPrim (viewSphere);
		}
		
		public List<Vector3d> getPoints ()
		{
			return points;
		}
		public Vector3d getPoint ()
		{
			return point;
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

