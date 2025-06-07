using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace Assets.VRGeometry.Solutions
{
	public abstract class FoldSolution : MonoBehaviour
	{
		[SerializeField]
		ParticleSystem correctParticle;

		public GameObject entryBase;
		public int motorspeed = 100;
		
		internal readonly List<Transform> tileTransforms = new List<Transform>();
		internal bool isCorrect = true;

		internal virtual void Recursive(Transform current) { }

		public void StartFolding()
		{
			Recursive(entryBase.transform);
			entryBase.SetActive(false);

			foreach (Transform tile in tileTransforms)
			{
				tile.GetComponent<Rigidbody>().useGravity = true;
				tile.GetComponent<Rigidbody>().isKinematic = false;
				tile.GetComponent<Collider>().isTrigger = true;
			}

			if (isCorrect)
			{
				StartCoroutine(FinalCheckAfterWaitForFold());
			}
			else
			{
				Debug.Log(isCorrect);
				ResetFolding();
			}
		}

		internal void CreateHinge(Transform snapzone, Transform tile, float angleMaxLimit)
		{
			HingeJoint hingeJoint = tile.gameObject.AddComponent<HingeJoint>();
			hingeJoint.connectedBody = snapzone.parent.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.GetComponent<Rigidbody>();

			Vector3 anchor;
			Vector3 axis;
			(anchor, axis) = GetAnchorAxisTuple(snapzone, tile);
			hingeJoint.anchor = anchor;
			hingeJoint.axis = axis;

			hingeJoint.motor = new JointMotor { targetVelocity = motorspeed/3, force = motorspeed };
			hingeJoint.useMotor = true;
			
			hingeJoint.limits = new JointLimits { min = 0, max = angleMaxLimit};
			hingeJoint.useLimits = true;
		}

		internal (Vector3, Vector3) GetAnchorAxisTuple(Transform snapzone, Transform tile)
		{
			Vector3 anchor;
			Vector3 axis;

			if (tile.name.StartsWith(Names.TriangleEqui))
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
					case Names.Up:
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
						if (snapzone.parent.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.name.StartsWith(Names.TriangleEqui))
						{
							anchor = new Vector3(0, -1, 0);
							axis = new Vector3(1, 0, 0);
						}
						else
						{
							anchor = new Vector3(-1, 0, 0);
							axis = new Vector3(0, -1, 0);
						}
						break;
					case Names.Left:
						if (snapzone.parent.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform.name.StartsWith(Names.TriangleEqui))
						{
							anchor = new Vector3(0, -1, 0);
							axis = new Vector3(1, 0, 0);
						}
						else
						{
							anchor = new Vector3(1, 0, 0);
							axis = new Vector3(0, 1, 0);
						}
						break;
					case Names.Up:
						anchor = new Vector3(0, -1, 0);
						axis = new Vector3(1, 0, 0);
						break;
					case Names.Down:
						anchor = new Vector3(0, 1, 0);
						axis = new Vector3(-1, 0, 0);
						break;
					case Names.Up + "_Large":
						anchor = new Vector3(0, -1, 0);
						axis = new Vector3(1, 0, 0);
						break;
					case Names.Down + "_Large":
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

		private IEnumerator FinalCheckAfterWaitForFold()
		{
			yield return new WaitForSecondsRealtime(3.5f);
			
			List<Vector3> foldedPositions = new List<Vector3>();
			foreach(Transform tile in tileTransforms)
			{
				foreach(Vector3 position in foldedPositions)
				{
					if (Vector3.Distance(tile.position, position) < 0.1)
					{
						isCorrect = false;
						break;
					}
				}
				if (!isCorrect)
				{
					break;
				}
				foldedPositions.Add(tile.position);
			}
			Debug.Log(isCorrect);
			if (isCorrect)
			{
				correctParticle.Play();
			}

			/*var vectors = new List<Vector3>
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
			}*/
		}

		public void ResetFolding()
		{
			foreach (Transform tile in tileTransforms)
			{
				tile.GetComponent<HingeJoint>().limits = new JointLimits { min = 0, max = 0 };
				StartCoroutine(ResetAfterWait(tile));
				Destroy(tile.GetComponent<HingeJoint>(), 0.75f);
			}
		}

		private IEnumerator ResetAfterWait(Transform tile)
		{
			yield return new WaitForSeconds(0.25f);
			tile.GetComponent<Rigidbody>().useGravity = false;
			tile.GetComponent<Rigidbody>().isKinematic = true;
			tile.GetComponent<Collider>().isTrigger = false;
			entryBase.SetActive(true);
			isCorrect = true;
			tileTransforms.Clear();
		}
	}
}
