using UnityEngine;

public class SnapFieldController : MonoBehaviour
{
	[SerializeField]
	private int childCount;

	private int _childCount;

	private void Start()
	{
		_childCount = transform.childCount;
	}

	//Used here to be called when this object is snapped to another (Snapping creates a child at the target)
	private void OnTransformChildrenChanged()
	{
		var allChildren = transform.GetComponentsInChildren<Transform>();
		var snapChild = transform.GetChild(childCount);


		foreach (var child in allChildren)
		{
			if (child.name.Contains("0"))
			{
				child.gameObject.SetActive(false);
				return;
			}
		}
	}
}
