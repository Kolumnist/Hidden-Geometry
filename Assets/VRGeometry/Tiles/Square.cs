using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.VRGeometry.Tiles
{
	public class Square
	{
		string Name { get; }
		float a { get; }
		
		Square(float scale)
		{
			Name = Names.Square;
			a = 2 * scale;


		}

		string NameOfSnapfield { get; set; }
		bool IsSnapped { get; set; }
		bool HasTriangleParent { get; set; }

		// APPLIED ON THE SNAPFIELD
		float X_DistanceSnapField { get; set; }
		float Y_DistanceSnapField { get; set; }
		int Z_RotationSnapField { get; set; }

		// APPLIED ON THE SNAPFIELD
		float Triangle_X_DistanceSnapField { get; set; }
		float Triangle_Y_DistanceSnapField { get; set; }
		int Triangle_Z_RotationSnapField { get; set; }

		// Vector3 localPosition
		// Vector3 rotation;

		// APPLIED TO THE NEW SNAPFIELDS
		float X_DistanceNewSnapFields { get; set; }
		float Y_DistanceNewSnapFields { get; }
		float Y_AdjustmentNewSnapFields { get; set; }
		float Z_RotationNewSnapFields { get; set; }
	}
}
