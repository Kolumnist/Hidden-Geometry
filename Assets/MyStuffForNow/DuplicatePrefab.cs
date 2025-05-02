using UnityEngine;

public class DuplicatePrefab : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

	[SerializeField]
	private Transform parent;

	public void Duplicate()
    {
        GameObject gameObject = Instantiate(this.gameObject);

        Destroy(this.gameObject.GetComponent<Unity.VRTemplate.Rotator>());
        Destroy(this);
    }
}
