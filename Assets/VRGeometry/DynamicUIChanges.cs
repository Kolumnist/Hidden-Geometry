using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DynamicUIChanges : MonoBehaviour
{
    [SerializeField]
    GameObject[] interactableObjects;

    [SerializeField]
	TMP_Dropdown dropDown;

    [SerializeField]
    Toggle toggle;

    private int oldestDropDownValue;

    void Start()
    {
        oldestDropDownValue = dropDown.value;
        interactableObjects[oldestDropDownValue].SetActive(true);
    }

	private void Update()
	{
		if (oldestDropDownValue != dropDown.value)
        {
            ChangeActiveObject();
        }
	}

	public void ChangeActiveObject()
    {
		interactableObjects[oldestDropDownValue].SetActive(false);
		oldestDropDownValue = dropDown.value;
        interactableObjects[oldestDropDownValue].SetActive(true);
	}
}
