// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_PlayerController
using UnityEngine;

public class MP_PlayerController : MonoBehaviour
{
	[RPC]
	public virtual void TakeDamage(int amount, string damageType)
	{
	}
}
