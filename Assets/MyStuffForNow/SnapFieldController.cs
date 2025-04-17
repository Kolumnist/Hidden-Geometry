using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapFieldController : MonoBehaviour
{

	// THIS CODE SUCKS

	[SerializeField] 
	private Transform baseTransform;

	private GameObject Right;
	private GameObject Down;
	private GameObject Left;
	private GameObject Up;

	// Starting values for square
	private float distance_Y = 1.2f;
	private float distance_X = 1.2f;

	private void CheckForDuplicationRecursive(Transform current, int currentlyFoundCount)
	{
		foreach (Transform child in current)
		{
			if (currentlyFoundCount == 3 || current == this.transform)
			{
				break;
			}

			if (Right != null && Vector3.Distance(Right.transform.position, child.position) < 0.1f)
			{
				Right.SetActive(false);
				currentlyFoundCount++;
				continue;
			}
			else if (Down != null && Vector3.Distance(Down.transform.position, child.position) < 0.1f)
			{
				Down.SetActive(false);
				currentlyFoundCount++;
				continue;
			}
			else if (Left != null && Vector3.Distance(Left.transform.position, child.position) < 0.1f)
			{
				Left.SetActive(false);
				currentlyFoundCount++;
				continue;
			}
			else if (Up != null && Vector3.Distance(Up.transform.position, child.position) < 0.1f)
			{
				Up.SetActive(false);
				currentlyFoundCount++;
				continue;
			}

			CheckForDuplicationRecursive(child, currentlyFoundCount);
		}
	}

	private void InstantiateAndSetObjects()
	{
		if (Right != null)
		{
			Right.name = "Right";
			Right.transform.SetPositionAndRotation(new Vector3(distance_X, 0, 0), Quaternion.identity);
			Right.transform.localScale = new Vector3(1f, 1f, 1f);
			Right.transform.SetParent(transform, false);
		}

		if (Down != null)
		{
			Down.name = "Down";
			Down.transform.SetPositionAndRotation(new Vector3(0, -distance_Y, 0), Quaternion.identity);
			Down.transform.localScale = new Vector3(1f, 1f, 1f);
			Down.transform.SetParent(transform, false);
		}

		if (Left != null)
		{
			Left.name = "Left";
			Left.transform.SetPositionAndRotation(new Vector3(-distance_X, 0, 0), Quaternion.identity);
			Left.transform.localScale = new Vector3(1f, 1f, 1f);
			Left.transform.SetParent(transform, false);
		}

		if (Up != null)
		{
			Up.name = "Up";
			Up.transform.SetPositionAndRotation(new Vector3(0, distance_Y, 0), Quaternion.identity);
			Up.transform.localScale = new Vector3(1f, 1f, 1f);
			Up.transform.SetParent(transform, false);
		}
	}

	public void DestructSnappers()
	{
		Debug.Log("Begin Destructing");
		/*Transform oldestInteractable = GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform;
		if (oldestInteractable.name.StartsWith("Quad"))
		{
			if(this.name == "Right")
				transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(0.3f, 0, 0), Quaternion.identity);
			else if(this.name == "Left")
				transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(-0.3f, 0, 0), Quaternion.identity);
		}*/

		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	public void CreateSnappers()
	{
		Debug.Log("Begin Creating");
		Debug.Log(transform.name);

		// I know this is dumb but it is in my mind necessary as I need the Instantiation before I set any position.
		// Also I need to duplicate or put this script on another object in the world the entire time, second option however is weird.
		Transform oldestInteractable = GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform;
		Debug.Log(oldestInteractable.name);

		if (oldestInteractable.name.StartsWith("Quad"))
		{
			distance_X = 1.5f;
			switch (this.name)
			{
				case "Base":
					Right = Instantiate(gameObject);
					//Down = Instantiate(gameObject);
					Left = Instantiate(gameObject);
					//Up = Instantiate(gameObject);
					
					InstantiateAndSetObjects();
					break;
				case "Right":
					Right = Instantiate(gameObject);
					//Down = Instantiate(gameObject);
					//Up = Instantiate(gameObject);

					transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(0.3f, 0, 0), Quaternion.identity);
					InstantiateAndSetObjects();
					break;
				/*case "D":
					Right = Instantiate(gameObject);
					//Down = Instantiate(gameObject);
					Left = Instantiate(gameObject);

					InstantiateAndSetObjects();
					break;*/
				case "Left":
					//Down = Instantiate(gameObject);
					Left = Instantiate(gameObject);
					//Up = Instantiate(gameObject);

					transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(-0.3f, 0, 0), Quaternion.identity);
					InstantiateAndSetObjects();
					break;
				/*case "U":
					Right = Instantiate(gameObject);
					Left = Instantiate(gameObject);
					Up = Instantiate(gameObject);

					InstantiateAndSetObjects();
					break;*/
				default:
					break;
			}
		}
		else
		{
			distance_X = 1.2f;

			switch (this.name)
			{
				case "Base":
					Right = Instantiate(gameObject);
					Down = Instantiate(gameObject);
					Left = Instantiate(gameObject);
					Up = Instantiate(gameObject);

					InstantiateAndSetObjects();
					break;
				case "Right":
					Right = Instantiate(gameObject);
					Down = Instantiate(gameObject);
					Up = Instantiate(gameObject);

					InstantiateAndSetObjects();
					break;
				case "Down":
					Right = Instantiate(gameObject);
					Down = Instantiate(gameObject);
					Left = Instantiate(gameObject);

					InstantiateAndSetObjects();
					break;
				case "Left":
					Down = Instantiate(gameObject);
					Left = Instantiate(gameObject);
					Up = Instantiate(gameObject);

					InstantiateAndSetObjects();
					break;
				case "Up":
					Right = Instantiate(gameObject);
					Left = Instantiate(gameObject);
					Up = Instantiate(gameObject);

					InstantiateAndSetObjects();
					break;
				default:
					break;
			}
		}
		CheckForDuplicationRecursive(transform.root, 0);
	}
}
