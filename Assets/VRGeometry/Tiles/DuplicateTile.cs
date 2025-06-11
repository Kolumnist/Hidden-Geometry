using UnityEngine;

public class DuplicateTile : MonoBehaviour
{
    //[SerializeField]
    //private string name;

	public void Duplicate()
    {
        GameObject gameObject = Instantiate(this.gameObject, this.transform.parent);

        Destroy(this.gameObject.GetComponent<Unity.VRTemplate.Rotator>());
        Destroy(this);
    }
}
