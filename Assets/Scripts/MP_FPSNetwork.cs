// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_FPSNetwork
using UnityEngine;

public class MP_FPSNetwork : MonoBehaviour
{
	public MP_FPSController controller;

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
			float value3 = controller.horizontalInput;
			float value4 = controller.verticalInput;
			float value5 = controller.speed;
			stream.Serialize(ref value3);
			stream.Serialize(ref value4);
			stream.Serialize(ref value5);
		}
		else
		{
			Vector3 value6 = Vector3.zero;
			Quaternion value7 = Quaternion.identity;
			stream.Serialize(ref value6);
			stream.Serialize(ref value7);
			correctPlayerPos = value6;
			correctPlayerRot = value7;
			float value8 = 0f;
			float value9 = 0f;
			float value10 = 0f;
			stream.Serialize(ref value8);
			stream.Serialize(ref value9);
			stream.Serialize(ref value10);
			controller.horizontalInput = value8;
			controller.verticalInput = value9;
			controller.speed = value10;
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
