using System;
using System.Drawing;
using OpenTK;
namespace volkrenderer
{
	public class Teapot : Primitive
	{
		Vector3d[] vertices;
		Quad[] faces;
		
		double[] colour;

		double[,,] texture;
		int tWidth, tHeight;
		
		Vector3d lastnormal;

		double diffuse, specular, transparency, reflect, ambient;
		
		public Teapot ()
		{
			vertices = new Vector3d[118];
			
			vertices[0] = new Vector3d (0.2000, 0.0000, 2.70000);
			vertices[1] = new Vector3d (0.2000, -0.1120, 2.70000);
			vertices[2] = new Vector3d (0.1120, -0.2000, 2.70000);
			vertices[3] = new Vector3d (0.0000, -0.2000, 2.70000);
			vertices[4] = new Vector3d (1.3375, 0.0000, 2.53125);
			vertices[5] = new Vector3d (1.3375, -0.7490, 2.53125);
			vertices[6] = new Vector3d (0.7490, -1.3375, 2.53125);
			vertices[7] = new Vector3d (0.0000, -1.3375, 2.53125);
			vertices[8] = new Vector3d (1.4375, 0.0000, 2.53125);
			vertices[9] = new Vector3d (1.4375, -0.8050, 2.53125);
			vertices[10] = new Vector3d (0.8050, -1.4375, 2.53125);
			vertices[11] = new Vector3d (0.0000, -1.4375, 2.53125);
			vertices[12] = new Vector3d (1.5000, 0.0000, 2.40000);
			vertices[13] = new Vector3d (1.5000, -0.8400, 2.40000);
			vertices[14] = new Vector3d (0.8400, -1.5000, 2.40000);
			vertices[15] = new Vector3d (0.0000, -1.5000, 2.40000);
			vertices[16] = new Vector3d (1.7500, 0.0000, 1.87500);
			vertices[17] = new Vector3d (1.7500, -0.9800, 1.87500);
			vertices[18] = new Vector3d (0.9800, -1.7500, 1.87500);
			vertices[19] = new Vector3d (0.0000, -1.7500, 1.87500);
			vertices[20] = new Vector3d (2.0000, 0.0000, 1.35000);
			vertices[21] = new Vector3d (2.0000, -1.1200, 1.35000);
			vertices[22] = new Vector3d (1.1200, -2.0000, 1.35000);
			vertices[23] = new Vector3d (0.0000, -2.0000, 1.35000);
			vertices[24] = new Vector3d (2.0000, 0.0000, 0.90000);
			vertices[25] = new Vector3d (2.0000, -1.1200, 0.90000);
			vertices[26] = new Vector3d (1.1200, -2.0000, 0.90000);
			vertices[27] = new Vector3d (0.0000, -2.0000, 0.90000);
			vertices[28] = new Vector3d (-2.0000, 0.0000, 0.90000);
			vertices[29] = new Vector3d (2.0000, 0.0000, 0.45000);
			vertices[30] = new Vector3d (2.0000, -1.1200, 0.45000);
			vertices[31] = new Vector3d (1.1200, -2.0000, 0.45000);
			vertices[32] = new Vector3d (0.0000, -2.0000, 0.45000);
			vertices[33] = new Vector3d (1.5000, 0.0000, 0.22500);
			vertices[34] = new Vector3d (1.5000, -0.8400, 0.22500);
			vertices[35] = new Vector3d (0.8400, -1.5000, 0.22500);
			vertices[36] = new Vector3d (0.0000, -1.5000, 0.22500);
			vertices[37] = new Vector3d (1.5000, 0.0000, 0.15000);
			vertices[38] = new Vector3d (1.5000, -0.8400, 0.15000);
			vertices[39] = new Vector3d (0.8400, -1.5000, 0.15000);
			vertices[40] = new Vector3d (0.0000, -1.5000, 0.15000);
			vertices[41] = new Vector3d (-1.6000, 0.0000, 2.02500);
			vertices[42] = new Vector3d (-1.6000, -0.3000, 2.02500);
			vertices[43] = new Vector3d (-1.5000, -0.3000, 2.25000);
			vertices[44] = new Vector3d (-1.5000, 0.0000, 2.25000);
			vertices[45] = new Vector3d (-2.3000, 0.0000, 2.02500);
			vertices[46] = new Vector3d (-2.3000, -0.3000, 2.02500);
			vertices[47] = new Vector3d (-2.5000, -0.3000, 2.25000);
			vertices[48] = new Vector3d (-2.5000, 0.0000, 2.25000);
			vertices[49] = new Vector3d (-2.7000, 0.0000, 2.02500);
			vertices[50] = new Vector3d (-2.7000, -0.3000, 2.02500);
			vertices[51] = new Vector3d (-3.0000, -0.3000, 2.25000);
			vertices[52] = new Vector3d (-3.0000, 0.0000, 2.25000);
			vertices[53] = new Vector3d (-2.7000, 0.0000, 1.80000);
			vertices[54] = new Vector3d (-2.7000, -0.3000, 1.80000);
			vertices[55] = new Vector3d (-3.0000, -0.3000, 1.80000);
			vertices[56] = new Vector3d (-3.0000, 0.0000, 1.80000);
			vertices[57] = new Vector3d (-2.7000, 0.0000, 1.57500);
			vertices[58] = new Vector3d (-2.7000, -0.3000, 1.57500);
			vertices[59] = new Vector3d (-3.0000, -0.3000, 1.35000);
			vertices[60] = new Vector3d (-3.0000, 0.0000, 1.35000);
			vertices[61] = new Vector3d (-2.5000, 0.0000, 1.12500);
			vertices[62] = new Vector3d (-2.5000, -0.3000, 1.12500);
			vertices[63] = new Vector3d (-2.6500, -0.3000, 0.93750);
			vertices[64] = new Vector3d (-2.6500, 0.0000, 0.93750);
			vertices[65] = new Vector3d (-2.0000, -0.3000, 0.90000);
			vertices[66] = new Vector3d (-1.9000, -0.3000, 0.60000);
			vertices[67] = new Vector3d (-1.9000, 0.0000, 0.60000);
			vertices[68] = new Vector3d (1.7000, 0.0000, 1.42500);
			vertices[69] = new Vector3d (1.7000, -0.6600, 1.42500);
			vertices[70] = new Vector3d (1.7000, -0.6600, 0.60000);
			vertices[71] = new Vector3d (1.7000, 0.0000, 0.60000);
			vertices[72] = new Vector3d (2.6000, 0.0000, 1.42500);
			vertices[73] = new Vector3d (2.6000, -0.6600, 1.42500);
			vertices[74] = new Vector3d (3.1000, -0.6600, 0.82500);
			vertices[75] = new Vector3d (3.1000, 0.0000, 0.82500);
			vertices[76] = new Vector3d (2.3000, 0.0000, 2.10000);
			vertices[77] = new Vector3d (2.3000, -0.2500, 2.10000);
			vertices[78] = new Vector3d (2.4000, -0.2500, 2.02500);
			vertices[79] = new Vector3d (2.4000, 0.0000, 2.02500);
			vertices[80] = new Vector3d (2.7000, 0.0000, 2.40000);
			vertices[81] = new Vector3d (2.7000, -0.2500, 2.40000);
			vertices[82] = new Vector3d (3.3000, -0.2500, 2.40000);
			vertices[83] = new Vector3d (3.3000, 0.0000, 2.40000);
			vertices[84] = new Vector3d (2.8000, 0.0000, 2.47500);
			vertices[85] = new Vector3d (2.8000, -0.2500, 2.47500);
			vertices[86] = new Vector3d (3.5250, -0.2500, 2.49375);
			vertices[87] = new Vector3d (3.5250, 0.0000, 2.49375);
			vertices[88] = new Vector3d (2.9000, 0.0000, 2.47500);
			vertices[89] = new Vector3d (2.9000, -0.1500, 2.47500);
			vertices[90] = new Vector3d (3.4500, -0.1500, 2.51250);
			vertices[91] = new Vector3d (3.4500, 0.0000, 2.51250);
			vertices[92] = new Vector3d (2.8000, 0.0000, 2.40000);
			vertices[93] = new Vector3d (2.8000, -0.1500, 2.40000);
			vertices[94] = new Vector3d (3.2000, -0.1500, 2.40000);
			vertices[95] = new Vector3d (3.2000, 0.0000, 2.40000);
			vertices[96] = new Vector3d (0.0000, 0.0000, 3.15000);
			vertices[97] = new Vector3d (0.8000, 0.0000, 3.15000);
			vertices[98] = new Vector3d (0.8000, -0.4500, 3.15000);
			vertices[99] = new Vector3d (0.4500, -0.8000, 3.15000);
			vertices[100] = new Vector3d (0.0000, -0.8000, 3.15000);
			vertices[101] = new Vector3d (0.0000, 0.0000, 2.85000);
			vertices[102] = new Vector3d (1.4000, 0.0000, 2.40000);
			vertices[103] = new Vector3d (1.4000, -0.7840, 2.40000);
			vertices[104] = new Vector3d (0.7840, -1.4000, 2.40000);
			vertices[105] = new Vector3d (0.0000, -1.4000, 2.40000);
			vertices[106] = new Vector3d (0.4000, 0.0000, 2.55000);
			vertices[107] = new Vector3d (0.4000, -0.2240, 2.55000);
			vertices[108] = new Vector3d (0.2240, -0.4000, 2.55000);
			vertices[109] = new Vector3d (0.0000, -0.4000, 2.55000);
			vertices[110] = new Vector3d (1.3000, 0.0000, 2.55000);
			vertices[111] = new Vector3d (1.3000, -0.7280, 2.55000);
			vertices[112] = new Vector3d (0.7280, -1.3000, 2.55000);
			vertices[113] = new Vector3d (0.0000, -1.3000, 2.55000);
			vertices[114] = new Vector3d (1.3000, 0.0000, 2.40000);
			vertices[115] = new Vector3d (1.3000, -0.7280, 2.40000);
			vertices[116] = new Vector3d (0.7280, -1.3000, 2.40000);
			vertices[117] = new Vector3d (0.0000, -1.3000, 2.40000);

			
			faces = new Quad[36];
			
			faces[0] = new Quad (vertices[0], vertices[1], vertices[2], vertices[3], Color.WhiteSmoke);
			faces[1] = new Quad (vertices[4], vertices[5], vertices[6], vertices[7], Color.WhiteSmoke);
			faces[2] = new Quad (vertices[8], vertices[9], vertices[10], vertices[11], Color.WhiteSmoke);
			faces[3] = new Quad (vertices[12], vertices[13], vertices[14], vertices[15], Color.WhiteSmoke);
			faces[4] = new Quad (vertices[16], vertices[17], vertices[18], vertices[19], Color.WhiteSmoke);
			faces[5] = new Quad (vertices[20], vertices[21], vertices[22], vertices[23], Color.WhiteSmoke);
			faces[6] = new Quad (vertices[24], vertices[25], vertices[26], vertices[27], Color.WhiteSmoke);
			faces[7] = new Quad (vertices[28], vertices[29], vertices[30], vertices[31], Color.WhiteSmoke);
			faces[8] = new Quad (vertices[32], vertices[33], vertices[34], vertices[35], Color.WhiteSmoke);
			faces[9] = new Quad (vertices[36], vertices[37], vertices[38], vertices[39], Color.WhiteSmoke);
			faces[10] = new Quad (vertices[40], vertices[41], vertices[42], vertices[43], Color.WhiteSmoke);
			faces[11] = new Quad (vertices[44], vertices[45], vertices[46], vertices[47], Color.WhiteSmoke);
			faces[12] = new Quad (vertices[48], vertices[49], vertices[50], vertices[51], Color.WhiteSmoke);
			faces[13] = new Quad (vertices[52], vertices[53], vertices[54], vertices[55], Color.WhiteSmoke);
			faces[14] = new Quad (vertices[56], vertices[57], vertices[58], vertices[59], Color.WhiteSmoke);
			faces[15] = new Quad (vertices[60], vertices[61], vertices[62], vertices[63], Color.WhiteSmoke);
			faces[16] = new Quad (vertices[64], vertices[65], vertices[66], vertices[67], Color.WhiteSmoke);
			faces[17] = new Quad (vertices[68], vertices[69], vertices[70], vertices[71], Color.WhiteSmoke);
			faces[18] = new Quad (vertices[72], vertices[73], vertices[74], vertices[75], Color.WhiteSmoke);
			faces[19] = new Quad (vertices[76], vertices[77], vertices[78], vertices[79], Color.WhiteSmoke);
			faces[20] = new Quad (vertices[80], vertices[81], vertices[82], vertices[83], Color.WhiteSmoke);
			faces[21] = new Quad (vertices[84], vertices[85], vertices[86], vertices[87], Color.WhiteSmoke);
			faces[22] = new Quad (vertices[89], vertices[90], vertices[91], vertices[92], Color.WhiteSmoke);
			faces[23] = new Quad (vertices[93], vertices[94], vertices[95], vertices[96], Color.WhiteSmoke);
			faces[24] = new Quad (vertices[97], vertices[98], vertices[99], vertices[100], Color.WhiteSmoke);
			faces[25] = new Quad (vertices[101], vertices[102], vertices[103], vertices[104], Color.WhiteSmoke);
			faces[26] = new Quad (vertices[105], vertices[106], vertices[107], vertices[108], Color.WhiteSmoke);
			faces[27] = new Quad (vertices[109], vertices[110], vertices[111], vertices[112], Color.WhiteSmoke);
			faces[28] = new Quad (vertices[113], vertices[114], vertices[115], vertices[116], Color.WhiteSmoke);
			/*faces[0] = new Quad (vertices[], vertices[], vertices[], vertices[], Color.WhiteSmoke);
			faces[0] = new Quad (vertices[], vertices[], vertices[], vertices[], Color.WhiteSmoke);
			faces[0] = new Quad (vertices[], vertices[], vertices[], vertices[], Color.WhiteSmoke);
			faces[0] = new Quad (vertices[], vertices[], vertices[], vertices[], Color.WhiteSmoke);
			faces[0] = new Quad (vertices[], vertices[], vertices[], vertices[], Color.WhiteSmoke);
			faces[0] = new Quad (vertices[], vertices[], vertices[], vertices[], Color.WhiteSmoke);
			faces[0] = new Quad (vertices[], vertices[], vertices[], vertices[], Color.WhiteSmoke);*/
			
			
			diffuse = 1.0;
			specular = 0.5;
			transparency = 0.0;
			reflect = 0.0;
			ambient = 0.3;
			
			colour = new double[3]{Color.WhiteSmoke.R,Color.WhiteSmoke.G,Color.WhiteSmoke.B };
			
			texture = null;
		

		}
	

		#region Primitive implementation
		public double intersect (OpenTK.Vector3d d0, OpenTK.Vector3d d1)
		{
			double intersectt = faces[0].intersect (d0, d1);
			for (int i = 1; i <= 28; i++) {
				double tempi = faces[i].intersect (d0, d1);
				if (tempi < intersectt && tempi > 0)
				{
					intersectt = tempi;
					lastnormal = faces[i].normal (d0);
				}
			}
			return intersectt;
			
		}

		public OpenTK.Vector3d normal (OpenTK.Vector3d point)
		{
			return lastnormal;	
		}

		public double[] getColour (OpenTK.Vector3d p)
		{
			return colour;
		}

		public double getDiffuse ()
		{
			return 1.0;
		}

		public double getSpecular ()
		{
			return 0.5;
		}

		public double getTransparency ()
		{
			return 0.0;
		}

		public double getReflect ()
		{
			return 0.0;
		}

		public double getAmbient ()
		{
			return 0.2;
		}
		
		public double getRefract ()
		{
			return 1.0;
		}

		public bool isLight ()
		{
			return false;
		}
		#endregion
}
}

