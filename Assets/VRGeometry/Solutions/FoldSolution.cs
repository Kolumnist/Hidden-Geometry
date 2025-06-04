using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Assets.VRGeometry.Solutions
{
	public abstract class RulesSolution : MonoBehaviour
	{
		[SerializeField]
		private Transform _3DObject;

		public Material correctMaterial;
		public Material falseMaterial;

		internal List<string> directions = new List<string>();
		internal List<Transform> snapfields = new List<Transform>();
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
			if (isCorrect && snapfields.Count == validAmount)
			{
				Debug.Log("YAAAAAY");
				ChangeMaterial(true);
			}
			else
			{
				Debug.Log("Incorrect Net");
				ChangeMaterial(false);

				var selected = errorTransform == null ? null : errorTransform.GetComponent<XRSocketInteractor>().interactablesSelected[0];
				if (selected != null)
					selected.transform.GetComponent<Renderer>().material = falseMaterial;
			}

			snapfields.Clear();
			directions.Clear();
			countRight = 0;
			countLeft = 0;
			countUp = 0;
			countDown = 0;
			isCorrect = true;
			errorTransform = null;
		}

		private void ChangeMaterial(bool isCorrect)
		{
			List<Renderer> renderers = new List<Renderer>();

			foreach (Transform child in _3DObject)
			{
				renderers.Add(child.GetComponent<Renderer>());
			}

			foreach (Renderer renderer in renderers)
			{
				renderer.material = isCorrect ? correctMaterial : falseMaterial;
			}
		}
	}

}
