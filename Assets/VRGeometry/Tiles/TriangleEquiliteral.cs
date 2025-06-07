namespace Assets.MyStuffForNow.Tiles
{
	public class TriangleEquiliteral : Tile
	{
		public string Name = "TriangleEquiliteral";
		public string NameOfSnapfield = string.Empty;
		public bool HasTriangleParent = false;
		public string[] Directions = new[] { "Right", "Left", "Down" };

		public int Z_RotationSnapField = 0;
		public int Triangle_Z_RotationSnapField = 90;

		public float X_DistanceNewSnapFields = 2.73f;
		public readonly float Y_DistanceNewSnapFields = 4f;

		public float Y_AdjustmentNewSnapFields = 0.73f;
		public float Z_RotationNewSnapFields = 60;
	}
}