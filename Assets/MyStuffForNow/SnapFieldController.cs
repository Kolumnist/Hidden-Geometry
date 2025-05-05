using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapFieldController : MonoBehaviour
{
	[SerializeField] 
	private Transform baseTransform;

	private GameObject Right;
	private GameObject Down;
	private GameObject Left;
	private GameObject Up;

	private int zRotation = 0;

	// new snapperobjects position and rotation
	private float otherDistanceX = 4f;
	private readonly float otherDistanceY = 4f;
	private float otherAdjustmentY = 0f;
	private float otherZRotation = 0f;

	private void CheckForDuplicationRecursive(Transform current, int currentlyFoundCount)
	{
		if (current == null) return;

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

	private GameObject InstantiateSnapper(string name, Vector3 position, float zRotation)
	{
		GameObject snapper = Instantiate(this.gameObject);
		snapper.name = name;
		snapper.transform.SetPositionAndRotation(position, Quaternion.identity);
		snapper.transform.Rotate(new Vector3(0, 0, zRotation));
		snapper.transform.localScale = new Vector3(1f, 1f, 1f);
		snapper.transform.SetParent(transform, false);
		
		return snapper;
	}

	public void CreateSnappers()
	{
		Transform oldestInteractable = GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform;
		
		Debug.Log("Creates: " + transform.name + " which holds a " + oldestInteractable.name);

		if (oldestInteractable.name.StartsWith("Quad"))
		{
			otherDistanceX = 1.5f;
			/*switch (this.name)
			{
				case "Base":
					Right = Instantiate(gameObject);
					//Down = Instantiate(gameObject);
					Left = Instantiate(gameObject);
					//Up = Instantiate(gameObject);
					
					SetSnapperValues();
					break;
				case "Right":
					Right = Instantiate(gameObject);
					//Down = Instantiate(gameObject);
					//Up = Instantiate(gameObject);

					transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(0.3f, 0, 0), Quaternion.identity);
					SetSnapperValues();
					break;
				case "D":
					Right = Instantiate(gameObject);
					//Down = Instantiate(gameObject);
					Left = Instantiate(gameObject);

					InstantiateAndSetObjects();
					break;
				case "Left":
					//Down = Instantiate(gameObject);
					Left = Instantiate(gameObject);
					//Up = Instantiate(gameObject);

					transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(-0.3f, 0, 0), Quaternion.identity);
					SetSnapperValues();
					break;
				case "U":
					Right = Instantiate(gameObject);
					Left = Instantiate(gameObject);
					Up = Instantiate(gameObject);

					InstantiateAndSetObjects();
					break;
				default:
					break;
			}*/
		}
		else if(oldestInteractable.name.StartsWith("Triangle"))
		{
			otherDistanceX = 2.73f;
			otherAdjustmentY = 0.73f;
			otherZRotation = 60;
			zRotation = transform.parent != null && transform.parent.GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform.name.StartsWith("Triangle") ? 0 : 90;

			Right = InstantiateSnapper("Right", new Vector3(otherDistanceX, otherAdjustmentY, 0), -otherZRotation);
			Left = InstantiateSnapper("Left", new Vector3(-otherDistanceX, otherAdjustmentY, 0), otherZRotation);

			switch (this.name)
			{
				case "Right":
					transform.Rotate(new Vector3(0, 0, zRotation = -zRotation));
					break;
				case "Left":
					transform.Rotate(new Vector3(0, 0, zRotation));
					break;
				case "Down":
					transform.Rotate(new Vector3(0, 0, zRotation = 180));
					break;
				default:
					Down = InstantiateSnapper("Down", new Vector3(0, -otherDistanceY, 0), 0);
					break;
			}
		}
		else
		{
			Right = InstantiateSnapper("Right", new Vector3(otherDistanceX, otherAdjustmentY, 0), -otherZRotation);
			Left = InstantiateSnapper("Left", new Vector3(-otherDistanceX, otherAdjustmentY, 0), otherZRotation);
			Up = InstantiateSnapper("Up", new Vector3(0, otherDistanceY, 0), 0);
			Down = InstantiateSnapper("Down", new Vector3(0, -otherDistanceY, 0), 0);
		}

		otherDistanceX = 4f;
		otherAdjustmentY = 0f;
		otherZRotation = 0;

		CheckForDuplicationRecursive(baseTransform, 0);
	}

	public void DestructSnappers()
	{
		Debug.Log("Begin Destructing for " + this.gameObject);
		/*Transform oldestInteractable = GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform;
		if (oldestInteractable.name.StartsWith("Quad"))
		{
			if(this.name == "Right")
				transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(0.3f, 0, 0), Quaternion.identity);
			else if(this.name == "Left")
				transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(-0.3f, 0, 0), Quaternion.identity);
		}*/
		if (zRotation != 0)
		{
			this.transform.Rotate(0, 0, -zRotation);
		}

		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}
}
