using System.Collections.Generic;
using UnityEngine;

public class ObjectMaterialChanger : MonoBehaviour
{
	// Change this later on when you find more use for it and if not then make it a lil more compact ty
	[SerializeField]
	private List<Renderer> _renderer = new List<Renderer>();

	[SerializeField]
	private Material _correctMaterial;

	[SerializeField]
	private Material _falseMaterial;

	public ObjectMaterialChanger(Material correctMaterial, Material falseMaterial)
	{
		_correctMaterial = correctMaterial;
		_falseMaterial = falseMaterial;
	}

	public void ChangeMaterial(bool isCorrect)
	{
		foreach (Renderer renderer in _renderer)
		{
			renderer.material = isCorrect ? _correctMaterial : _falseMaterial;
		}
	}
}
