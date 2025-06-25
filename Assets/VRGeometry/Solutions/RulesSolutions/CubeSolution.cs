using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/* Rules
 * 
/ Rule 1: Start Transform is my floor
/ Rule 2: First in the same hierarchy horizontally and vertically are exact R=R, L=L, U=U, D=D
/ Rule 3: If the "First starting in Root" repeats itself once then it is Ceiling
/ Rule 4: If any direction repeats the first time, then it is the opposite of the "First starting in Root"
/ Rule 5: If any direction repeats a second time, then it is the opposite of itself
 *
*/

public class CubeSolution : RulesSolution
{
	// FSIR = First Starting in Root
	bool isRightFSIR = false;
	bool isLeftFSIR = false;
	bool isUpFSIR = false;
	bool isDownFSIR = false;

	bool noInstantCeiling = false;

	public override void StartAlgorithm()
	{
		DirectionsRecursive(baseTransform);

		EndSolution(6);

		isRightFSIR = false;
		isLeftFSIR = false;
		isUpFSIR = false;
		isDownFSIR = false;
		noInstantCeiling = false;
	}

	private void DirectionsRecursive(Transform current)
	{
		if (!isCorrect)
		{
			return;
		}

		if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count == 0)
		{
			return;
		}

		// I have to do this differently somehow but currently its working. It basically determines if I use only square or not
		if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count != 0 && !current.GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform.name.StartsWith("Square"))
		{
			isCorrect = false;
			errorTransform = current;
			return;
		}

		switch (current.name)
		{
			case "Right":
				ApplyRules(current, ref isRightFSIR, ref countRight);
				break;
			case "Left":
				ApplyRules(current, ref isLeftFSIR, ref countLeft);
				break;
			case "Up":
				ApplyRules(current, ref isUpFSIR, ref countUp);
				break;
			case "Down":
				ApplyRules(current, ref isDownFSIR, ref countDown);
				break;
			default: 
				snapfields.Add(current);
				break;
		}

		foreach (Transform child in current)
		{
			DirectionsRecursive(child);

			if (current.name.Equals("Base"))
			{
				countRight = 0;
				countLeft = 0;
				countUp = 0;
				countDown = 0;
			}
		}
	}

	private void ApplyRules(Transform current, ref bool isFirstStartingInRoot, ref int count)
	{
		count++;

		if (current.parent.name == "Base" && current.childCount != 0)
		{
			isFirstStartingInRoot = true;
		}
		
		// Rule 2: First in the same hierarchy horizontally and vertically are exact
		if (count == 1)
		{
			snapfields.Add(current);
			CheckDirections(current, current.name);
			return;
		}
		
		// Rule 3: If the "First starting in Root" repeats itself once then it is Ceiling
		if (isFirstStartingInRoot && count == 2)
		{
			snapfields.Add(current);

			// >:[
			if (!current.transform.name.Equals(current.parent.transform.name))
			{
				noInstantCeiling = true;
			}
		
			CheckDirections(current, "Ceiling");
			return;
		}
		
		// Rule 4: If any direction repeats the first time, then it is the opposite of the "First starting in Root"
		if (count == 2)
		{
			snapfields.Add(current);

			Transform firstStartingInRoot = current.parent;
			while(!firstStartingInRoot.parent.name.Equals("Base"))
			{
				firstStartingInRoot = firstStartingInRoot.parent;
			}
			OppositeDirectionOf(current, firstStartingInRoot.name);
			return;
		}

		// Rule 5: If any direction repeats a second time, then it is the opposite of itself
		if (count == 3)
		{
			snapfields.Add(current);

			// >:[
			if (noInstantCeiling)
			{
				switch (current.name)
				{
					case "Right":
						CheckDirections(current, "Down");
						break;
					case "Left":
						CheckDirections(current, "Up");
						break;
					case "Up":
						CheckDirections(current, "Right");
						break;
					case "Down":
						CheckDirections(current, "Left");
						break;
				}
				return;
			}

			OppositeDirectionOf(current, current.name); 
			return;
		}

		isCorrect = false;
		errorTransform = current;
	}

	private void OppositeDirectionOf(Transform current, string direction)
	{
		switch (direction)
		{
			case "Right":
				CheckDirections(current, "Left");
				break;
			case "Left":
				CheckDirections(current, "Right");
				break;
			case "Up":
				CheckDirections(current, "Down");
				break;
			case "Down":
				CheckDirections(current, "Up");
				break;
		}
	}
}
