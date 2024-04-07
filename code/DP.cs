// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// DP
using UnityEngine;

public class DP : MonoBehaviour
{
	public RectTransform rt;

	public float scale;

	public float scaleZ;

	public Vector3 vec;

	private void Start()
	{
	}

	private void Update()
	{
		Vector3 position = base.transform.position;
		float x = position.z / -100f;
		float y = position.x / 100f;
		vec.x = x;
		vec.y = y;
		rt.anchoredPosition = vec;
		rt.localEulerAngles = new Vector3(0f, 0f, 0f - base.transform.eulerAngles.y);
	}
}
