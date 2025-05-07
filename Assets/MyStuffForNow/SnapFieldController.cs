using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapFieldController : MonoBehaviour
{
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

	private void Start()
	{
		baseTransform = this.transform;
		while (!baseTransform.name.Equals("Base"))
		{
			baseTransform = baseTransform.parent;
		}
	}

	private void DeleteDuplicatesRecursive(Transform current, int currentlyFoundCount)
	{
		float maxDistance = 0.21f;
		if (current == null) return;

		foreach (Transform child in current)
		{
			Debug.Log(child.name);
			DeleteDuplicatesRecursive(child, currentlyFoundCount);

			if (currentlyFoundCount == 3 || current == this.transform)
			{
				break;
			}

			var x = Vector3.Distance(baseTransform.position, Right.transform.position);

			if (Right != null && (Vector3.Distance(Right.transform.position, child.position) < maxDistance || Vector3.Distance(baseTransform.position, Right.transform.position) < maxDistance))
			{
				Destroy(Right);
				currentlyFoundCount++;
			}
			else if (Left != null && (Vector3.Distance(Left.transform.position, child.position) < maxDistance || Vector3.Distance(baseTransform.position, Left.transform.position) < maxDistance))
			{
				Destroy(Left);
				currentlyFoundCount++;
			}
			else if (Up != null && (Vector3.Distance(Up.transform.position, child.position) < maxDistance || Vector3.Distance(baseTransform.position, Up.transform.position) < maxDistance))
			{
				Destroy(Up);
				currentlyFoundCount++;
			}
			else if (Down != null && (Vector3.Distance(Down.transform.position, child.position) < maxDistance || Vector3.Distance(baseTransform.position, Down.transform.position) < maxDistance))
			{
				Destroy(Down);
				currentlyFoundCount++;
			}
		}
	}

	private void SetSnapper(ref GameObject snapper, string name, Vector3 position, float zRotation)
	{
		snapper.name = name;
		snapper.transform.SetPositionAndRotation(position, Quaternion.identity);
		snapper.transform.Rotate(new Vector3(0, 0, zRotation));
		snapper.transform.localScale = new Vector3(1f, 1f, 1f);
		snapper.transform.SetParent(transform, false);
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
			
			// Help making this better appreciated
			zRotation = !transform.name.Equals("Base") && transform.parent.GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform.name.StartsWith("Triangle") ? 0 : 90;

			Right = Instantiate(this.gameObject);
			Left = Instantiate(this.gameObject);

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
					Down = Instantiate(this.gameObject);
					SetSnapper(ref Down, "Down", new Vector3(0, -otherDistanceY, 0), 0);
					break;
			}
			SetSnapper(ref Right, "Right", new Vector3(otherDistanceX, otherAdjustmentY, 0), -otherZRotation);
			SetSnapper(ref Left, "Left", new Vector3(-otherDistanceX, otherAdjustmentY, 0), otherZRotation);
		}
		else
		{
			Right = Instantiate(this.gameObject);
			Left = Instantiate(this.gameObject);
			Up = Instantiate(this.gameObject);
			Down = Instantiate(this.gameObject);

			SetSnapper(ref Right, "Right", new Vector3(otherDistanceX, otherAdjustmentY, 0), -otherZRotation);
			SetSnapper(ref Left, "Left", new Vector3(-otherDistanceX, otherAdjustmentY, 0), otherZRotation);
			SetSnapper(ref Up, "Up", new Vector3(0, otherDistanceY, 0), 0);
			SetSnapper(ref Down, "Down", new Vector3(0, -otherDistanceY, 0), 0);
		}

		otherDistanceX = 4f;
		otherAdjustmentY = 0f;
		otherZRotation = 0;

		DeleteDuplicatesRecursive(baseTransform, 0);
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
