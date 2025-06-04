using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TriangularPyramidSolution : RulesSolution
{
	bool isRightFSIR = false;
	bool isLeftFSIR = false;
	bool isDownFSIR = false;

	public override void StartAlgorithm()
	{
		DirectionsRecursive(this.transform);
		EndSolution(4);

		isRightFSIR = false;
		isLeftFSIR = false;
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

		// I have to do this differently somehow but currently its working. It basically determines if I use only Triangle or not
		if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count != 0 && !current.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.name.StartsWith("Triangle"))
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
				isDownFSIR = false;
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

		if (count == 1)
		{
			snapfields.Add(current);
			CheckDirections(current, !isDownFSIR ? current.name : OppositeDirectionOf(current.name));
			return;
		}

		if (count == 2 && isFirstStartingInRoot)
		{
			snapfields.Add(current);
			CheckDirections(current, !isDownFSIR ? "Down" : OppositeDirectionOf(current.name));
			return;
		}

		isCorrect = false;
		errorTransform = current;
	}

	private string OppositeDirectionOf(string direction)
	{
		switch (direction)
		{
			case "Right":
				return "Left";
			case "Left":
				return "Right";
			default: return "";
		}
	}
}
