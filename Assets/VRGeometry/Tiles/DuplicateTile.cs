using UnityEngine;

public class DuplicateTile : MonoBehaviour
{
    //[SerializeField]
    //private string name;

	public void Duplicate()
    {
        /*
        switch(name)
        {
            case "Square":
                
                break;
            case "TriangleEquiliteral":
                
                break;
            case "TriangleLong":
                
                break;
            case "Quad": break;

            case "Circle": break;
        }
        */
        GameObject gameObject = Instantiate(this.gameObject, this.transform.parent);

        Destroy(this.gameObject.GetComponent<Unity.VRTemplate.Rotator>());
        Destroy(this);
    }
}
