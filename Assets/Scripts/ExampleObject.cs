// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// ExampleObject
using UnityEngine;

public class ExampleObject : MonoBehaviour
{
	public float rotateSpeed = 15f;

	public Material[] materials = new Material[0];

	private bool isRotating;

	private Material currentMaterial;

	private void Awake()
	{
		if (materials.Length == 0)
		{
			Debug.Log("ExampleObject has no materials set");
		}
		else
		{
			currentMaterial = materials[0];
		}
		isRotating = ((Random.Range(0, 2) != 0) ? true : false);
	}

	private void Update()
	{
		if (isRotating)
		{
			base.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
		}
	}

	public void ToggleRotation()
	{
		isRotating = !isRotating;
	}

	public void SetRotatingSpeed(float speed)
	{
		isRotating = true;
		rotateSpeed = speed;
	}

	public void SetMaterial(string name)
	{
		string text = string.Empty;
		for (int i = 0; i < materials.Length; i++)
		{
			if (materials[i].name == name)
			{
				currentMaterial = materials[i];
				if ((bool)GetComponent<Renderer>())
				{
					GetComponent<Renderer>().material = materials[i];
				}
				return;
			}
			text += materials[i].name;
			if (i + 1 != materials.Length)
			{
				text += ", ";
			}
		}
		Debug.Log("Could not find a material named " + name + " in the materials list of the Example Object " + base.gameObject.name + ". Valid names: " + text);
	}

	public string GetCurrentMaterial()
	{
		return currentMaterial.name;
	}
}
