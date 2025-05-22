using System.Collections;
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
	private float yDistance = 0;
	private int zRotation = 0;

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
	}

	public void CreateSnappers()
	{
		string interactableName = GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.name;
		if (interactableName == null)
		{
			Debug.Log("Is Null");
		}

		string parentInteractableName = "";
		if (transform.parent.GetComponent<XRSocketInteractor>() != null)
		{
			parentInteractableName = transform.parent.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.name;
		}

		Debug.Log("Creates: " + transform.name + " which holds a " + interactableName);
		SnappersPerTile(interactableName, parentInteractableName);

		otherDistanceX = 4f;
		otherAdjustmentY = 0f;
		otherZRotation = 0;

		DeleteDuplicatesRecursive(baseTransform);
	}

	private void SnappersPerTile(string interactableName, string parentInteractableName)
	{
		if (interactableName.StartsWith(Names.Circle))
		{
			// We do not create new snapping fields as that would only increase complexity but is not helpful for any 3dobjects net
		}
		else if (interactableName.StartsWith(Names.Quad))
		{
			xDistance = 1f;
			yDistance = 0f;
			zRotation = 0;
			if (parentInteractableName.StartsWith(Names.TriangleEqui))
			{
				xDistance = 0.86f;
				yDistance = 0.5f;
				zRotation = 90;
			}
			switch (this.name)
			{
				case Names.Right:
					this.transform.localPosition += new Vector3(xDistance, yDistance, 0);
					this.transform.Rotate(new Vector3(0, 0, zRotation));
					break;
				case Names.Left:
					this.transform.localPosition += new Vector3(xDistance = -xDistance, yDistance, 0);
					this.transform.Rotate(new Vector3(0, 0, zRotation = -zRotation));
					break;
				case Names.Up:
					xDistance = 0f;
					yDistance = 0f;
					zRotation = 0;
					GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Renderer>().material = falseMaterial;
					return;
				case Names.Down:
					xDistance = 0f;
					yDistance = 0f;
					zRotation = 0;
					GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Renderer>().material = falseMaterial;
					return;
				default:
					xDistance = 0f;
					break;
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
			otherDistanceX = 2.73f;
			otherAdjustmentY = 0.73f;
			otherZRotation = 60;

			// Help making this better appreciated
			bool shouldResetRotation = transform.name.Equals("Base") || parentInteractableName.StartsWith(Names.TriangleEqui) == true;
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

			SetSnapper(ref Right, Names.Right, new Vector3(otherDistanceX, otherAdjustmentY, 0), -otherZRotation);
			SetSnapper(ref Left, Names.Left, new Vector3(-otherDistanceX, otherAdjustmentY, 0), otherZRotation);
		}
		else if (interactableName.StartsWith(Names.TriangleLong))
		{
			if (!this.name.EndsWith("_Large") && !this.name.Equals("Base"))
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

			if (Right != null && 
				(Vector3.Distance(Right.transform.position, child.position) < maxDistance || (child.name.StartsWith(Names.Right) && Vector3.Distance(baseTransform.position, Right.transform.position) < maxDistance)))
			{
				Destroy(Right);
			}
			else if (Left != null &&
				(Vector3.Distance(Left.transform.position, child.position) < maxDistance || (child.name.StartsWith(Names.Left) && Vector3.Distance(baseTransform.position, Left.transform.position) < maxDistance)))
			{
				Destroy(Left);
			}
			else if (Up != null &&
				(Vector3.Distance(Up.transform.position, child.position) < maxDistance || (child.name.StartsWith(Names.Up) && Vector3.Distance(baseTransform.position, Up.transform.position) < maxDistance)))
			{
				Destroy(Up);
			}
			else if (Down != null && 
				(Vector3.Distance(Down.transform.position, child.position) < maxDistance || (child.name.StartsWith(Names.Down) && Vector3.Distance(baseTransform.position, Down.transform.position) < maxDistance)))
			{
				Destroy(Down);
			}
		}
	}
	public void DestructSnappers()
	{
		Debug.Log("Begin Destructing for " + this.gameObject);

		if (xDistance != 0)
		{
			this.transform.localPosition += new Vector3(-xDistance, 0, 0);
			xDistance = 0;
		}
		
		if (yDistance != 0)
		{
			this.transform.localPosition += new Vector3(0, -yDistance, 0);
			yDistance = 0;
		}

		if (zRotation != 0)
		{
			this.transform.Rotate(0, 0, -zRotation);
			zRotation = 0;
		}

		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	public Material falseMaterial;
}
