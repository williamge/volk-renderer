using System;
using System.Collections.Generic;
using OpenTK;
using System.Drawing;
namespace volkrenderer
{
	public class radiosity
	{
		//List of all patches in the scene
		List<RPatch> patchList;
		//Queue of all patches that still need to be operated on
		Queue<RPatch> patchQueue;
		
		int passNumber;
		
		public radiosity (vScene vs)
		{
			patchList = new List<RPatch> ();
			patchQueue = new Queue<RPatch> ();
			
			passNumber = 0;
			
			const int gridSize = 10;
			foreach (Primitive pr in vs.getPrims ()) 
			{
				patchList.AddRange (pr.RSplit (gridSize));
			}
		}
		
		private void update ()
		{
			//update everything in queue
			foreach (RPatch rp in patchQueue) {
				double[,,] patchImage = Rrender (rp.getCenter (), rp.getNormal (), 64, 64, 2);
				double[] pIn = averageImage (patchImage, 64, 64);
				rp.setIncident (pIn);
			}
			
			//calculate lighting of scene
			double[] newIn = new double[3] { 0.0, 0.0, 0.0 };
			foreach (RPatch rp in patchQueue)
			{
				
				newIn[0] = newIn[1] = newIn[2] = 0.0;
				
				newIn[0] = rp.getReflectance () * (rp.getColour ()[0] / 255.0) * rp.getIncident ()[0];
				newIn[1] = rp.getReflectance () * (rp.getColour ()[1] / 255.0) * rp.getIncident ()[1];
				newIn[2] = rp.getReflectance () * (rp.getColour ()[2] / 255.0) * rp.getIncident ()[2];
				
				newIn[0] += rp.getEmission () * rp.getColour()[0];
				newIn[1] += rp.getEmission () * rp.getColour()[1];
				newIn[2] += rp.getEmission () * rp.getColour()[2];
				
				rp.setExcident (newIn);
			}
		
		}
		
		public void RPass ()
		{
			passNumber++;
			
			patchQueue.Clear ();
			
			foreach (RPatch rp in patchList) 
			{
				patchQueue.Enqueue (rp);
			}
			update ();
		}
		
		private double[] averageImage (double[,,] im, int dimx, int dimy)
		{
			double[] average = new double[3] { 0.0, 0.0, 0.0 };
			
			for (int n = 0; n < 3; n++) {
				for (int x = 0; x < dimx; x++) {
					for (int y = 0; y < dimy; y++) {
						average[n] += im[x, y, n];
					}
				}
				average[n] = average[n] / dimx*dimy;
			}
			
			return average;
		}
		
		private double[,,] Rrender (Vector3d origin, Vector3d target, int dimx, int dimy, byte patch)
		{
			double[,,] image = new double[dimx, dimy, 3];
			
			double[] pcol = new double[3]{0.0,0.0,0.0};
			
			/* TODO: start using 'patch' to decide which part of hemicube to render */
			
			double fovx = 1.0 - 0.3;
			double fovy = fovx;
			
			fovx = Math.Tan (fovx);
			fovy = Math.Tan (fovy);
			
			Vector3d camz = target - origin;
			camz.Normalize ();
			
			Vector3d up = new Vector3d (0, 1, 0);
			
			Vector3d camx = Vector3d.Cross (up, camz);
			//if (up = scalar times camz) then we should choose a different up 
			camx = camx != Vector3d.Zero ? camx : Vector3d.Cross (new Vector3d (0, 0, 1), camz);
			
			Vector3d camy = Vector3d.Cross (camx, -camz);
			
			Vector3d camzz = camz * dimx;
			

				for (int x = 0; x < dimx; x++)
				{
					for (int y = 0; y < dimy; y++)
					{
						Vector3d dir = ((fovx * camx * (x - dimx / 2)) + (fovy * camy * -(y - dimy / 2)) + camzz);
						dir.Normalize ();
					
						pcol = Rtrace(origin,dir,patchList);
						image[x,y,0] = pcol[0];
						image[x,y,1] = pcol[1];
						image[x,y,2] = pcol[2];
					}
				}
				
			
			return image;
		}
		
