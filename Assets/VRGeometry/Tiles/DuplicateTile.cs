using UnityEngine;

public class DuplicateTile : MonoBehaviour
{
    [SerializeField]
    Transform parent;

	public void Duplicate()
    {
        GameObject gameObject = Instantiate(this.gameObject);
        gameObject.name = this.name;

        Destroy(this.gameObject.GetComponent<Unity.VRTemplate.Rotator>());
        Destroy(this);
    }
}
