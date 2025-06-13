using UnityEngine;

// I JUST WANNA SET THE PARENT I TRIED ALL METHODS IT WONT WOOOOORK ;((((((((((((
public class DuplicateTile : MonoBehaviour
{
	public void Duplicate()
    {
        GameObject gameObject = Instantiate(this.gameObject);
        gameObject.name = this.name;

        Destroy(this.gameObject.GetComponent<Unity.VRTemplate.Rotator>());
        Destroy(this);
    }
}
