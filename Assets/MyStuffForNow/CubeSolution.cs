using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


/* Rules
 * 
/ Rule 1: Start Transform is floor
/ Rule 2: First in the same hierarchy horizontally and vertically are exact R=R, L=L, U=U, D=D
/ Rule 3: If the "First starting in Root" repeats itself once then it is Ceiling
/ Rule 4: If any direction repeats the first time, then it is the opposite of the "First starting in Root"
/ Rule 5: If any direction repeats a second time, then it is the opposite of itself
 *
*/

public class CubeSolution : MonoBehaviour
{
	ObjectMaterialChanger objectMaterialChanger;

	List<string> directions = new List<string>();
	List<Transform> invalidSnappers = new List<Transform>();

    int countRight = 0;
    int countLeft = 0;
    int countUp = 0;
    int countDown = 0;

	// FSIR = First Starting in Root
	bool isRightFSIR = false;
	bool isLeftFSIR = false;
	bool isUpFSIR = false;
	bool isDownFSIR = false;

	bool noInstantCeiling = false;

	bool getsTerminated = false;
	
	public void Start()
	{
		if (this.transform.name.Equals("Base"))
		{
			objectMaterialChanger = GetComponent<ObjectMaterialChanger>();
			//DirectionsRecursive(this.transform);
		}
	}

	public void Solution()
	{
		DirectionsRecursive(this.transform);

		if (!getsTerminated)
		{
			Debug.Log("YAAAAAY");
			objectMaterialChanger.ChangeMaterial(true);
		}

		isRightFSIR = false;
		isLeftFSIR = false;
		isUpFSIR = false;
		isDownFSIR = false;
		noInstantCeiling = false;
		getsTerminated = false;
		countRight = 0;
		countLeft = 0;
		countUp = 0;
		countDown = 0;
		directions.Clear();
		invalidSnappers.Clear();
	}

	private void DirectionsRecursive(Transform current)
	{
		if (getsTerminated)
		{
			// For later you could try and save which transform made problems and change their color.
			return;
		}

		if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count == 0 
			|| !current.GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform.name.StartsWith("Square"))
		{
			invalidSnappers.Add(current);
			return;
		}

		switch (current.name)
		{
			case "Right":
				countRight++;
				ApplyRules(current, ref isRightFSIR, countRight);
				break;
			case "Left":
				countLeft++;
				ApplyRules(current, ref isLeftFSIR, countLeft);
				break;
			case "Up":
				countUp++;
				ApplyRules(current, ref isUpFSIR, countUp);
				break;
			case "Down":
				countDown++;
				ApplyRules(current, ref isDownFSIR, countDown);
				break;
			
			// Rule 1: Start Transform is floor
			default: directions.Add("Floor");
				break;
		}

		foreach (Transform child in current)
		{
			DirectionsRecursive(child);
		}
	}

	private void ApplyRules(Transform current, ref bool isFirstStartingInRoot, int count)
	{
		if (current.parent.name == "Base" && current.childCount != 0)
		{
			isFirstStartingInRoot = true;
		}

		// Rule 2: First in the same hierarchy horizontally and vertically are exact
		if (count == 1)
		{
			CheckDirections(current.name);
			directions.Add(current.name);
			return;
		}
		
		// Rule 3: If the "First starting in Root" repeats itself once then it is Ceiling
		if (isFirstStartingInRoot && count == 2)
		{
			//I hate this but there is ONE f edge case
			if (!current.transform.name.Equals(current.parent.transform.name))
			{
				noInstantCeiling = true;
			}
		
			CheckDirections("Ceiling");
			directions.Add("Ceiling");
			return;
		}
		
		// Rule 4: If any direction repeats the first time, then it is the opposite of the "First starting in Root"
		if (count == 2)
		{
			Transform firstStartingInRoot = current.parent;
			while(!firstStartingInRoot.parent.name.Equals("Base"))
			{
				firstStartingInRoot = firstStartingInRoot.parent;
			}
			OppositeDirectionOf(firstStartingInRoot.name);
			return;
		}

		// Rule 5: If any direction repeats a second time, then it is the opposite of itself
		if (count == 3)
		{
			// Temporary Solution for frickn edge case
			if (noInstantCeiling)
			{
				switch (current.name)
				{
					case "Right":
						CheckDirections("Down");
						directions.Add("Down");
						break;
					case "Left":
						CheckDirections("Up");
						directions.Add("Up");
						break;
					case "Up":
						CheckDirections("Right");
						directions.Add("Right");
						break;
					case "Down":
						CheckDirections("Left");
						directions.Add("Left");
						break;
				}
				return;
			}
			OppositeDirectionOf(current.name); 
			return;
		}

		getsTerminated = true;
	}

	private void CheckDirections(string direction)
	{
		if (directions.Contains(direction))
		{
			Debug.Log("Incorrect Net");
			getsTerminated = true;
			objectMaterialChanger.ChangeMaterial(false);
		}
	}

	private void OppositeDirectionOf(string direction)
	{
		switch (direction)
		{
			case "Right":
				CheckDirections("Left");
				directions.Add("Left");
				break;
			case "Left":
				CheckDirections("Right");
				directions.Add("Right");
				break;
			case "Up":
				CheckDirections("Down");
				directions.Add("Down");
				break;
			case "Down":
				CheckDirections("Up");
				directions.Add("Up");
				break;
		}
	}
}
