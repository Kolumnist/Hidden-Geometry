using UnityEngine;

public class DuplicatePrefab : MonoBehaviour
{
	public void Duplicate()
    {
        GameObject gameObject = Instantiate(this.gameObject, this.transform.parent);

        Destroy(this.gameObject.GetComponent<Unity.VRTemplate.Rotator>());
        Destroy(this);
    }
}
