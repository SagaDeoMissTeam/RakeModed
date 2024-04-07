// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_RPSNetwork
using UnityEngine;

public class MP_RPSNetwork : MonoBehaviour
{
	public MP_RPSController controller;

	public Vector3 correctPlayerPos = Vector3.zero;

	public Quaternion correctPlayerRot = Quaternion.identity;

	private Vector3 currentPos;

	private Quaternion currentRot;

	private void Start()
	{
		if (GetComponent<NetworkView>().isMine)
		{
			controller.EnableControllScripts();
		}
		else
		{
			controller.DisableControllScripts();
		}
	}

	private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 value = base.transform.position;
			Quaternion value2 = base.transform.rotation;
			stream.Serialize(ref value);
			stream.Serialize(ref value2);
			bool value3 = controller.grounded;
			float value4 = controller.horizontalInput;
			float value5 = controller.verticalInput;
			float value6 = controller.speed;
			stream.Serialize(ref value4);
			stream.Serialize(ref value5);
			stream.Serialize(ref value6);
			stream.Serialize(ref value3);
		}
		else
		{
			Vector3 value7 = Vector3.zero;
			Quaternion value8 = Quaternion.identity;
			stream.Serialize(ref value7);
			stream.Serialize(ref value8);
			correctPlayerPos = value7;
			correctPlayerRot = value8;
			float value9 = 0f;
			float value10 = 0f;
			bool value11 = false;
			float value12 = 0f;
			stream.Serialize(ref value9);
			stream.Serialize(ref value10);
			stream.Serialize(ref value12);
			stream.Serialize(ref value11);
			controller.grounded = value11;
			controller.horizontalInput = value9;
			controller.verticalInput = value10;
			controller.speed = value12;
		}
	}

	private void Update()
	{
		if (!GetComponent<NetworkView>().isMine)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, correctPlayerPos, Time.deltaTime * 15f);
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, correctPlayerRot, Time.deltaTime * 15f);
		}
	}
}
