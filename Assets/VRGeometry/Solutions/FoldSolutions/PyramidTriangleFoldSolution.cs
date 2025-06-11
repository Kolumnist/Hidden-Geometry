using UnityEngine;

namespace Assets.VRGeometry.Solutions.FoldingSolution
{
	public class PyramidTriangleFoldSolution : FoldSolution
	{
		private readonly float angleMaxLimit = 109.5f;
		
		internal override void CheckRequirements(Transform snapzone, Transform tile)
		{
			if (!tile.name.StartsWith(Names.TriangleEqui) ||
				tileTransforms.Count > 3)
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
