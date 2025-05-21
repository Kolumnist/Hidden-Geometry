using UnityEngine;

public class DebugScript : MonoBehaviour
{

	private new Collider collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<Collider>();
    }

	private void OnCollisionEnter(Collision collision)
	{
		UnityEngine.Debug.Log("Collided with: " + collision.collider + " our collider " + collider);
	}
}
