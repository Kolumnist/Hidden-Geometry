using System.Numerics;

namespace Assets.MyStuffForNow.Tiles
{
	public abstract class Tile
	{
		string Name { get; }
		float a { get; }
		float b { get; }

		string NameOfSnapzone {get; set;}
		bool IsSnapped { get; set;}
		bool HasTriangleParent { get; set;}

		// APPLIED ON THE Snapzone
		float X_DistanceSnapzone { get; set; }
		float Y_DistanceSnapzone { get; set; }
		int Z_RotationSnapzone { get; set; }

		// APPLIED ON THE Snapzone
		float Triangle_X_DistanceSnapzone { get; set; }
		float Triangle_Y_DistanceSnapzone { get; set; }
		int Triangle_Z_RotationSnapzone { get; set; }

		// Vector3 localPosition
		// Vector3 rotation;

		// APPLIED TO THE NEW SnapzoneS
		float X_DistanceNewSnapzones { get; set; }
		float Y_DistanceNewSnapzones { get; }
		float Y_AdjustmentNewSnapzones { get; set; }
		float Z_RotationNewSnapzones { get; set; }
	}
}
