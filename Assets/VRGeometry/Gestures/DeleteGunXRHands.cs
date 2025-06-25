using System.Collections;
using UnityEngine;

public class DeleteGunXRHands : MonoBehaviour
{
	[SerializeField]
	Transform indexFingerTip;

	[SerializeField]
	private LineRenderer lineRenderer;

	public void RaycastToTile()
	{
		RaycastHit hit;
		if (Physics.Raycast(indexFingerTip.position, indexFingerTip.forward, out hit, 8f) && 
			hit.transform.CompareTag("Tile"))
		{
			Debug.Log("Did Hit: " + hit.transform.name);
			Destroy(hit.transform.gameObject);
		}

		Vector3 origin = indexFingerTip.position;
		Vector3 direction = indexFingerTip.forward;
		lineRenderer.positionCount = 2;
		lineRenderer.SetPosition(0, origin);
		lineRenderer.SetPosition(1, indexFingerTip.forward*8f);
		
		StartCoroutine(ResetLineRenderer());
	}

	private IEnumerator ResetLineRenderer()
	{
		yield return new WaitForSeconds(0.8f);
		lineRenderer.positionCount = 0;
	}

}
