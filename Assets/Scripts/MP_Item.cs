// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MP_Item
using UnityEngine;

public class MP_Item : MonoBehaviour
{
	public new string name;

	public int count;

	public int id;

	public AudioClip pickupSound;

	public virtual void PickupItem()
	{
	}
}
