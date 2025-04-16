using System;
using System.Collections.Generic;
using UnityEngine;

public class SnapFieldController : MonoBehaviour
{
	[SerializeField] 
	private Transform baseTransform;

	GameObject R;
	GameObject D;
	GameObject L;
	GameObject U;

	private void CheckForDuplicationRecursive(Transform current, int currentlyFound)
	{
		foreach (Transform child in current)
		{
			if (currentlyFound == 3 || current == this.transform)
			{
				break;
			}

			if (R != null && Vector3.Distance(R.transform.position, child.position) < 0.01f)
			{
				R.SetActive(false);
				currentlyFound++;
				continue;
			}
			else if (D != null && Vector3.Distance(D.transform.position, child.position) < 0.01f)
			{
				D.SetActive(false);
				currentlyFound++;
				continue;
			}
			else if (L != null && Vector3.Distance(L.transform.position, child.position) < 0.01f)
			{
				L.SetActive(false);
				currentlyFound++;
				continue;
			}
			else if (U != null && Vector3.Distance(U.transform.position, child.position) < 0.01f)
			{
				U.SetActive(false);
				currentlyFound++;
				continue;
			}

			CheckForDuplicationRecursive(child, currentlyFound);
		}
	}

	private void InstantiateAndSetObjects()
	{
		//if(name != "Base") baseTransform = GetComponentInParent<SnapFieldController>().baseTransform;
	
		if (name != "L")
		{
			R.name = "R";
			R.transform.SetPositionAndRotation(new Vector3(1.2f, 0, 0), Quaternion.identity);
			R.transform.localScale = new Vector3(1f, 1f, 1f);
			R.transform.SetParent(transform, false);
		}

		if (name != "U")
		{
			D.name = "D";
			D.transform.SetPositionAndRotation(new Vector3(0, -1.2f, 0), Quaternion.identity);
			D.transform.localScale = new Vector3(1f, 1f, 1f);
			D.transform.SetParent(transform, false);
		}

		if (name != "R")
		{
			L.name = "L";
			L.transform.SetPositionAndRotation(new Vector3(-1.2f, 0, 0), Quaternion.identity);
			L.transform.localScale = new Vector3(1f, 1f, 1f);
			L.transform.SetParent(transform, false);
		}

		if (name != "D")
		{
			U.name = "U";
			U.transform.SetPositionAndRotation(new Vector3(0, 1.2f, 0), Quaternion.identity);
			U.transform.localScale = new Vector3(1f, 1f, 1f);
			U.transform.SetParent(transform, false);
		}
	}

	public void DestructSnappers()
	{
		Debug.Log("Begin Destructing");

		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	public void CreateSnappers()
	{
		Debug.Log("Begin Creating");
		Debug.Log(transform.name);

		// I know this is dumb but it is in my mind necessary as I need the Instantiation before I set any position.
		// Also I need to duplicate or put this script on another object in the world the entire time, second option however is weird.
		switch (this.name)
		{
			case "Base":
				R = Instantiate(gameObject);
				D = Instantiate(gameObject);
				L = Instantiate(gameObject);
				U = Instantiate(gameObject);

				InstantiateAndSetObjects();
				break;
			case "R":
				R = Instantiate(gameObject);
				D = Instantiate(gameObject);
				U = Instantiate(gameObject);

				InstantiateAndSetObjects();
				break;
			case "D":
				R = Instantiate(gameObject);
				D = Instantiate(gameObject);
				L = Instantiate(gameObject);

				InstantiateAndSetObjects();
				break;
			case "L":
				D = Instantiate(gameObject);
				L = Instantiate(gameObject);
				U = Instantiate(gameObject);

				InstantiateAndSetObjects();
				break;
			case "U":
				R = Instantiate(gameObject);
				L = Instantiate(gameObject);
				U = Instantiate(gameObject);

				InstantiateAndSetObjects(); 
				break;
			default:
				break;
		}
		CheckForDuplicationRecursive(transform.root, 0);
	}
}
