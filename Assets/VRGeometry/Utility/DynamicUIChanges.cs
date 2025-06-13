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
    Slider motorSpeedSlider;

    [SerializeField]
    TMP_Text motorSpeedSliderLabel;

	[SerializeField]
	Slider freeBuilderAngleSlider;

	[SerializeField]
	TMP_Text freeBuilderAngleSliderLabel;


	private FoldSolution foldSolution;
    
    private int oldestDropDownValue;

    void Start()
    {
        oldestDropDownValue = dropDown.value;
        interactableObjects[oldestDropDownValue].SetActive(true);
		foldSolution = interactableObjects[oldestDropDownValue].GetComponent<FoldSolution>();

        if (motorSpeedSlider != null)
        {
			motorSpeedSliderLabel.text = motorSpeedSlider.value.ToString();
		}
        if (freeBuilderAngleSlider != null)
        {
			freeBuilderAngleSliderLabel.text = freeBuilderAngleSlider.value.ToString();
		}
    }

	private void Update()
	{
		if (oldestDropDownValue != dropDown.value)
        {
            ChangeActiveObject();
        }

        if(freeBuilderAngleSlider != null)
        {
			foldSolution.freeBuildAngle = freeBuilderAngleSlider.value;
			freeBuilderAngleSliderLabel.text = freeBuilderAngleSlider.value.ToString();
		}
		if (motorSpeedSlider != null)
		{
			foldSolution.motorspeed = (int)motorSpeedSlider.value;
			motorSpeedSliderLabel.text = motorSpeedSlider.value.ToString();
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
