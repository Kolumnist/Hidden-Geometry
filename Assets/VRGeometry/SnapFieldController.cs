using Assets.MyStuffForNow.Tiles;
using Assets.VRGeometry;
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
			Quad quad = new Quad();
			xDistance = quad.X_DistanceSnapField;
			yDistance = quad.Y_DistanceSnapField;
			zRotation = quad.Z_RotationSnapField;
			otherDistanceX = quad.X_DistanceNewSnapFields;

			if (parentInteractableName.StartsWith(Names.TriangleEqui))
			{
				xDistance = quad.Triangle_X_DistanceSnapField;
				yDistance = quad.Triangle_Y_DistanceSnapField;
				zRotation = quad.Triangle_Z_RotationSnapField;
			}

			Right = Instantiate(gameObject);
			Down = Instantiate(gameObject);
			Left = Instantiate(gameObject);
			Up = Instantiate(gameObject);

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
			SetNewSnapper(ref Right, Names.Right, new Vector3(otherDistanceX, 0, 0), 0);
			SetNewSnapper(ref Left, Names.Left, new Vector3(-otherDistanceX, 0, 0), 0);
			SetNewSnapper(ref Up, Names.Up + "_Large", new Vector3(0, otherDistanceY, 0), 0);
			SetNewSnapper(ref Down, Names.Down + "_Large", new Vector3(0, -otherDistanceY, 0), 0);
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
			SetNewSnapper(ref Right, Names.Right, new Vector3(otherDistanceX, 0, 0), 0);
			SetNewSnapper(ref Left, Names.Left, new Vector3(-otherDistanceX, 0, 0), 0);
			SetNewSnapper(ref Up, Names.Up, new Vector3(0, otherDistanceY, 0), 0);
			SetNewSnapper(ref Down, Names.Down, new Vector3(0, -otherDistanceY, 0), 0);
		}
		else if (interactableName.StartsWith(Names.TriangleEqui))
		{
			if (this.name.EndsWith("_Large"))
			{
				GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Renderer>().material = falseMaterial;
				return;
			}
			
			TriangleEquiliteral triangle = new TriangleEquiliteral();
			otherDistanceX = triangle.X_DistanceNewSnapFields;
			otherAdjustmentY = triangle.Y_AdjustmentNewSnapFields;
			otherZRotation = triangle.Z_RotationNewSnapFields;

			// Help making this better appreciated
			bool shouldResetRotation = transform.name.Equals("Base") || parentInteractableName.StartsWith(Names.TriangleEqui) == true;
			zRotation = shouldResetRotation ? triangle.Z_RotationSnapField : triangle.Triangle_Z_RotationSnapField;
			
			Right = Instantiate(this.gameObject);
			Left = Instantiate(this.gameObject);

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
					SetNewSnapper(ref Down, Names.Down, new Vector3(0, -otherDistanceY, 0), 0);
					break;
			}

			SetNewSnapper(ref Right, Names.Right, new Vector3(otherDistanceX, otherAdjustmentY, 0), -otherZRotation);
			SetNewSnapper(ref Left, Names.Left, new Vector3(-otherDistanceX, otherAdjustmentY, 0), otherZRotation);
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
					SetNewSnapper(ref Down, Names.Down + "_Large", new Vector3(0, -otherDistanceY, 0), 0);
					break;
			}
		}
	}

	private void SetNewSnapper(ref GameObject snapper, string name, Vector3 position, float zRotation)
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
				continue;
			}

			if (Right != null && Vector3.Distance(Right.transform.position, child.position) < maxDistance)
			{
				Destroy(Right);
			}
			else if (Left != null && Vector3.Distance(Left.transform.position, child.position) < maxDistance)
			{
				Destroy(Left);
			}
			else if (Up != null && Vector3.Distance(Up.transform.position, child.position) < maxDistance)
			{
				Destroy(Up);
			}
			else if (Down != null && Vector3.Distance(Down.transform.position, child.position) < maxDistance)
			{
				Destroy(Down);
			}

			if (Right != null && Vector3.Distance(baseTransform.position, Right.transform.position) < maxDistance + 0.1f)
			{
				Destroy(Right);
			}
			else if (Left != null && Vector3.Distance(baseTransform.position, Left.transform.position) < maxDistance + 0.1f)
			{
				Destroy(Left);
			}
			else if (Up != null && Vector3.Distance(baseTransform.position, Up.transform.position) < maxDistance + 0.1f)
			{
				Destroy(Up);
			}
			else if (Down != null && Vector3.Distance(baseTransform.position, Down.transform.position) < maxDistance + 0.1f)
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
