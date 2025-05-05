using UnityEngine;

public class DuplicatePrefab : MonoBehaviour
{
	public void Duplicate()
    {
        GameObject gameObject = Instantiate(this.gameObject);

        Destroy(this.gameObject.GetComponent<Unity.VRTemplate.Rotator>());
        Destroy(this);
    }
}
