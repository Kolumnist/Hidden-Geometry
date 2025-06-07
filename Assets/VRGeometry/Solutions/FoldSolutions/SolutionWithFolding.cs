using Assets.VRGeometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SolutionWithFolding : MonoBehaviour
{
    [SerializeField]
    GameObject entry;

	private float[] angles = { 90, 109.5f };

	private Transform entryInteractable;

	private readonly List<Transform> tileTransforms = new List<Transform>();

	LineRenderer lineRenderer = new LineRenderer();

	bool isLegitimate = true;

    public void StartSolution()
    {
        Recursive(entry.transform);

		entryInteractable = entry.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;

		entry.SetActive(false);
		AdjustRigidbodies();
		
		StartCoroutine(Waait(entryInteractable));
		
		Debug.Log(isLegitimate);
    }

    void Recursive(Transform current)
    {
		if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count == 0)
		{
			return;
		}

		/*if (!current.GetComponent<XRSocketInteractor>().GetOldestInteractableSelected().transform.name.StartsWith("Square"))
		{
			isLegitimate = false;
			return;
		}*/

        if (!current.name.Equals("Base"))
        {
			Transform tile = current.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;
			CreateHinge(current, tile, 109.5f);
			tileTransforms.Add(tile);
		}

		foreach (Transform child in current)
        {
            Recursive(child);
        }
    }

	// Anchors & Axis for Squares
	/*
     * R = [-1 0 0], [0 -1 0]
     * L = [1 0 0], [0 1 0]
     * U = [0 -1 0], [1 0 0]
     * D = [0 1 0], [-1 0 0]
     */

	private void CreateHinge(Transform snapzone, Transform tile, float angle)
    {
		HingeJoint hingeJoint = tile.gameObject.AddComponent<HingeJoint>();
        hingeJoint.connectedBody = snapzone.parent.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Rigidbody>();
        
		Vector3 anchor;
		Vector3 axis;

		(anchor, axis) = GetAnchorAxisTuple(snapzone, tile);

		hingeJoint.anchor = anchor;
		hingeJoint.axis = axis;

		hingeJoint.useMotor = true;
		JointMotor motor = new JointMotor
		{
			targetVelocity = 30,
			force = 100
		};
		hingeJoint.motor = motor;

		hingeJoint.useLimits = true;
		JointLimits limits = new JointLimits
		{
			min = 0,
			max = angle
		};
        hingeJoint.limits = limits;
	}

	private (Vector3, Vector3) GetAnchorAxisTuple(Transform snapzone, Transform tile)
	{
		Vector3 anchor;
		Vector3 axis;

		if(tile.name.StartsWith(Names.TriangleEqui))
		{
			switch (snapzone.name)
			{
				case Names.Right:
					anchor = new Vector3(0, -1, 0);
					axis = new Vector3(1, 0, 0);
					break;
				case Names.Left:
					anchor = new Vector3(0, -1, 0);
					axis = new Vector3(1, 0, 0);
					break;
				case Names.Down:
					anchor = new Vector3(0, -1, 0);
					axis = new Vector3(1, 0, 0);
					break;
				default:
					anchor = Vector3.zero;
					axis = Vector3.zero;
					break;
			}
		}
		else
		{
			switch (snapzone.name)
			{
				case Names.Right:
					anchor = new Vector3(-1, 0, 0);
					axis = new Vector3(0, -1, 0);
					break;
				case Names.Left:
					anchor = new Vector3(1, 0, 0);
					axis = new Vector3(0, 1, 0);
					break;
				case Names.Up:
					anchor = new Vector3(0, -1, 0);
					axis = new Vector3(1, 0, 0);
					break;
				case Names.Down:
					anchor = new Vector3(0, 1, 0);
					axis = new Vector3(-1, 0, 0);
					break;
				default:
					anchor = Vector3.zero;
					axis = Vector3.zero;
					break;
			}
		}
		return (anchor, axis);
	}

	private IEnumerator Waait(Transform interactable)
	{
		yield return new WaitForSecondsRealtime(4);
		var vectors = new List<Vector3>
		{
			Vector3.up,
			Vector3.down,
			Vector3.left,
			Vector3.right,
			Vector3.forward,
			Vector3.back
		};

		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.startWidth = 0.03f;
		lineRenderer.endWidth = 0.03f;
		lineRenderer.material = new Material(Shader.Find("Unlit/Color"))
		{
			color = Color.yellow
		};
		//lineRenderer.useWorldSpace = true;
		lineRenderer.positionCount = 12;

		//This has to happen after some time like 3 seconds or smth
		for (int i = 0; i < 6; i++)
		{
			//layerforeverythingandstuff
			RaycastHit hit;
			if (Physics.Raycast(interactable.transform.position - new Vector3(0, -0.15f, 0), interactable.transform.TransformDirection(vectors[i]), out hit, 1f))
			{
				Debug.Log("Did Hit: " + hit.transform.name);
			}
			else
			{
				isLegitimate = false;
			}

			lineRenderer.SetPosition(i * 2, interactable.transform.position - new Vector3(0, 0, -0.15f));
			lineRenderer.SetPosition(i * 2 + 1, interactable.transform.position + interactable.transform.TransformDirection(vectors[i]).normalized);
		}
	}

	private IEnumerator Waaaaaaait(Transform tile)
	{
		yield return new WaitForSeconds(0.5f);
		tile.GetComponent<Rigidbody>().useGravity = false;
		tile.GetComponent<Rigidbody>().isKinematic = true;
		entry.SetActive(true);
	}

	public void ResetSolution()
	{
		JointLimits limits = new JointLimits
		{
			min = 0,
			max = 0
		};
		foreach (Transform tile in tileTransforms)
		{
			tile.GetComponent<HingeJoint>().limits = limits;
			StartCoroutine(Waaaaaaait(tile));
			Destroy(tile.GetComponent<HingeJoint>(), 1);
		}

		//delete the hingejoints AND change the rigidbodies again. (Order idk yet youll figure it out.)
		// Set active of base/entry
	}

	private void AdjustRigidbodies()
	{
		foreach(Transform tile in tileTransforms)
		{
			tile.GetComponent<Rigidbody>().useGravity = true;
			tile.GetComponent<Rigidbody>().isKinematic = false;
		}
	}
}
