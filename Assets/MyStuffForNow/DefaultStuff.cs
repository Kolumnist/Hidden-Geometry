using UnityEngine;

public class DefaultStuff : MonoBehaviour
{
    public void Duplicate()
    {
        GameObject duplicate = Instantiate(gameObject);
        Destroy(duplicate.GetComponent<DefaultStuff>());
    }
}
