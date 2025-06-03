namespace Assets.MyStuffForNow.Tiles
{
	public class Quad : Tile
	{
		public string Name = "Quad";
		public string NameOfSnapfield = string.Empty;
		public bool HasTriangleParent = false;
		public string[] Directions = new[] { "Right", "Left", "Up", "Down" };

		public float X_DistanceSnapField = 1f;
		public float Y_DistanceSnapField = 0f;
		public int Z_RotationSnapField = 0;

		public float Triangle_X_DistanceSnapField = 0.86f;
		public float Triangle_Y_DistanceSnapField = 0.5f;
		public int Triangle_Z_RotationSnapField = 90;

		public float X_DistanceNewSnapFields = 5f;
		public readonly float Y_DistanceNewSnapFields = 4f;
		public float Z_RotationNewSnapFields = 0;
	}
}
