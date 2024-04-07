// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_CanvasManager
using UnityEngine;

public class MP_CanvasManager : MonoBehaviour
{
	[SerializeField]
	private GameObject canvas;

	public GameObject[] weapons;

	public RectTransform playerHealth;

	private void Start()
	{
		canvas = GameObject.Find("Canvas");
	}

	public GameObject GetWeaponCanvas(int id)
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (i == id)
			{
				return weapons[id].gameObject;
			}
		}
		return null;
	}

	private void Update()
	{
	}
}
