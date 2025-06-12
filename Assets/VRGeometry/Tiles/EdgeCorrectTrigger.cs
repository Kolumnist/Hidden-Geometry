using UnityEngine;

public class EdgeCorrectTrigger : MonoBehaviour
{
    public bool isConnected = false;
	public int overlaps = -1;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("EdgeCollider"))
		{
			isConnected = true;
			overlaps++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("EdgeCollider"))
		{
			isConnected = false;
			overlaps--;
		}
	}
}
