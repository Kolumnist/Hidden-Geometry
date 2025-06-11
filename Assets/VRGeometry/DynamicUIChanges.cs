using Assets.VRGeometry.Solutions;
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

    [SerializeField]
    Slider slider;

    [SerializeField]
    TMP_Text sliderLabel;

    private FoldSolution foldSolution;
    
    private int oldestDropDownValue;

    void Start()
    {
        oldestDropDownValue = dropDown.value;
        interactableObjects[oldestDropDownValue].SetActive(true);
		foldSolution = interactableObjects[oldestDropDownValue].GetComponent<FoldSolution>();

        if (slider != null)
        {
            sliderLabel.text = "100";
        }
    }

	private void Update()
	{
		if (oldestDropDownValue != dropDown.value)
        {
            ChangeActiveObject();
        }
        if(slider != null)
        {
			foldSolution.motorspeed = (int)slider.value;
			sliderLabel.text = slider.value.ToString();
		}
	}

    public void StartOrResetFold()
    {
        if (toggle.isOn)
        {
            foldSolution.StartFolding();
        }
        else
        {
            foldSolution.ResetFolding();
        }
    }

	public void ChangeActiveObject()
    {
        foldSolution.ResetFolding();
        toggle.isOn = false;

		interactableObjects[oldestDropDownValue].SetActive(false);
		oldestDropDownValue = dropDown.value;
        interactableObjects[oldestDropDownValue].SetActive(true);
        foldSolution = interactableObjects[oldestDropDownValue].GetComponent<FoldSolution>();
	}
}
