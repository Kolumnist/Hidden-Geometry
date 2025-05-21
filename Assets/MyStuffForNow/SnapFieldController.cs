using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SnapFieldController : MonoBehaviour
{
	private Transform baseTransform;

	private GameObject Right;
	private GameObject Down;
	private GameObject Left;
	private GameObject Up;

	private float xDistance = 0;
	private int zRotation = 0;

	private bool isDestructing = false;

	// new snapperobjects position and rotation
	private float otherDistanceX = 4f;
	private readonly float otherDistanceY = 4f;
	private float otherAdjustmentY = 0f;
	private float otherZRotation = 0f;

	private struct Names
	{
		public const string Circle = "Circle";
		public const string Quad = "Quad";
		public const string Square = "Square";
		public const string TriangleEqui = "TriangleEquiliteral";
		public const string TriangleLong = "TriangleLong";

		public const string Right = "Right";
		public const string Left = "Left";
		public const string Up = "Up";
		public const string Down = "Down";
	}

	private void Start()
	{
		baseTransform = this.transform;
		while (!baseTransform.name.Equals("Base"))
		{
			baseTransform = baseTransform.parent;
		}
		/*foreach(Transform child in this.transform)
		{
			if(child.GetComponent<SnapFieldController>() == null)
			{
				Destroy(child.gameObject);
				Debug.Log("Hey Interesting");
			}
		}*/
	}

	public void CreateSnappers()
	{
		if (isDestructing) return;
	
		string interactableName = GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.name;
		Debug.Log("Creates: " + transform.name + " which holds a " + interactableName);
		var parentInteractor = transform.parent.GetComponent<XRSocketInteractor>();

		if (interactableName.StartsWith(Names.Circle))
		{
			// We do not create new snapping fields as that would only increase complexity but is not helpful for any 3dobjects net
		}
		else if(interactableName.StartsWith(Names.Quad))
		{
			xDistance = 1f;
			switch (this.name)
			{
				case Names.Right:
					this.transform.localPosition += new Vector3(xDistance, 0, 0);
					break;
				case Names.Left:
					this.transform.localPosition += new Vector3(-xDistance, 0, 0);
					break;
				case Names.Up:
					GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Renderer>().material = falseMaterial;
					return;
				case Names.Down:
					GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Renderer>().material = falseMaterial;
					return;
			}

			Right = Instantiate(gameObject);
			Down = Instantiate(gameObject);
			Left = Instantiate(gameObject);
			Up = Instantiate(gameObject);

			otherDistanceX = 5f;
			SetSnapper(ref Right, Names.Right, new Vector3(otherDistanceX, 0, 0), 0);
			SetSnapper(ref Left, Names.Left, new Vector3(-otherDistanceX, 0, 0), 0);
			SetSnapper(ref Up, Names.Up + "_Large", new Vector3(0, otherDistanceY, 0), 0);
			SetSnapper(ref Down, Names.Down + "_Large", new Vector3(0, -otherDistanceY, 0), 0);
		}
		else if (interactableName.StartsWith(Names.Square))
		{
			if (this.name.EndsWith("_Large"))
			{
				GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Renderer>().material = falseMaterial;
				return;
			}

			Right = Instantiate(this.gameObject);
			Left = Instantiate(this.gameObject);
			Up = Instantiate(this.gameObject);
			Down = Instantiate(this.gameObject);
			SetSnapper(ref Right, Names.Right, new Vector3(otherDistanceX, 0, 0), 0);
			SetSnapper(ref Left, Names.Left, new Vector3(-otherDistanceX, 0, 0), 0);
			SetSnapper(ref Up, Names.Up, new Vector3(0, otherDistanceY, 0), 0);
			SetSnapper(ref Down, Names.Down, new Vector3(0, -otherDistanceY, 0), 0);
		}
		else if (interactableName.StartsWith(Names.TriangleEqui))
		{
			if (this.name.EndsWith("_Large"))
			{
				GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Renderer>().material = falseMaterial;
				return;
			}

			Right = Instantiate(this.gameObject);
			Left = Instantiate(this.gameObject);
			
			// Help making this better appreciated
			bool shouldResetRotation = transform.name.Equals("Base") || parentInteractor.interactablesSelected[0].transform.name.StartsWith(Names.TriangleEqui) == true;
			zRotation = shouldResetRotation ? 0 : 90;
			
			switch (this.name)
			{
				case Names.Right:
					transform.Rotate(new Vector3(0, 0, zRotation = -zRotation));
					break;
				case Names.Left:
					transform.Rotate(new Vector3(0, 0, zRotation));
					break;
				case Names.Up:
					zRotation = 0;
					break;
				case Names.Down:
					transform.Rotate(new Vector3(0, 0, zRotation = 180));
					break;
				default: // Base
					Down = Instantiate(this.gameObject);
					SetSnapper(ref Down, Names.Down, new Vector3(0, -otherDistanceY, 0), 0);
					break;
			}

			otherDistanceX = 2.73f;
			otherAdjustmentY = 0.73f;
			otherZRotation = 60;
			SetSnapper(ref Right, Names.Right, new Vector3(otherDistanceX, otherAdjustmentY, 0), -otherZRotation);
			SetSnapper(ref Left, Names.Left, new Vector3(-otherDistanceX, otherAdjustmentY, 0), otherZRotation);
		}
		else if (interactableName.StartsWith(Names.TriangleLong))
		{	
			if (!this.name.EndsWith("_Large") || !this.name.Equals("Base"))
			{
				GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Renderer>().material = falseMaterial;
				return;
			}

			switch (this.name)
			{
				case Names.Up + "_Large":
					zRotation = 0;
					break;
				case Names.Down + "_Large":
					transform.Rotate(new Vector3(0, 0, zRotation = 180));
					break;
				default:
					Down = Instantiate(this.gameObject);
					SetSnapper(ref Down, Names.Down + "_Large", new Vector3(0, -otherDistanceY, 0), 0);
					break;
			}
		}

		otherDistanceX = 4f;
		otherAdjustmentY = 0f;
		otherZRotation = 0;

		DeleteDuplicatesRecursive(baseTransform);
	}

	private void SetSnapper(ref GameObject snapper, string name, Vector3 position, float zRotation)
	{
		snapper.name = name;
		snapper.transform.SetPositionAndRotation(position, Quaternion.identity);
		snapper.transform.Rotate(new Vector3(0, 0, zRotation));
		snapper.transform.localScale = new Vector3(1f, 1f, 1f);
		snapper.transform.SetParent(transform, false);
	}

	private void DeleteDuplicatesRecursive(Transform current)
	{
		float maxDistance = 0.21f;
		//if (current == null) return;

		foreach (Transform child in current)
		{
			DeleteDuplicatesRecursive(child);

			if (current == this.transform)
			{
				break;
			}

			var x = Vector3.Distance(baseTransform.position, Right.transform.position);

			if (Right != null && 
				(Vector3.Distance(Right.transform.position, child.position) < maxDistance || Vector3.Distance(baseTransform.position, Right.transform.position) < maxDistance))
			{
				Destroy(Right);
			}
			else if (Left != null && 
				(Vector3.Distance(Left.transform.position, child.position) < maxDistance || Vector3.Distance(baseTransform.position, Left.transform.position) < maxDistance))
			{
				Destroy(Left);
			}
			else if (Up != null && 
				(Vector3.Distance(Up.transform.position, child.position) < maxDistance || Vector3.Distance(baseTransform.position, Up.transform.position) < maxDistance))
			{
				Destroy(Up);
			}
			else if (Down != null && 
				(Vector3.Distance(Down.transform.position, child.position) < maxDistance || Vector3.Distance(baseTransform.position, Down.transform.position) < maxDistance))
			{
				Destroy(Down);
			}
		}
	}
	public void DestructSnappers()
	{
		isDestructing = true;
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
		if (xDistance != 0)
		{
			this.transform.position += new Vector3(-xDistance, 0, 0);
		}

		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}

		isDestructing = false;
	}

	public Material falseMaterial;
}
