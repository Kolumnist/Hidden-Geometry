using UnityEngine;

namespace Assets.VRGeometry.Solutions.FoldSolutions
{
	public class CuboidFoldSolution : FoldSolution
	{
		private readonly float angleMaxLimit = 90;
		
		private float squaresCount = 0;
		private float quadCount = 0;

		internal override void CheckRequirements(Transform snapzone, Transform tile)
		{
			_ = tile.name.StartsWith(Names.Square) ? squaresCount++ : quadCount++;

			if ((!tile.name.StartsWith(Names.Square) && !tile.name.StartsWith(Names.Quad)) ||
				squaresCount > 2 ||
				quadCount > 4)
			{
				isCorrect = false;
			}

			if (!snapzone.name.Equals("Base"))
			{
				CreateHinge(snapzone, tile, angleMaxLimit);
				tileTransforms.Add(tile);
			}
		}

		internal override void AdditionalReset()
		{
			squaresCount = 0;
			quadCount = 0;
		}
	}
}
