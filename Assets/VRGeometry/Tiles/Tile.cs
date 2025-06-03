using System.Numerics;

namespace Assets.MyStuffForNow.Tiles
{
	public abstract class Tile
	{
		string Name { get; }

		string NameOfSnapfield {get; set;}
		bool IsSnapped { get; set;}
		bool HasTriangleParent { get; set;}

		// WHAT DIRECTIONS IT HAS (Right Left Up Down)
		string[] Directions { get; set; }

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
