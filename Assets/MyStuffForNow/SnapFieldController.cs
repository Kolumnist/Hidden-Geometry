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

	// This Gameobject
	private int zRotation = 0;

	// Instantiate Objects Position and Rotation
	private float otherDistanceX = 4f;
	private float otherDistanceY = 4f;

	private float otherAdjustmentY = 0f;

	private float otherZRotation = 0f;

	private void CheckForDuplicationRecursive(Transform current, int currentlyFoundCount)
	{
		foreach (Transform child in current)
		{
			Debug.Log(child.name);
			if (currentlyFoundCount == 3 || current == this.transform)
			{
				break;
			}

			if (Right != null && Vector3.Distance(Right.transform.position, child.position) < 0.21f)
			{
				Destroy(Right);
				currentlyFoundCount++;
			}
			else if (Down != null && Vector3.Distance(Down.transform.position, child.position) < 0.21f)
			{
				Destroy(Down);
				currentlyFoundCount++;
			}
			else if (Left != null && Vector3.Distance(Left.transform.position, child.position) < 0.21f)
			{
				Destroy(Left);
				currentlyFoundCount++;
			}
			else if (Up != null && Vector3.Distance(Up.transform.position, child.position) < 0.21f)
			{
				Destroy(Up);
				currentlyFoundCount++;
			}

			CheckForDuplicationRecursive(child, currentlyFoundCount);
		}
	}

	private void InstantiateAndSetObjects()
	{
		if (Right != null)
		{
			Right.name = "Right";
			Right.transform.SetPositionAndRotation(new Vector3(otherDistanceX, otherAdjustmentY, 0), Quaternion.identity);
			Right.transform.Rotate(new Vector3(0,0, -otherZRotation));
			Right.transform.localScale = new Vector3(1f, 1f, 1f);
			Right.transform.SetParent(transform, false);
		}

		if (Down != null)
		{
			Down.name = "Down";
			Down.transform.SetPositionAndRotation(new Vector3(0, -otherDistanceY, 0), Quaternion.identity);
			Down.transform.localScale = new Vector3(1f, 1f, 1f);
			Down.transform.SetParent(transform, false);
		}

		if (Left != null)
		{
			Left.name = "Left";
			Left.transform.SetPositionAndRotation(new Vector3(-otherDistanceX, otherAdjustmentY, 0), Quaternion.identity);
			Left.transform.Rotate(new Vector3(0, 0, otherZRotation));
			Left.transform.localScale = new Vector3(1f, 1f, 1f);
			Left.transform.SetParent(transform, false);
		}

		if (Up != null)
		{
			Up.name = "Up";
			Up.transform.SetPositionAndRotation(new Vector3(0, otherDistanceY, 0), Quaternion.identity);
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
		if(zRotation != 0)
		{
			this.transform.Rotate(0,0, -zRotation);
		}

		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	public void CreateSnappers()
	{
		Debug.Log("Begin Creating");
		Debug.Log("Creates: " + transform.name);

		// I know this is dumb but it is in my mind necessary as I need the Instantiation before I set any position.
		// Also I need to duplicate or put this script on another object in the world the entire time, second option however is weird.
		Transform oldestInteractable = GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform;
		Debug.Log("Holds a " + oldestInteractable.name);

		if (oldestInteractable.name.StartsWith("Quad"))
		{
			otherDistanceX = 1.5f;
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
		else if(oldestInteractable.name.StartsWith("Triangle"))
		{
			otherDistanceX = 2.73f;
			otherAdjustmentY = 0.73f;
			otherZRotation = 60;
			zRotation = transform.parent != null && transform.parent.GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform.name.StartsWith("Triangle") ? 0 : 90;

			Right = Instantiate(gameObject);
			Left = Instantiate(gameObject);
			switch (this.name)
			{
				case "Base":
					Down = Instantiate(gameObject);
					break;
				case "Right":
					transform.Rotate(new Vector3(0, 0, zRotation = -zRotation));
					break;
				case "Down":
					transform.Rotate(new Vector3(0, 0, zRotation = 180));
					break;
				case "Left":
					transform.Rotate(new Vector3(0, 0, zRotation));
					break;
				default:
					break;
			}
			InstantiateAndSetObjects();
		}
		else
		{
			Right = Instantiate(gameObject);
			Down = Instantiate(gameObject);
			Left = Instantiate(gameObject);
			Up = Instantiate(gameObject);

			InstantiateAndSetObjects();
		}
		otherDistanceX = 4f;
		otherDistanceY = 4f;
		otherAdjustmentY = 0f;
		otherZRotation = 0;

		CheckForDuplicationRecursive(transform.root, 0);
	}
}
