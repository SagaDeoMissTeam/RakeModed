// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// EnableCameraDepthInForward
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class EnableCameraDepthInForward : MonoBehaviour
{
	private void Start()
	{
		Set();
	}

	private void Set()
	{
		if (GetComponent<Camera>().depthTextureMode == DepthTextureMode.None)
		{
			GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
		}
	}
}
