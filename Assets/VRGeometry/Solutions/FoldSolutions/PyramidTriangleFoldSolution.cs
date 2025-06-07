using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Assets.VRGeometry.Solutions.FoldingSolution
{
	public class PyramidTriangleFoldSolution : FoldSolution
	{
		private readonly float angleMaxLimit = 109.5f;

		internal override void Recursive(Transform current)
		{
			if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count == 0 || !isCorrect)
			{
				return;
			}

			if (!current.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.name.StartsWith(Names.TriangleEqui))
			{
				isCorrect = false;
				return;
			}

			if (!current.name.Equals("Base"))
			{
				Transform tile = current.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;
				CreateHinge(current, tile, angleMaxLimit);
				tileTransforms.Add(tile);
			}

			if (tileTransforms.Count > 3)
			{
				isCorrect = false;
				return;
			}

			foreach (Transform child in current)
			{
				Recursive(child);
			}
		}
	}
}
