using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public abstract class Solution : MonoBehaviour
{
	[SerializeField]
	private Transform _3DObject;

	public Material correctMaterial;
	public Material falseMaterial;

	internal List<string> directions = new List<string>();
	internal List<Transform> snappers = new List<Transform>();
	internal List<Transform> emptySnappers = new List<Transform>();
	internal Transform errorTransform = null;

	internal int countRight = 0;
	internal int countLeft = 0;
	internal int countUp = 0;
	internal int countDown = 0;
	internal bool isCorrect = true;

	public virtual void StartAlgorithm() { }

	internal void EndSolution(int validAmount)
	{
		if (isCorrect && snappers.Count == validAmount)
		{
			Debug.Log("YAAAAAY");
			ChangeMaterial(true);
		}
		else
		{
			Debug.Log("Incorrect Net");
			ChangeMaterial(false);

			var selected = errorTransform.GetComponent<XRSocketInteractor>().interactablesSelected[0] ?? null;
			if (selected != null) 
				selected.transform.GetComponent<Renderer>().material = falseMaterial;
		}

		snappers.Clear();
		emptySnappers.Clear();
		directions.Clear();
		countRight = 0;
		countLeft = 0;
		countUp = 0;
		countDown = 0;
		isCorrect = true;
		errorTransform = null;
	}

	internal void CheckDirections(Transform current, string direction)
	{
		if (directions.Contains(direction))
		{
			isCorrect = false;
			errorTransform = current;
			return;
		}
		directions.Add(direction);
	}

	private void ChangeMaterial(bool isCorrect)
	{
		List<Renderer> renderers = new List<Renderer>();

		foreach(Transform child in _3DObject)
		{
			renderers.Add(child.GetComponent<Renderer>());
		}

		foreach (Renderer renderer in renderers)
		{
			renderer.material = isCorrect ? correctMaterial : falseMaterial;
		}
	}
}
