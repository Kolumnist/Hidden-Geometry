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
		[SerializeField]
		private GameObject entryBase;
		[SerializeField]
		private GameObject bigFoldObject;

		public int motorspeed = 100;
		public float freeBuildAngle = 90;
		
		internal readonly List<Transform> tileTransforms = new List<Transform>();
		
		internal bool isCorrect = true;

		public void StartFolding()
		{
			Recursive(entryBase.transform);

			Transform entryTransform = entryBase.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;
			entryBase.SetActive(false);
			// XRSocketInteractor locks the RigidBody... I have to do it like this
			foreach (Transform tile in tileTransforms)
			{
				tile.GetComponent<Rigidbody>().useGravity = true;
				tile.GetComponent<Rigidbody>().isKinematic = false;
				tile.GetComponent<Collider>().isTrigger = true;
			}
			tileTransforms.Add(entryTransform);

			StartCoroutine(FinalCheckAfterWaitForFold());
		}

		private void Recursive(Transform current)
		{
			if (current.GetComponent<XRSocketInteractor>().interactablesSelected.Count == 0 || !isCorrect)
			{
				return;
			}

			Transform tile = current.GetComponent<XRSocketInteractor>().interactablesSelected[0].transform;
			CheckRequirements(current, tile);

			foreach (Transform child in current)
			{
				Recursive(child);
			}
		}

		/***
		 * There are specific requirements for each object that need to be adressed and are not general.
		 * They basically set the isCorrect false if they are not met.
		 * 
		 * Also important "Base" will not have a hinge.
		 */
		internal virtual void CheckRequirements(Transform snapzone, Transform tile) 
		{
			if (!snapzone.name.Equals(Names.Base))
			{
				CreateHinge(snapzone, tile, freeBuildAngle);
				tileTransforms.Add(tile);
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

			hingeJoint.motor = new JointMotor { targetVelocity = motorspeed / 2, force = motorspeed };
			hingeJoint.useMotor = true;

			hingeJoint.limits = new JointLimits { min = 0, max = angleMaxLimit };
			hingeJoint.useLimits = true;
		}

		// This Method is a nightmare
		internal (Vector3, Vector3) GetAnchorAxisTuple(Transform snapzone, Transform tile)
		{
			Vector3 anchor;
			Vector3 axis;

			if (tile.name.StartsWith(Names.TriangleEqui))
			{
				anchor = new Vector3(0, -1, 0);
				axis = new Vector3(1, 0, 0);
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
						else if(tile.name.StartsWith(Names.Quad))
						{
							anchor = new Vector3(-1.5f, 0, 0);
							axis = new Vector3(0, -1, 0);
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
						else if (tile.name.StartsWith(Names.Quad))
						{
							anchor = new Vector3(1.5f, 0, 0);
							axis = new Vector3(0, 1, 0);
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
			// Change Wait to some kinda trigger
			yield return new WaitForSecondsRealtime(4f);

			foreach (Transform transform in tileTransforms)
			{
				foreach (Transform edges in transform)
				{
					EdgeCorrectTrigger ect = edges.GetComponent<EdgeCorrectTrigger>();
					if (ect != null && (!ect.isConnected || ect.overlaps > 0))
					{
						isCorrect = false;
						break;
					}
				}
			}

			if (isCorrect)
			{
				correctParticle.Play();
				bigFoldObject.SetActive(true);
			}
		}

		public void ResetFolding()
		{
			foreach (Transform tile in tileTransforms)
			{
				if(tile.GetComponent<HingeJoint>() != null)
				{
					tile.GetComponent<HingeJoint>().limits = new JointLimits { min = 0, max = 0 };
					StartCoroutine(ResetTileAfterWait(tile));
					Destroy(tile.GetComponent<HingeJoint>(), 0.35f);
				}
			}
			StartCoroutine(SetBaseActiveAfterWait());
			isCorrect = true;
			tileTransforms.Clear();

			AdditionalReset();
		}

		private IEnumerator ResetTileAfterWait(Transform tile)
		{
			yield return new WaitForSeconds(0.25f);
			tile.GetComponent<Rigidbody>().useGravity = false;
			tile.GetComponent<Rigidbody>().isKinematic = true;
			tile.GetComponent<Collider>().isTrigger = false;
		}

		private IEnumerator SetBaseActiveAfterWait()
		{
			yield return new WaitForSeconds(0.35f); 
			entryBase.SetActive(true);
		}

		internal virtual void AdditionalReset() { }
	}
}
