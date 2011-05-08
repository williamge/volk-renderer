using System;
using OpenTK;
namespace volkrenderer
{
	public interface RPatch
	{
		double getEmission();
		void setEmission (double e_);
		double getReflectance();
		double[] getIncident();
		double[] getExcident();
		void setIncident(double[] i);
		void setExcident(double[] e);
		
		double[] getColour();
		
		Vector3d getCenter();
		Vector3d getNormal();
		
		double intersect(Vector3d o, Vector3d d);
		Vector3d normal(Vector3d p);
		
	}
}

