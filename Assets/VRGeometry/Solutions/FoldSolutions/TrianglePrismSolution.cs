using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Assets.VRGeometry.Solutions.FoldSolutions
{
	public class TrianglePrismSolution : FoldSolution
	{
		private readonly float angleMaxLimit = 90;
		private readonly float squareNextToSquareAngleMaxLimit = 120;

		private float squaresCount = 0;
		private float triangleCount = 0;

		internal override void CheckRequirements(Transform snapzone, Transform tile)
		{
			_ = tile.name.StartsWith(Names.Square) ? squaresCount++ : triangleCount++;

			if (!tile.name.StartsWith(Names.TriangleEqui) && !tile.name.StartsWith(Names.Square) ||
				triangleCount > 2 ||
				squaresCount > 3)
			{
				isCorrect = false;
			}

			if (!snapzone.name.Equals("Base"))
			{
				Transform parentTile = snapzone.parent.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;
				float angle = parentTile.name.StartsWith(Names.Square) && tile.name.StartsWith(Names.Square) 
					? squareNextToSquareAngleMaxLimit : angleMaxLimit;
				CreateHinge(snapzone, tile, angle);
				tileTransforms.Add(tile);
			}
		}

		internal override void AdditionalReset()
		{
			squaresCount = 0;
			triangleCount = 0;
		}
	}
}
