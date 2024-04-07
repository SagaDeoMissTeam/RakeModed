// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_ConnectionsCounter
using UnityEngine;

public class MP_ConnectionsCounter : MonoBehaviour
{
	public int connections;

	public int currentConnections;

	private static MP_ConnectionsCounter ThisInstance;

	public static MP_ConnectionsCounter DefaultInstance = ThisInstance;

	private void Awake()
	{
		if (ThisInstance != null)
		{
			Object.DestroyImmediate(base.gameObject);
			return;
		}
		ThisInstance = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	[RPC]
	public void AddConnection()
	{
		currentConnections++;
		if (currentConnections <= connections)
		{
		}
	}

	[RPC]
	public void SetConnection(int count)
	{
		connections = count;
	}
}
