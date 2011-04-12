using System;
using System.Drawing;
using OpenTK;
using NUnit.Framework;
namespace volkrenderer
{
	[TestFixture()]
	public class sphereTest
	{
		[Test()]
		public void TestCase ()
		{

			Sphere testSp = new Sphere (Color.FromName ("SlateBlue"), new Vector3d (0, 0, 20), 3);
			double result = testSp.intersect (new Vector3d (0, 0, 1));
			//i have no idea what this should be, fix it.
			Assert.AreEqual (0.0f,result);
		}
	}
}

