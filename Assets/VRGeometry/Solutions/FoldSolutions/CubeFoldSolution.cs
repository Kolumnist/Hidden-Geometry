using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Assets.VRGeometry.Solutions.FoldingSolution
{
	public class CubeFoldSolution : FoldSolution
	{
		private readonly float angleMaxLimit = 90;

		internal override void Recursive(Transform current)
		{
			if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count == 0 || !isCorrect)
			{
				return;
			}

			if (!current.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.name.StartsWith(Names.Square))
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

			if (tileTransforms.Count > 5)
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
