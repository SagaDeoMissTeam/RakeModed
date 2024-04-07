// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_AnimalNetwork
using UnityEngine;

public class MP_AnimalNetwork : MonoBehaviour
{
	public MP_Animal controller;

	public Vector3 correctPlayerPos = Vector3.zero;

	public Quaternion correctPlayerRot = Quaternion.identity;

	public Vector3 latestPosition = Vector3.zero;

	public float latestPositionTime;

	public Vector3 previousPosition = Vector3.zero;

	public float InterpolationPeriod = 0.3f;

	public float animatorSpeed;

	private Vector3 currentPos;

	private Quaternion currentRot;

	private Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			if (Network.isServer)
			{
				Vector3 value = base.transform.position;
				Quaternion value2 = base.transform.rotation;
				float value3 = controller.animatorProperties.speed;
				bool value4 = controller.animatorProperties.bAttack;
				stream.Serialize(ref value);
				stream.Serialize(ref value2);
				stream.Serialize(ref value3);
				stream.Serialize(ref value4);
			}
		}
		else
		{
			Vector3 value5 = Vector3.zero;
			Quaternion value6 = Quaternion.identity;
			float value7 = 0f;
			bool value8 = false;
			stream.Serialize(ref value5);
			stream.Serialize(ref value6);
			stream.Serialize(ref value7);
			stream.Serialize(ref value8);
			correctPlayerPos = value5;
			correctPlayerRot = value6;
			controller.animatorProperties.speed = value7;
			controller.animatorProperties.bAttack = value8;
		}
	}

	private void Update()
	{
		if (Network.isClient)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * 25f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * 25f);
		}
	}
}
