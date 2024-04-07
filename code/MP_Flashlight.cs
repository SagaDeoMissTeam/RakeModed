// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Flashlight
using UnityEngine;

public class MP_Flashlight : MonoBehaviour
{
	[SerializeField]
	private Light light;

	private void Update()
	{
		if (GetComponent<NetworkView>().isMine && Input.GetButtonDown("Flashlight"))
		{
			GetComponent<NetworkView>().RPC("SetLightState", RPCMode.All);
		}
	}

	[RPC]
	public void SetLightState()
	{
		light.enabled = !light.enabled;
	}
}
