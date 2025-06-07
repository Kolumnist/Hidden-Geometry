using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Assets.VRGeometry.Solutions.FoldSolutions
{
	public class PyramidSquareFoldSolution : FoldSolution
	{
		private readonly float angleMaxLimit = 125.5f;
		private readonly float angleMaxLimitTrianglesAround = 70.5f;

		internal override void Recursive(Transform current)
		{
			if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count == 0 || !isCorrect)
			{
				return;
			}

			/*if (!current.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.name.StartsWith(Names.TriangleEqui))
			{
				isCorrect = false;
				return;
			}*/

			if (!current.name.Equals("Base"))
			{
				Transform tile = current.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;
				Transform parentTile = current.parent.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;
				float angle = parentTile.name.StartsWith(Names.Square) || tile.name.StartsWith(Names.Square) ? angleMaxLimit : angleMaxLimitTrianglesAround;
				CreateHinge(current, tile, angle);
				tileTransforms.Add(tile);
			}

			/*if (tileTransforms.Count > 3)
			{
				isCorrect = false;
				return;
			}*/

			foreach (Transform child in current)
			{
				Recursive(child);
			}
		}
	}
}
