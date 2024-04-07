// Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Item
using UnityEngine;

public class Item : MonoBehaviour
{
	public new string name;

	public int count;

	public int id;

	public AudioClip pickupSound;

	public virtual void PickupItem()
	{
	}
}
