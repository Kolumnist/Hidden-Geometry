namespace Assets.MyStuffForNow.Tiles
{
	public class TriangleEquiliteral : Tile
	{
		public string Name = "TriangleEquiliteral";
		public string NameOfSnapzone = string.Empty;
		public bool HasTriangleParent = false;
		public string[] Directions = new[] { "Right", "Left", "Down" };

		public int Z_RotationSnapzone = 0;
		public int Triangle_Z_RotationSnapzone = 90;

		public float X_DistanceNewSnapzones = 2.73f;
		public readonly float Y_DistanceNewSnapzones = 4f;

		public float Y_AdjustmentNewSnapzones = 0.73f;
		public float Z_RotationNewSnapzones = 60;
	}
}