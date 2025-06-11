using UnityEngine;

namespace Assets.VRGeometry.Solutions.FoldingSolution
{
	public class CubeFoldSolution : FoldSolution
	{
		private readonly float angleMaxLimit = 90;

		internal override void CheckRequirements(Transform snapzone, Transform tile)
		{
			if (!tile.name.StartsWith(Names.Square) ||
				tileTransforms.Count > 5)
			{
				isCorrect = false;
				return;
			}

			if (!snapzone.name.Equals("Base"))
			{
				CreateHinge(snapzone, tile, angleMaxLimit);
				tileTransforms.Add(tile);
			}
		}
	}
}
