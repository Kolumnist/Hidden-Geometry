namespace Assets.MyStuffForNow.Tiles
{
	public class Quad : Tile
	{
		public string Name = "Quad";
		public string NameOfSnapzone = string.Empty;
		public bool HasTriangleParent = false;
		public string[] Directions = new[] { "Right", "Left", "Up", "Down" };

		public float X_DistanceSnapzone = 1f;
		public float Y_DistanceSnapzone = 0f;
		public int Z_RotationSnapzone = 0;

		public float Triangle_X_DistanceSnapzone = 0.86f;
		public float Triangle_Y_DistanceSnapzone = 0.5f;
		public int Triangle_Z_RotationSnapzone = 90;

		public float X_DistanceNewSnapzones = 5f;
		public readonly float Y_DistanceNewSnapzones = 4f;
		public float Z_RotationNewSnapzones = 0;
	}
}
