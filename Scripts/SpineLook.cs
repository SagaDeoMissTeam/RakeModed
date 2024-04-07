// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// SpineLook
using UnityEngine;

public class SpineLook : MonoBehaviour
{
	public Animator anim;

	public new Transform transform;

	public Vector3 pos;

	public Vector3 nn = new Vector3(0f, -68f, -100f);

	private void Start()
	{
		nn = new Vector3(0f, -90f, -90f);
	}

	private void LateUpdate()
	{
		GetComponent<Transform>().LookAt(pos);
		GetComponent<Transform>().Rotate(nn, Space.Self);
	}
}
