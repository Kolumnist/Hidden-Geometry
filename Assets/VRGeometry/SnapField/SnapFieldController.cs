using Assets.MyStuffForNow.Tiles;
using Assets.VRGeometry;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/***
* This class is my way of connecting the different tiles with each other.
* Every tile creates new XRSocketInteractor Fields that I call snappers.
* My Solutions are not necessarily dependend on this approach however they do consider it,
* which is why should anyone want to change this approach completely they would have to look through my Solutions to see if it still works.
* Especially do I make use of the parenting of unity where a snapper has child snappers.
* 
* This also doesnt support anything above 4 edges currently.
*/
public class SnapFieldController : MonoBehaviour
{
	private Transform baseTransform;

	// this snapper objects position and rotation
	private float xDistance = 0;
	private float yDistance = 0;
	private int zRotation = 0;

	// possible new snapper objects
	private GameObject Right;
	private GameObject Down;
	private GameObject Left;
	private GameObject Up;

	// new snapperobjects position and rotation
	private float otherDistanceX = 4f;
	private readonly float otherDistanceY = 4f;
	private float otherAdjustmentY = 0f;
	private float otherZRotation = 0f;

	private void Start()
	{
		baseTransform = this.transform;
		while (!baseTransform.name.StartsWith(Names.Base))
		{
			baseTransform = baseTransform.parent;
		}
	}

	public void CreateSnappers()
	{
		Transform tile = transform.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;
		Transform parentTile = null;
		if (!transform.name.StartsWith(Names.Base))
		{
			parentTile = transform.parent.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;
		}

		SnappersPerTile(tile, parentTile);
		DeleteDuplicatesRecursive(baseTransform);

		otherDistanceX = 4f;
		otherAdjustmentY = 0f;
		otherZRotation = 0;
	}

	/***
	 * Each Tile is handled differently, if a new tile should get introduced there will be another else if...
	 * Tiles may move the snapper to its place and also rotate them or even its own snapper.
	 */
	private void SnappersPerTile(Transform tile, Transform parentTile)
	{
		Debug.Log("Creates: " + transform.name + " which holds a " + parentTile != null ? parentTile.name : "");

		if (tile.name.StartsWith(Names.Circle))
		{
			// We do not create new snapping fields as that would only increase complexity but is not helpful for any 3dobjects net... yet
		}
		else if (tile.name.StartsWith(Names.Quad))
		{
			Quad quad = new Quad();
			xDistance = quad.X_DistanceSnapField;
			yDistance = quad.Y_DistanceSnapField;
			zRotation = quad.Z_RotationSnapField;
			otherDistanceX = quad.X_DistanceNewSnapFields;

			if (parentTile.name.StartsWith(Names.TriangleEqui))
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
					return;
				case Names.Down:
					xDistance = 0f;
					yDistance = 0f;
					zRotation = 0;
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
		else if (tile.name.StartsWith(Names.Square))
		{
			Right = Instantiate(this.gameObject);
			Left = Instantiate(this.gameObject);
			Up = Instantiate(this.gameObject);
			Down = Instantiate(this.gameObject);
			SetNewSnapper(ref Right, Names.Right, new Vector3(otherDistanceX, 0, 0), 0);
			SetNewSnapper(ref Left, Names.Left, new Vector3(-otherDistanceX, 0, 0), 0);
			SetNewSnapper(ref Up, Names.Up, new Vector3(0, otherDistanceY, 0), 0);
			SetNewSnapper(ref Down, Names.Down, new Vector3(0, -otherDistanceY, 0), 0);
		}
		else if (tile.name.StartsWith(Names.TriangleEqui))
		{
			TriangleEquiliteral triangle = new TriangleEquiliteral();
			otherDistanceX = triangle.X_DistanceNewSnapFields;
			otherAdjustmentY = triangle.Y_AdjustmentNewSnapFields;
			otherZRotation = triangle.Z_RotationNewSnapFields;

			// Help making this better appreciated
			bool shouldResetRotation = transform.name.Equals("Base") || parentTile.name.StartsWith(Names.TriangleEqui) == true;
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
		else if (tile.name.StartsWith(Names.TriangleLong))
		{
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

	/***
	 * Checks for snappers that are created near other snappers and are mostly overlapping. 
	 * These snappers are then deleted.
	 * Unfortunately this method searches through ALL snappers starting at the base as you can see in CreateSnappers.
	 */
	private void DeleteDuplicatesRecursive(Transform current)
	{
		float maxDistance = 0.21f;

		// I know this code is the biggest performance cow and also just bad but it works right now and I really dont wanna break it. I hate it too like really hate it.
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

			if (Right != null && Vector3.Distance(baseTransform.position, Right.transform.position) < maxDistance + 0.01f)
			{
				Destroy(Right);
			}
			else if (Left != null && Vector3.Distance(baseTransform.position, Left.transform.position) < maxDistance + 0.01f)
			{
				Destroy(Left);
			}
			else if (Up != null && Vector3.Distance(baseTransform.position, Up.transform.position) < maxDistance + 0.01f)
			{
				Destroy(Up);
			}
			else if (Down != null && Vector3.Distance(baseTransform.position, Down.transform.position) < maxDistance + 0.01f)
			{
				Destroy(Down);
			}
		}
	}

	public void DestructSnappers()
	{
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
}
