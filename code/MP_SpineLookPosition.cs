// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_SpineLookPosition
using UnityEngine;

public class MP_SpineLookPosition : MonoBehaviour
{
	[SerializeField]
	private SpineLook spine;

	public Quaternion correctCamRot = Quaternion.identity;

	private void Start()
	{
	}

	private void Update()
	{
		Ray ray = new Ray(base.transform.position, base.transform.forward);
		spine.pos = ray.GetPoint(10f);
	}
}
