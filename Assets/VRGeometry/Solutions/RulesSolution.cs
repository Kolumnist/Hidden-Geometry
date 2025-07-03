using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

// OLD WAY AND TECHNICALLY DEPRECATED
public abstract class RulesSolution : MonoBehaviour
{
	[SerializeField]
	private Transform _3DObject;

	[SerializeField]
	internal Transform baseTransform;

	public Material correctMaterial;
	public Material falseMaterial;

	internal List<string> directions = new List<string>();
	internal List<Transform> snapzones = new List<Transform>();
	internal Transform errorTransform;

	internal int countRight = 0;
	internal int countLeft = 0;
	internal int countUp = 0;
	internal int countDown = 0;
	internal bool isCorrect = true;

	public virtual void StartAlgorithm() { }

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

	internal void EndSolution(int validAmount)
	{
		if (isCorrect && snapzones.Count == validAmount)
		{
			if (_3DObject.GetComponent<Renderer>() != null)
			{
				_3DObject.GetComponent<Renderer>().material = correctMaterial;
			}
		}
		else
		{
			if (_3DObject.GetComponent<Renderer>() != null)
			{
				_3DObject.GetComponent<Renderer>().material = falseMaterial;
			}

			var selected = errorTransform == null ? null : errorTransform.GetComponent<XRSocketInteractor>().interactablesSelected[0];
			if (selected != null) selected.transform.GetComponent<Renderer>().material = falseMaterial;
		}

		snapzones.Clear();
		directions.Clear();
		countRight = 0;
		countLeft = 0;
		countUp = 0;
		countDown = 0;
		isCorrect = true;
		errorTransform = null;
	}
}