		private double[] Rtrace (Vector3d origin, Vector3d direction, List<RPatch> pList)
		{
			
			double[] pcol = new double[3] { 0.0, 0.0, 0.0 };
			
			//closest t so far
			double ct = 0.0;
			RPatch cobject = null;
			
			//loop through all objects in scene.
			foreach (RPatch pr in pList) {
				double iresult = pr.intersect (origin, direction);
				
				if (iresult > 0.0000001) {
					if (ct == 0.0 || iresult < ct) {
						ct = iresult;
						cobject = pr;
					}
				}
			}
			
			if (ct > 0.0) {
				pcol[0] = cobject.getExcident ()[0];
				pcol[1] = cobject.getExcident ()[1];
				pcol[2] = cobject.getExcident ()[2];
			}
			
			
			return pcol;
		}
		
		public bool Rimage (int x, int y, double fov, vScene scene)
		{
			
			double[,,] dimage = Rrender (scene.origin, scene.target, x, y, 2);
			dimage = colourCorrection (dimage, x, y, scene);
			imageSave ("my path here", x, y, dimage);
			
			return true;
			
		}
		
		private double[,,] colourCorrection (double[,,] dimage, int dimx, int dimy, vScene scene)
		{
			
			//exposures, may not be useful idk
			double exposure = scene.exposure;
			
			for (int x = 0; x < dimx; x++) {
				for (int y = 0; y < dimy; y++) {
					
					dimage[x, y, 0] = 255.0 * (1.0 - Math.Exp (dimage[x, y, 0] / 255.0 * exposure));
					dimage[x, y, 1] = 255.0 * (1.0 - Math.Exp (dimage[x, y, 1] / 255.0 * exposure));
					dimage[x, y, 2] = 255.0 * (1.0 - Math.Exp (dimage[x, y, 2] / 255.0 * exposure));
					
					//gamma correction, may be wrong or unnecessary
					dimage[x, y, 0] = dimage[x, y, 0] * Math.Pow (dimage[x, y, 0] / 255.0, 1.0 / 2.2);
					dimage[x, y, 1] = dimage[x, y, 1] * Math.Pow (dimage[x, y, 1] / 255.0, 1.0 / 2.2);
					dimage[x, y, 2] = dimage[x, y, 2] * Math.Pow (dimage[x, y, 2] / 255.0, 1.0 / 2.2);
					
					dimage[x, y, 0] = Math.Max (Math.Min (255, dimage[x, y, 0]), 0);
					dimage[x, y, 1] = Math.Max (Math.Min (255, dimage[x, y, 1]), 0);
					dimage[x, y, 2] = Math.Max (Math.Min (255, dimage[x, y, 2]), 0);
				}
			}
			return dimage;
		}
		
		public bool imageSave (string path, int dimx, int dimy, double[,,] dimage)
		{
			if (dimage == null) {
				return false;
			}
			
			Bitmap im = new Bitmap (dimx, dimy);
			
			for (int i = 0; i < dimx; i++) {
				for (int j = 0; j < dimy; j++) {
					//Convert the values from the array of doubles to a Color value
					//and set the pixel to that Color
					Color pixcol = Color.FromArgb (Math.Max (Math.Min (255, (int)dimage[i, j, 0]), 0), Math.Max (Math.Min (255, (int)dimage[i, j, 1]), 0), Math.Max (Math.Min (255, (int)dimage[i, j, 2]), 0));
					im.SetPixel (i, j, pixcol);
					
				}
			}
			
			im.Save (path);
			
			/* HACK */
			//im.Save ("/Users/william/Dropbox/Public/test.jpg");

			return true;
		}
		

			
		
		
	}
}

