using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Assets.VRGeometry.Solutions.FoldSolutions
{
	public class CuboidFoldSolution : FoldSolution
	{
		private readonly float angleMaxLimit = 90;
		
		private float squaresCount = 0;
		private float quadCount = 0;

		internal override void Recursive(Transform current)
		{
			if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count == 0 || !isCorrect)
			{
				return;
			}

			Transform tile = current.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;

			if ((!tile.name.StartsWith(Names.Square) && !tile.name.StartsWith(Names.Quad)) ||
				(tile.name.StartsWith(Names.Square) && squaresCount >= 2) || 
				(tile.name.StartsWith(Names.Quad) && quadCount >= 4))
			{
				isCorrect = false;
				return;
			}

			if (!current.name.Equals("Base"))
			{
				_ = tile.name.StartsWith(Names.Square) ? squaresCount++ : quadCount++;
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
			squaresCount = 0;
			quadCount = 0;
		}
	}
}
